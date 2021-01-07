using JustGive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JustGive.Controllers
{
    public class ContactInfoController : Controller
    {
        private Models.ApplicationDbContext db = new ApplicationDbContext();
        // GET: ContactInfo
        public ActionResult Index()
        {
            List<ContactInfo> contactInfos = db.ContactInfos.ToList();
            ViewBag.ContactInfos = contactInfos;
            return View();
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if(id.HasValue)
            {
                ContactInfo contactInfo = db.ContactInfos.Find(id);
                if (contactInfo != null)
                {
                    return View(contactInfo);
                }
                return HttpNotFound("Couldn't find the contact info's id " + id.ToString());
            }
            return HttpNotFound("Missing contact info's id parameter!");
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                ContactInfo contactInfo = db.ContactInfos.Find(id);
                if (contactInfo != null)
                {
                    if(User.IsInRole("Admin"))
                    {
                        return View(contactInfo);
                    }
                }
                return HttpNotFound("Couldn't find the contact info's id " + id.ToString());
            }
            return HttpNotFound("Missing contact info's id parameter!");
        }

        [HttpPut]
        [Authorize(Roles ="Admin")]
        public ActionResult Edit(int id, ContactInfo contactInfoReq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContactInfo contactInfo = db.ContactInfos.SingleOrDefault(ct => ct.Id.Equals(id));
                    if(TryUpdateModel(contactInfo))
                    {
                        contactInfo.Name = contactInfoReq.Name;
                        contactInfo.PhoneNumber = contactInfoReq.PhoneNumber;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(contactInfoReq);
                }
                return View(contactInfoReq);
            }
            catch(Exception e)
            {
                return View(contactInfoReq);
            }
        }
    }
}