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
        public ActionResult New()
        {
            ContactInfo contactInfo = new ContactInfo();
            return View(contactInfo);
        }

        [HttpPost]
        public ActionResult New(ContactInfo contactInfo)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    db.ContactInfos.Add(contactInfo);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(contactInfo);
            }
            catch(Exception e)
            {
                return View(contactInfo);
            }
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
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
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

        [HttpPut]
        public ActionResult Edit(int id, ContactInfo contactInfoReq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContactInfo contactInfo = db.ContactInfos.SingleOrDefault(ct => ct.Id.Equals(id));
                    //TODO: tryupdatemodel!!!!!!!!!!!!!!!!!
                    if(contactInfo != null)
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
        [HttpDelete]
        public ActionResult Delete(int? id)
        {
            if(id.HasValue)
            {
                ContactInfo contactInfo = db.ContactInfos.Find(id);
                if(contactInfo != null)
                {
                    db.ContactInfos.Remove(contactInfo);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return HttpNotFound("Couldn't find the contact info's id " + id.ToString());
            }
            return HttpNotFound("Missing contact info's id parameter!");
        }
    }
}