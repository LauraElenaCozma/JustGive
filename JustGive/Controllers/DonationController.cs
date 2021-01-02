using JustGive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JustGive.Controllers
{
    public class DonationController : Controller
    {
        private Models.ApplicationDbContext db = new ApplicationDbContext();

        // GET: Donation
        [HttpGet]
        public ActionResult Index()
        {
            List<Donation> donations = db.Donations.Include("Tags").ToList();
            ViewBag.Donations = donations;

            return View();
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Donation donation = db.Donations.Include("Location").SingleOrDefault(d => d.DonationId == id);

                if (donation != null)
                {
                    return View(donation);
                }
                return HttpNotFound("Couldn't find the donation id " + id.ToString());
            }
            return HttpNotFound("Missing donation id!");
        }

        [HttpGet]
        public ActionResult New()
        {
            Donation donation = new Donation();
            donation.TagList = GetAllTags();
            donation.LocationList = getAllLocations();
            donation.Tags = new List<Tag>();
            return View(donation);
        }

        [HttpPost]
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
        public ActionResult Edit(int? id)
        {
            if(id.HasValue)
            {
                Donation donation = db.Donations.Find(id);
                if (donation != null)
                {
                    donation.LocationList = getAllLocations();
                    donation.TagList = GetAllTags();
                    foreach(Tag tagChecked in donation.Tags)
                    {
                        //update the checkbox list
                        donation.TagList.FirstOrDefault(t => t.Id == tagChecked.TagId).IsChecked = true;
                    }
                    return View(donation);
                }
                return HttpNotFound("Couldn't find the donation id " + id.ToString());
            }
            return HttpNotFound("Missing the donation id");
        }

        [HttpPut]
        public ActionResult Edit(int id, Donation donationReq)
        {
            //save the tags that where selected into the form
            var selectedTags = donationReq.TagList.Where(t => t.IsChecked).ToList();
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
        public ActionResult Delete(int? id)
        {
            if(id.HasValue)
            {
                Donation donation = db.Donations.Find(id);

                if (donation != null)
                {
                    db.Donations.Remove(donation);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return HttpNotFound("Couldn't find the donation id " + id.ToString());
            }
            return HttpNotFound("Missing donation id!");
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