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


        }


        /// <summary>
        /// This method displays the list of all locations
        /// <example>Locations/LocationList</example>
        /// </summary>
        /// <returns>Display location list</returns>
        /// 
        public ActionResult LocationList()
        {

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


        /// <summary>
        /// This method provides details about the selected location which ID is given
        /// <example>GET: Locations/Details/5</example>
        /// <example>GET: Locations/Details/3</example>
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Shows information about the location</returns>
        // 
        public ActionResult Details(int id)
        {
            
            string url = "Locationdata/FindLocation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            
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
            return View();
        }


        /// <summary>
        /// This method creates and saves a new location to the database
        /// <example>POST: Locations/Create</example>
        /// </summary>
        /// <param name="newLocation"></param>
        /// <returns></returns>
        // 
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Location newLocation)
        {

            string url = "LocationData/AddLocation";
            HttpContent content = new StringContent(jss.Serialize(newLocation));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = newLocation.locId });
               // return RedirectToAction("LocationList");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        /// <summary>
        /// This method shows the information about the location to edit
        /// <example>GET: Locations/Edit/5</example>
        /// <example>GET: Locations/Edit/1</example>
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Fields of the location to edit</returns>
        // 
        public ActionResult Edit(int id)
        {
            string url = "LocationData/FindLocation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                Location SelectedPlayer = response.Content.ReadAsAsync<Location>().Result;
                return View(SelectedPlayer);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        /// <summary>
        /// This method edits and saves the selected location which ID is given
        /// <example>POST: Locations/Edit/5</example>
        /// <example>POST: Locations/Edit/3</example>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentLocation"></param>
        /// <returns></returns>
        // 
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
            
        }

        /// <summary>
        /// This method displays the fields of the location to delete
        /// <example>GET: Locations/Delete/5</example>
        /// <example>GET: Locations/Delete/5</example>
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Shows the selected location</returns>
        // 

        public ActionResult Delete(int id)
        {
            string url = "LocationData/FindLocation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            
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

        /// <summary>
        /// This method removes the selected location from the database
        /// <example>POST: Locations/Delete/5 </example>
        ///  <example>POST: Locations/Delete/2 </example>
        /// </summary>
        /// <param name="id">Id of the selected location</param>
        /// <returns></returns>
        // 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string url = "LocationData/DeleteLocation/" + id;
            //post body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("LocationList");
            }
            else
            {
                return RedirectToAction("Error");
            }
           
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
