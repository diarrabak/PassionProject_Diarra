using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using PassionProjectMVP_Diarra.Models;
using PassionProjectMVP_Diarra.Models.ModelViews;

namespace PassionProjectMVP_Diarra.Controllers
{
    public class PupilsController : Controller
    {
        //private PassionDataContext db = new PassionDataContext();

        /*All the controllers can be automatically generated from:
        Controllers (folder)->Add->Controller-> MVC5 Controller with views, using Entity Framework->
        Add->Select the right Model and database, then give a name to the controller. Keep the 3 fields of Views ticked. 
        */
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static PupilsController()
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
        /// This method displays the list of all pupils with ther Classes and Locations
        /// <example>Pupils/PupilList</example>
        /// </summary>
        /// <returns>Pupils list with Classes and Locations</returns>
        public ActionResult PupilList()
        {
            //Getting the list of all pupils with their information
            string url = "PupilData/GetPupils";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ShowPupil> SelectedPupils = response.Content.ReadAsAsync<IEnumerable<ShowPupil>>().Result;
                return View(SelectedPupils);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// This method shows the information of the selected pupil
        /// <example>Pupils/Details/1</example>
        /// <example>Pupils/Details/4</example>
        /// </summary>
        /// <param name="id">ID of the selected pupil</param>
        /// <returns>Details of the Pupil whose ID is given</returns>
        
        public ActionResult Details(int id)
        {
            
            ShowPupil showPupil = new ShowPupil();

            //Get the current pupil from the database
            string url = "Pupildata/FindPupil/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            
            if (response.IsSuccessStatusCode)
            {
                PupilDto SelectedPupil = response.Content.ReadAsAsync<PupilDto>().Result;
                showPupil.pupil=SelectedPupil;

                //Get the Classe to which the pupil belongs
                url = "PupilData/GetPupilClasse/" + id;
                response = client.GetAsync(url).Result;
                ClasseDto SelectedClasse = response.Content.ReadAsAsync<ClasseDto>().Result;
                showPupil.classe = SelectedClasse;

                //Get the location of the selected pupil
                url = "PupilData/GetPupilLocation/" + id;
                response = client.GetAsync(url).Result;
                LocationDto SelectedLocation = response.Content.ReadAsAsync<LocationDto>().Result;
                showPupil.location = SelectedLocation;

                return View(showPupil);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// This method displays the field required to create a new pupil
        /// <example>// GET: Pupils/Create</example>
        /// </summary>
        /// <returns>Shows the fields required for the new pupil</returns>

        public ActionResult Create()
        {
            //Get all the Classes for dropdown list
            EditPupil editPupil = new EditPupil();
            string url = "PupilData/GetClasses";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ClasseDto> SelectedClasses = response.Content.ReadAsAsync<IEnumerable<ClasseDto>>().Result;
            editPupil.allClasses = SelectedClasses;

            //Get all the locations for dropdown list
            url = "PupilData/GetLocations";
            response = client.GetAsync(url).Result;
           
            IEnumerable<LocationDto> SelectedLocations = response.Content.ReadAsAsync<IEnumerable<LocationDto>>().Result;
            editPupil.allLocations = SelectedLocations;

            return View(editPupil);
        }

        /// <summary>
        /// This method creates a new pupil object
        /// </summary>
        /// <param name="Pupil">Pupil to be created</param>
        /// <returns>Creates and saves the new pupil to the database</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pupil Pupil)//newPupil
        {
           
            Debug.WriteLine(Pupil.firstName + " " +Pupil.lastName+" "+Pupil.classId+" "+Pupil.locId+" "+ Pupil.pId);
            
            //Add a new pupil to the database
            string url = "PupilData/AddPupil";
            HttpContent content = new StringContent(jss.Serialize(Pupil));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                //Redirect to the PupilList
                return RedirectToAction("PupilList");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// This method allows to show the information of the selected pupil to be edited
        /// <example>Pupils/Edit/4</example>
        /// <example>Pupils/Edit/2</example>
        /// </summary>
        /// <param name="id">ID of the selected pupil</param>
        /// <returns>Shows the selected pupil in the view</returns>
        [Authorize(Roles = "Admin, User")]
        public ActionResult Edit(int id)
        {
            EditPupil newPupil = new EditPupil();

            //Get the selected pupil from the database
            string url = "PupilData/FindPupil/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
           
            if (response.IsSuccessStatusCode)
            {
                PupilDto SelectedPupil = response.Content.ReadAsAsync<PupilDto>().Result;
                newPupil.pupil = SelectedPupil;
            }
            else
            {
                return RedirectToAction("Error");
            }

            //Get all Classes from the database for dropdown list
            url = "PupilData/GetClasses";
            response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ClasseDto> SelectedClasses = response.Content.ReadAsAsync<IEnumerable<ClasseDto>>().Result;
                newPupil.allClasses = SelectedClasses;
            }
            else
            {
                return RedirectToAction("Error");
            }

            //Get all locations from the database for dropdown list
            url = "PupilData/GetLocations";
            response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<LocationDto> SelectedLocations = response.Content.ReadAsAsync<IEnumerable<LocationDto>>().Result;
                newPupil.allLocations = SelectedLocations;
            }
            else
            {
                return RedirectToAction("Error");
            }
            return View(newPupil);

        }

        /// <summary>
        /// This method edits the selected pupil 
        /// <example>POST: Pupils/Edit/1</example>
        /// </summary>
        /// <param name="id">ID of the selected pupil</param>
        /// <param name="Pupil">Selected pupil itself</param>
        /// <returns>Updates and saves the current pupil to the database</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, User")]
        public ActionResult Edit(int id, Pupil Pupil)//currentPupil
        {

            Debug.WriteLine(Pupil.firstName + " " +Pupil.lastName+" "+Pupil.classId+" "+Pupil.locId+" "+ Pupil.pId + " " + id);
            
            //Update and save the current pupil
            string url = "PupilData/UpdatePupil/" + id;

            HttpContent content = new StringContent(jss.Serialize(Pupil));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Details", new { id = Pupil.pId });
            }
            else
            {
                return RedirectToAction("Error");
            }

        }


        /// <summary>
        /// This method shows the selected pupil
        /// <example>GET: Pupils/Delete/1</example>
        /// <example>GET: Pupils/Delete/3</example>
        /// </summary>
        /// <param name="id">ID of the selected pupil</param>
        /// <returns>Shows the current pupils</returns>
        // 
        [Authorize(Roles = "Admin, User")]
        public ActionResult Delete(int id)
        {
            //Get current pupil from the database
            string url = "PupilData/FindPupil/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                Pupil SelectedPupil = response.Content.ReadAsAsync<Pupil>().Result;
                return View(SelectedPupil);
            }
            else
            {
                return RedirectToAction("Error");
            }
            
        }

        /// <summary>
        /// This method removes the selected pupil from the database
        /// <example>POST: Pupils/Delete/2</example>
        ///  <example>POST: Pupils/Delete/5</example>
        /// </summary>
        /// <param name="id">Id of the selected pupil</param>
        /// <returns>Removes the selected pupil from the database</returns>
        // 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, User")]
        public ActionResult DeleteConfirmed(int id)
        {
            //Delete current pipul from database
            string url = "PupilData/DeletePupil/" + id;
            
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("PupilList");
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

