using PassionProjectMVP_Diarra.Models.ModelViews;
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
       
        /*All the controllers can be automatically generated from:
        Controllers (folder)->Add->Controller-> MVC5 Controller with views, using Entity Framework->
        Add->Select the right Model and database, then give a name to the controller. Keep the 3 fields of Views ticked. 
        */
        
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
            //getting all the location from the database
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
        /// This method provides details about the selected location which ID is given and the list of pupils living in this location
        /// <example>GET: Locations/Details/5</example>
        /// <example>GET: Locations/Details/3</example>
        /// </summary>
        /// <param name="id">ID of the selected location</param>
        /// <returns>Shows information about the location and the list of pupils livind in it</returns>
        // 
        public ActionResult Details(int id)
        {
            //View model containing location and its pupils
            ShowLocation showLocation = new ShowLocation();

            //Find the current location
            string url = "Locationdata/FindLocation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            
            if (response.IsSuccessStatusCode)
            {
                LocationDto SelectedLocation = response.Content.ReadAsAsync<LocationDto>().Result;
                showLocation.location=SelectedLocation;
            }
            else
            {
                return RedirectToAction("Error");
            }

            //List of all the pupils living in the selected location
             url = "LocationData/GetLocationPupils/" + id;
             response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<PupilDto> SelectedPupils = response.Content.ReadAsAsync<IEnumerable<PupilDto>>().Result;
                showLocation.pupils=SelectedPupils;
            }
            else
            {
                return RedirectToAction("Error");
            }
            return View(showLocation);

        }

        /// <summary>
        /// This methods shows the view to create a location
        /// </summary>
        /// <returns>Fields to create a location</returns>
        public ActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// This method creates and saves a new location to the database
        /// <example>POST: Locations/Create</example>
        /// </summary>
        /// <param name="newLocation">Location object to be created and added to the database</param>
        /// <returns>Saves the new location to the database</returns>
        // 
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Location newLocation)
        {
            //Add the new location to the database
            string url = "LocationData/AddLocation";
            HttpContent content = new StringContent(jss.Serialize(newLocation));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
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


        /// <summary>
        /// This method shows the information about the location to edit
        /// <example>GET: Locations/Edit/5</example>
        /// <example>GET: Locations/Edit/1</example>
        /// </summary>
        /// <param name="id">ID of the selected location</param>
        /// <returns>Fields of the location to edit</returns>
        // 
        public ActionResult Edit(int id)
        {
            //Find the selected location from the database
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
        /// <param name="id"> ID of the selected location</param>
        /// <param name="currentLocation">Selected location objet itself</param>
        /// <returns>Updates and saves the location object to the database</returns>
        // 
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Location currentLocation)
        {
            //Updating and saving the current location
            string url = "LocationData/UpdateLocation/" + id;

            HttpContent content = new StringContent(jss.Serialize(currentLocation));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Details", new { id = currentLocation.locId });
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
        /// <param name="id">ID of the selected location</param>
        /// <returns> Shows the selected location</returns>
        // 

        public ActionResult Delete(int id)
        {
            //Getting the current location from the database
            string url = "LocationData/FindLocation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            
            if (response.IsSuccessStatusCode)
            {
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
        /// <returns>Removes the selected location from the database</returns>
        // 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Removing the location from the database
            string url = "LocationData/DeleteLocation/" + id;
            
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
