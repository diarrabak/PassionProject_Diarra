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
        /// This method return the classe list to view
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
        /// This method gives details about the selected class element. It shows all the pupils and modules linked to the class.
        /// <example>Classes/Details/5</example>
        /// <example>Classes/Details/2</example>
        /// </summary>
        /// <param name="id">The ID of the selected class</param>
        /// <returns>Class details with pupils and modules</returns>
        /// 
        public ActionResult Details(int id)
        {
            
            ShowClasse ViewModel = new ShowClasse();
            string url = "ClasseData/FindClasse/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            
            if (response.IsSuccessStatusCode)
            {
                //Put data into classe data transfer object
                ClasseDto SelectedClasse = response.Content.ReadAsAsync < ClasseDto>().Result;
                ViewModel.classe = SelectedClasse;
            }
            else
            {
                return RedirectToAction("Error");
            }

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

            url = "ClasseData/GetClassePupils/" + id;
            response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                //Put data into Team data transfer object
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
       /// This methos shows the fields of the class to be created
       /// </summary>
       /// <returns></returns>
        public ActionResult Create()
        {
            // ViewBag.classId = new SelectList(db.Classes, "classId", "className");
            return View();
        }

        /// <summary>
        /// This method saves the new classe to the database
        /// <example>POST: Classes/Create</example>
        /// </summary>
        /// <param name="newClasse"></param>
        /// <returns></returns>
        // 
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Classe newClasse)
        {
          
            string url = "ClasseData/AddClasse";
            HttpContent content = new StringContent(jss.Serialize(newClasse));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

              //  int modid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = newClasse.classId });
                //return RedirectToAction("ClasseList");
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
        /// <param name="id">Id of the selected classe</param>
        /// <returns>Display the selected classe element</returns>
        // 
        public ActionResult Edit(int id)
        {
            string url = "ClasseData/FindClasse/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                Classe SelectedPlayer = response.Content.ReadAsAsync<Classe>().Result;
                return View(SelectedPlayer);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// This method is used to edit and save the selected classe element
        /// <example>Classes/Edit/5</example>
        /// /// <example>Classes/Edit/1</example>
        /// </summary>
        /// <param name="id">Id of the classe to be edited</param>
        /// <param name="currentClasse"></param>
        /// <returns></returns>
        // POST: 
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Classe currentClasse)
        {

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
        /// This method shows the information about the classe element before deletion
        /// <example>GET: Classes/Delete/5</example>
        /// <example>GET: Classes/Delete/1</example>
        /// </summary>
        /// <param name="id">Id of the selected classe</param>
        /// <returns>Selected classe </returns>
        // 

        public ActionResult Delete(int id)
        {
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
        /// <param name="id"></param>
        /// <returns></returns>

        // 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string url = "ClasseData/DeleteClasse/" + id;
            //post body is empty
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
