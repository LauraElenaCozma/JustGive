using JustGive.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace JustGive.Controllers
{
    [Authorize]
    public class DonationController : Controller
    {
        private Models.ApplicationDbContext db = new ApplicationDbContext();


        // GET: Donation
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            List<Donation> donations = db.Donations.Include("Tags").ToList();
            ViewBag.Donations = donations;

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Donation donation = db.Donations.Include("Location").Include("User").SingleOrDefault(d => d.DonationId == id);

                if (donation != null)
                {
                    ViewBag.UserId = User.Identity.GetUserId();
                    return View(donation);
                }
                return HttpNotFound("Couldn't find the donation id " + id.ToString());
            }
            return HttpNotFound("Missing donation id!");
        }

        [HttpGet]
        [Authorize(Roles = "Donator, Admin")]
        public ActionResult New()
        {
            Donation donation = new Donation();
            donation.TagList = GetAllTags();
            donation.LocationList = getAllLocations();
            donation.Tags = new List<Tag>();
            return View(donation);
        }

        [HttpPost]
        [Authorize(Roles = "Donator, Admin")]
        public ActionResult New(Donation donation)
        {
            //save the selected tags
            donation.LocationList = getAllLocations();
            var selectedTags = donation.TagList.Where(b => b.IsChecked).ToList();
            try
            {
                if(ModelState.IsValid)
                {
                    
                    donation.Tags = new List<Tag>();
                    for (int i = 0; i < selectedTags.Count(); i++)
                    {
                        //asign to the donation the selected tags
                        Tag tag = db.Tags.Find(selectedTags[i].Id);
                        donation.Tags.Add(tag);
                    }
                    donation.UserId = User.Identity.GetUserId();
                    db.Donations.Add(donation);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(donation);
            }
            catch(Exception e)
            {
                return View(donation);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Donator,Admin")]
        public ActionResult Edit(int? id)
        {
            if(id.HasValue)
            {
                Donation donation = db.Donations.Find(id);
                if (donation != null)
                {
                    if(User.Identity.GetUserId() == donation.UserId || User.IsInRole("Admin"))
                    {
                        donation.LocationList = getAllLocations();
                        donation.TagList = GetAllTags();
                        foreach (Tag tagChecked in donation.Tags)
                        {
                            //update the checkbox list
                            donation.TagList.FirstOrDefault(t => t.Id == tagChecked.TagId).IsChecked = true;
                        }
                        return View(donation);
                    }
                    return RedirectToAction("Index");
                }
                return HttpNotFound("Couldn't find the donation id " + id.ToString());
            }
            return HttpNotFound("Missing the donation id");
        }

        [HttpPut]
        [Authorize(Roles = "Donator,Admin")]
        public ActionResult Edit(int id, Donation donationReq)
        {
            //save the tags that where selected into the form
            var selectedTags = donationReq.TagList.Where(t => t.IsChecked).ToList();
            donationReq.LocationList = getAllLocations();
            try
            {
                if(ModelState.IsValid)
                {
                    Donation donation = db.Donations.Include("Location").
                        SingleOrDefault(d => d.DonationId.Equals(id));
                    if (TryUpdateModel(donation))
                    {
                        donation.Title = donationReq.Title;
                        donation.Description = donationReq.Description;
                        donation.LocationId = donationReq.LocationId;
                        donation.Cause = donationReq.Cause;
                        donation.Tags.Clear();
                        donation.Tags = new List<Tag>();

                        for(int i = 0; i < selectedTags.Count(); ++i)
                        {
                            Tag tag = db.Tags.Find(selectedTags[i].Id);
                            donation.Tags.Add(tag);
                        }
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(donationReq);
            }
            catch(Exception e)
            {
                return View(donationReq);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Donator,Admin")]
        public ActionResult Delete(int? id)
        {
            if(id.HasValue)
            {
                Donation donation = db.Donations.Find(id);

                if (donation != null)
                {
                    if (User.Identity.GetUserId() == donation.UserId || User.IsInRole("Admin"))
                    {
                        db.Donations.Remove(donation);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("Index");
                }
                return HttpNotFound("Couldn't find the donation id " + id.ToString());
            }
            return HttpNotFound("Missing donation id!");
        }

        [HttpGet]
        [Authorize(Roles= "Donator,Admin")]
        public ActionResult AddToCause(int? id)
        {
            if (id.HasValue)
            {
                Donation donation = db.Donations.Include("Location").Include("User").SingleOrDefault(d => d.DonationId == id);

                if (donation != null)
                {
                    if (User.Identity.GetUserId() == donation.UserId || User.IsInRole("Admin"))
                    {
                        List<Cause> causes = db.Causes.Include("ContactInfo").ToList();
                        ViewBag.Causes = causes;
                        ViewBag.donationId = donation.DonationId;
                        return View();
                    }
                    return RedirectToAction("Index");
                }
                return HttpNotFound("Couldn't find the donation id " + id.ToString());
            }
            return HttpNotFound("Missing donation id!");
        }

        [Authorize(Roles = "Admin,Donator")]


        [HttpPut]
        public ActionResult ChooseCause(int? donationId, int? causeId)
        {

            if (donationId.HasValue)
            {
                Donation donation = db.Donations.Find(donationId);
                if (donation != null)
                {
                    if (causeId.HasValue)
                    {
                        Cause cause = db.Causes.Find(causeId);
                        if (cause != null)
                        {
                            if (TryUpdateModel(donation))
                            {
                                donation.IsDonated = true;
                                donation.Cause = cause;

                                if (TryUpdateModel(cause))
                                {
                                    cause.Donations.Add(donation);
                                    db.SaveChanges();
                                    return RedirectToAction("Index");
                                }
                                return HttpNotFound("The cause model couldn't be updated");
                            }
                            return HttpNotFound("The donation model couldn't be updated");
                        }
                        return HttpNotFound("Couldn't find the cause id " + causeId.ToString());
                    }
                    return HttpNotFound("Missing id parameter for cause");
                }
                return HttpNotFound("Couldn't find the donation id " + donationId.ToString());
            }
            return HttpNotFound("Missing id parameter for donation");

        }

        [NonAction]
        private IEnumerable<SelectListItem> getAllLocations()
        {
            var selectedList = new List<SelectListItem>();
            foreach(var location in db.Locations.ToList())
            {
                selectedList.Add(new SelectListItem
                {
                    Value = location.LocationId.ToString(),
                    Text = location.City + ", " + location.Country
                });
            }
            return selectedList;
        }

        [NonAction]
        private List<TagDonationViewModel> GetAllTags()
        {
            var checkboxList = new List<TagDonationViewModel>();
            foreach (var tag in db.Tags.ToList())
            {
                checkboxList.Add(new TagDonationViewModel
                {
                    Id = tag.TagId,
                    Name = tag.Name,
                    IsChecked = false
                });
            }
            return checkboxList;
        }
    }
}