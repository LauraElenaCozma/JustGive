using JustGive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JustGive.Controllers
{
    public class CauseController : Controller
    {
        private Models.ApplicationDbContext db = new ApplicationDbContext();
        // GET: Cause
        public ActionResult Index()
        {
            List<Cause> causes = db.Causes.Include("ContactInfo").ToList();
            ViewBag.Causes = causes;
            return View();
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Cause cause = db.Causes.Include("Location").SingleOrDefault(c => c.CauseId == id);

                if (cause != null)
                {
                    return View(cause);
                }
                return HttpNotFound("Couldn't find the cause with id " + id.ToString());
            }
            return HttpNotFound("Missing cause id!");
        }

        [HttpGet]
        public ActionResult New()
        {
            CauseContactViewModel causeContact = new CauseContactViewModel();
            causeContact.Cause = new Cause();
            causeContact.ContactInfo = new ContactInfo();
            causeContact.Cause.TagList = GetAllTags();
            causeContact.Cause.LocationList = getAllLocations();
            causeContact.Cause.Tags = new List<Tag>();
            return View(causeContact);
        }
        
        [HttpPost]
        public ActionResult New(CauseContactViewModel causeContact)
        {
            
            try
            {
                causeContact.Cause.LocationList = getAllLocations();
                //causeContact.Cause.Location = db.Locations.Find(causeContact.Cause.LocationId);
                /*var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors })
                    .ToArray();*/
                ModelState.Remove("Cause.ContactInfo");

                if (ModelState.IsValid)
                {
                    ContactInfo contactInfo = new ContactInfo
                    {
                        Name = causeContact.ContactInfo.Name,
                        PhoneNumber = causeContact.ContactInfo.PhoneNumber
                    };

                    Cause cause = new Cause
                    {
                        Title = causeContact.Cause.Title,
                        Description = causeContact.Cause.Description,
                        LocationId = causeContact.Cause.LocationId,
                        ContactInfo = contactInfo,
                        Tags = new List<Tag>(),
                        LocationList = getAllLocations()
                    };
                    contactInfo.Cause = cause;

                    //save the selected tags
                    var selectedTags = causeContact.Cause.TagList.
                        Where(b => b.IsChecked).ToList();
                    
                    db.ContactInfos.Add(contactInfo);
                    
                    
                    for (int i = 0; i < selectedTags.Count(); i++)
                    {
                        //asign to the donation the selected tags
                        Tag tag = db.Tags.Find(selectedTags[i].Id);
                        cause.Tags.Add(tag);
                    }
                    db.Causes.Add(cause);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(causeContact);
            }
            catch (Exception e)
            {
                return View(causeContact);
            }
        }

        [HttpGet]
        public ActionResult Edit(int ?id)
        {
            if (id.HasValue)
            {
                Cause cause = db.Causes.Find(id);
                if (cause != null)
                {
                    cause.LocationList = getAllLocations();
                    cause.TagList = GetAllTags();
                    foreach (Tag tagChecked in cause.Tags)
                    {
                        //update the checkbox list
                        cause.TagList.FirstOrDefault(t => t.Id == tagChecked.TagId).IsChecked = true;
                    }

                    CauseContactViewModel causeContact = new CauseContactViewModel();
                    causeContact.Cause = cause;
                    causeContact.ContactInfo = cause.ContactInfo;
                    return View(causeContact);
                }
                return HttpNotFound("Couldn't find the cause id " + id.ToString());
            }
            return HttpNotFound("Missing the cause id");
        }

        [HttpPut]
        public ActionResult Edit(int id, CauseContactViewModel causeContactReq)
        {
            //save the tags that where selected into the form
            var selectedTags = causeContactReq.Cause.TagList.Where(t => t.IsChecked).ToList();
            causeContactReq.Cause.LocationList = getAllLocations();
            try
            {
                ModelState.Remove("Cause.ContactInfo");
                if (ModelState.IsValid)
                { 
                    Cause cause = db.Causes.Include("Location").
                        SingleOrDefault(c => c.CauseId.Equals(id));
                    if (TryUpdateModel(cause))
                    {
                        cause.Title = causeContactReq.Cause.Title;
                        cause.Description = causeContactReq.Cause.Description;
                        cause.LocationId = causeContactReq.Cause.LocationId;
                        cause.Donations = causeContactReq.Cause.Donations;
                        cause.Tags.Clear();
                        cause.Tags = new List<Tag>();

                        for (int i = 0; i < selectedTags.Count(); ++i)
                        {
                            Tag tag = db.Tags.Find(selectedTags[i].Id);
                            cause.Tags.Add(tag);
                        }
                        db.SaveChanges();
                    }
                    ContactInfo contactInfo = db.ContactInfos.Find(cause.ContactInfo.Id);
                    if (TryUpdateModel(contactInfo))
                    {
                        contactInfo.Name = causeContactReq.ContactInfo.Name;
                        contactInfo.PhoneNumber = causeContactReq.ContactInfo.PhoneNumber;
                    }
                    return RedirectToAction("Index");
                }
                return View(causeContactReq);
            }
            catch (Exception e)
            {
                return View(causeContactReq);
            }
        }


        [HttpDelete]
        public ActionResult Delete(int? id)
        {
            if(id.HasValue)
            {
                Cause cause = db.Causes.Find(id);
                if(cause != null)
                {
                    db.ContactInfos.Remove(cause.ContactInfo);
                    db.Causes.Remove(cause);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                    
                }
                return HttpNotFound("Couldn't find the cause id " + id.ToString());
            }
            return HttpNotFound("Missing id parameter");
        }

        [NonAction]
        private IEnumerable<SelectListItem> getAllLocations()
        {
            var selectedList = new List<SelectListItem>();
            foreach (var location in db.Locations.ToList())
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