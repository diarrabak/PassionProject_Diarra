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
    public class ClassesController : Controller
    {
        //NB: This code is inspired from the Christine Bittle course, professor at Humber college.

        // private PassionDataContext db = new PassionDataContext();

        /*All the controllers can be automatically generated from:
        Controllers (folder)->Add->Controller-> MVC5 Controller with views, using Entity Framework->
        Add->Select the right Model and database, then give a name to the controller. Keep the 3 fields of Views ticked. 
        */
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static ClassesController()
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
        /// This method returns the Classe list to view
        /// <example>Classes/ClasseList</example>
        /// </summary>
        /// <returns>Classe list</returns>

        public ActionResult ClasseList()
        {
            string url = "ClasseData/GetClasses";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<Classe> SelectedClasses = response.Content.ReadAsAsync<IEnumerable<Classe>>().Result;
                return View(SelectedClasses);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// This method gives details about the selected Classe object. It shows all the pupils and modules linked to that Classe.
        /// <example>Classes/Details/5</example>
        /// <example>Classes/Details/2</example>
        /// </summary>
        /// <param name="id">The ID of the selected Classed object</param>
        /// <returns>Classe object details with pupils and modules</returns>
        /// 
        public ActionResult Details(int id)
        {
            //Model used to combine a Classe object and its pupils and modules
            ShowClasse ViewModel = new ShowClasse();

            //Get the current Classe object
            string url = "ClasseData/FindClasse/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            
            if (response.IsSuccessStatusCode)
            {
                ClasseDto SelectedClasse = response.Content.ReadAsAsync < ClasseDto>().Result;
                ViewModel.classe = SelectedClasse;
            }
            else
            {
                return RedirectToAction("Error");
            }

            //Get the mudules which are linked to the current Classe object
            url = "ClasseData/GetClasseModules/" + id;
            response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                //Put data into Team data transfer object
                IEnumerable<ModuleDto> SelectedModules = response.Content.ReadAsAsync<IEnumerable<ModuleDto>>().Result;
                ViewModel.allModules = SelectedModules;
            }
            else
            {
                return RedirectToAction("Error");
            }

            //Get all the pupils which are following the current Classe
            url = "ClasseData/GetClassePupils/" + id;
            response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<PupilDto> Selectedpupils = response.Content.ReadAsAsync<IEnumerable<PupilDto>>().Result;
                ViewModel.allPupils = Selectedpupils;
            }
            else
            {
                return RedirectToAction("Error");
            }

            return View(ViewModel);
        }

        /// <summary>
        /// This method shows the fields of the Classe object to be created
        /// </summary>
        /// <returns>The current Classe fields to the view</returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// This method saves the new classe to the database
        /// <example>POST: Classes/Create</example>
        /// </summary>
        /// <param name="newClasse">The current Classe object to be saved</param>
        /// <returns></returns>
        // 
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Classe newClasse)
        {
          //Saving the new Classe object to the database
            string url = "ClasseData/AddClasse";
            HttpContent content = new StringContent(jss.Serialize(newClasse));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                //Go back to classes list
                return RedirectToAction("ClasseList");
                
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// This method shows the fields of the classe element to edit which ID is provided
        /// <example>GET: Classes/Edit/5 </example>
        /// </summary>
        /// <param name="id">Id of the selected Classe object</param>
        /// <returns>Displays the selected Classe object</returns>
        // 
        [Authorize(Roles = "User")]
        public ActionResult Edit(int id)
        {
            string url = "ClasseData/FindClasse/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                Classe SelectedPlayer = response.Content.ReadAsAsync<Classe>().Result;
                return View(SelectedPlayer);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// This method is used to edit and save the selected Classe object
        /// <example>Classes/Edit/5</example>
        /// /// <example>Classes/Edit/1</example>
        /// </summary>
        /// <param name="id">ID of the Classe object to be edited</param>
        /// <param name="currentClasse"></param>
        /// <returns>Modifies and Saves the selected Classe to the database</returns>
        // POST: 
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public ActionResult Edit(int id, Classe currentClasse)
        {
            //Update and save the Classe which ID is given
            string url = "ClasseData/UpdateClasse/" + id;

            HttpContent content = new StringContent(jss.Serialize(currentClasse));
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
        /// This method shows the information about the Classe object before deletion
        /// <example>GET: Classes/Delete/5</example>
        /// <example>GET: Classes/Delete/1</example>
        /// </summary>
        /// <param name="id">ID of the selected Classe</param>
        /// <returns>Show the selected Classe </returns>
        // 
        [Authorize(Roles = "User")]
        public ActionResult Delete(int id)
        {
            //Getting the Classe which ID is given
            string url = "ClasseData/FindClasse/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            
            if (response.IsSuccessStatusCode)
            {
                Classe SelectedClasse = response.Content.ReadAsAsync<Classe>().Result;
                return View(SelectedClasse);
            }
            else
            {
                return RedirectToAction("Error");
            }
           
        }

        /// <summary>
        /// This method removes the selected classe from the database
        /// <example>POST: Classes/Delete/5</example>
        /// <example>POST: Classes/Delete/2</example>
        /// </summary>
        /// <param name="id">ID of the selected Classe object</param>
        /// <returns>Remove the Classe from the database</returns>
                // 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public ActionResult DeleteConfirmed(int id)
        {
            string url = "ClasseData/DeleteClasse/" + id;
            
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
           
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("ClasseList");
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
