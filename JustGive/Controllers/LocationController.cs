using JustGive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JustGive.Controllers
{
    public class LocationController : Controller
    {
        private Models.ApplicationDbContext db = new ApplicationDbContext();
        // GET: Location
        public ActionResult Index()
        {
            List<Location> locations = db.Locations.ToList();

            return View(locations);
        }

        [HttpGet]
        public ActionResult New()
        {
            Location location = new Location();
            return View(location);
        }

        [HttpPost]
        public ActionResult New(Location location)
        {
        
            try
            {
                if(ModelState.IsValid)
                {
                    db.Locations.Add(location);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(location);
            }
            catch(Exception e)
            {
                return View(location);
            }
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if(id.HasValue)
            {
                Location location = db.Locations.Find(id);
                if(location != null)
                {
                    return View(location);
                }
                return HttpNotFound("Couldn't find the location id " + id.ToString());
            }
            return HttpNotFound("Missing location id parameter!");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if(id.HasValue)
            {
                Location location = db.Locations.Find(id);
                if(location != null)
                {
                    return View(location);
                }
                return HttpNotFound("Couldn't find the location id " + id.ToString());
            }
            return HttpNotFound("Missing location id parameter!");
        }

        [HttpPut]
        public ActionResult Edit(int id, Location locationRequest)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    Location location = db.Locations.SingleOrDefault(loc => loc.LocationId.Equals(id));
                    if (TryUpdateModel(location))
                    {
                        location.City = locationRequest.City;
                        location.Country = locationRequest.Country;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(locationRequest);
            }
            catch(Exception e)
            {
                return View(locationRequest);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int? id)
        {
            if(id.HasValue)
            {
                Location location = db.Locations.Find(id);
                if(location != null)
                {
                    db.Locations.Remove(location);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return HttpNotFound("Couldn't find the location id " + id.ToString());
            }
            return HttpNotFound("Missing location id parameter!");
        }

    }
}