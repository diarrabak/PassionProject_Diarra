using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PassionProjectMVP_Diarra.Models
{
    public class LocationsController : Controller
    {
        // private PassionDataContext db = new PassionDataContext();

        /*All the controllers can be automatically generated from:
        Controllers (folder)->Add->Controller-> MVC5 Controller with views, using Entity Framework->
        Add->Select the right Model and database, then give a name to the controller. Keep the 3 fields of Views ticked. 
        */
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static LocationsController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            //change this to match your own local port number
            client.BaseAddress = new Uri("https://localhost:44327/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));


            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

        }


        // GET: Locations
        public ActionResult LocationList()
        {
            //var Locations = db.Locations.Include(m => m.Location);
            //return View(Locations.ToList());

            string url = "LocationData/GetLocations";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<Location> SelectedLocations = response.Content.ReadAsAsync<IEnumerable<Location>>().Result;
                return View(SelectedLocations);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Locations/Details/5
        public ActionResult Details(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Location Location = db.Locations.Find(id);
            //if (Location == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(Location);
            string url = "Locationdata/FindLocation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                Location SelectedLocation = response.Content.ReadAsAsync<Location>().Result;
                return View(SelectedLocation);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Locations/Create
        public ActionResult Create()
        {
            // ViewBag.classId = new SelectList(db.Locations, "classId", "className");
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Location newLocation)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Locations.Add(Location);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //ViewBag.classId = new SelectList(db.Locations, "classId", "className", Location.classId);
            //return View(Location);

            string url = "LocationData/AddLocation";
            HttpContent content = new StringContent(jss.Serialize(newLocation));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                //int modid = response.Content.ReadAsAsync<int>().Result;
                //return RedirectToAction("Details", new { id = modid });
                return RedirectToAction("LocationList");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Locations/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "LocationData/FindLocation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                Location SelectedPlayer = response.Content.ReadAsAsync<Location>().Result;
                return View(SelectedPlayer);
            }
            else
            {
                return RedirectToAction("Error");
            }
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Location Location = db.Locations.Find(id);
            //if (Location == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.classId = new SelectList(db.Locations, "classId", "className", Location.classId);
            //return View(Location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Location currentLocation)
        {

            string url = "LocationData/UpdateLocation/" + id;

            HttpContent content = new StringContent(jss.Serialize(currentLocation));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
            //    if (ModelState.IsValid)
            //    {
            //        db.Entry(Location).State = EntityState.Modified;
            //        db.SaveChanges();
            //        return RedirectToAction("Index");
            //    }
            //    ViewBag.classId = new SelectList(db.Locations, "classId", "className", Location.classId);
            //    return View(Location);
        }

        // GET: Locations/Delete/5

        public ActionResult Delete(int id)
        {
            string url = "LocationData/FindLocation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                Location SelectedLocation = response.Content.ReadAsAsync<Location>().Result;
                return View(SelectedLocation);
            }
            else
            {
                return RedirectToAction("Error");
            }
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Location Location = db.Locations.Find(id);
            //if (Location == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(Location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string url = "LocationData/DeleteLocation/" + id;
            //post body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("LocationList");
            }
            else
            {
                return RedirectToAction("Error");
            }
            //Location Location = db.Locations.Find(id);
            //db.Locations.Remove(Location);
            //db.SaveChanges();
            //return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
