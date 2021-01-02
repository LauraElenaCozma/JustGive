using JustGive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JustGive.Controllers
{
    public class TagController : Controller
    {
        // GET: Tag
        private Models.ApplicationDbContext db = new ApplicationDbContext();
        [HttpGet]
        public ActionResult Index()
        {
            List<Tag> tags = db.Tags.ToList();
            return View(tags);
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if(id.HasValue)
            {
                Tag tag = db.Tags.Find(id);
                if(tag != null)
                {
                    return View(tag);
                }
                return HttpNotFound("Couldn't find the tag id " + id.ToString()); 
            }
            return HttpNotFound("Missing donation id!");
        }

        [HttpGet]
        public ActionResult New()
        {
            Tag tag = new Tag();
            return View(tag);
        }

        [HttpPost]
        public ActionResult New(Tag tag)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    db.Tags.Add(tag);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(tag);
            }
            catch(Exception e)
            {
                return View(tag);
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if(id.HasValue)
            {
                Tag tag = db.Tags.Find(id);

                if(tag != null)
                {
                    return View(tag);
                }
                return HttpNotFound("Couldn't find the tag with id " + id.ToString());
            }
            return HttpNotFound("Missing tag id parameter!");
        }

        [HttpPut]
        public ActionResult Edit(int id, Tag tag)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    Tag t = db.Tags.SingleOrDefault(tg => tg.TagId.Equals(id));
                    if(TryUpdateModel(t))
                    {
                        t.Name = tag.Name;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(tag);
            }
            catch(Exception e)
            {
                return View(tag);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int? id)
        {
            if(id.HasValue)
            {
                Tag tag = db.Tags.Find(id);
                if(tag != null)
                {
                    db.Tags.Remove(tag);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return HttpNotFound("Couldn't find the tag with id " + id.ToString());
            }
            return HttpNotFound("Missing tag id parameter");
        }
    }
}