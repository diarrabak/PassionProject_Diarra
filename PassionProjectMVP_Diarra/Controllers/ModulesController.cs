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
    public class ModulesController : Controller
    {
        /*All the controllers can be automatically generated from:
        Controllers (folder)->Add->Controller-> MVC5 Controller with views, using Entity Framework->
        Add->Select the right Model and database, then give a name to the controller. Keep the 3 fields of Views ticked. 
        */
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static ModulesController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            //Port number to be customized
            client.BaseAddress = new Uri("https://localhost:44327/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));


        }

        /// <summary>
        /// This method displays the list of all modules and the classes they belong
        /// <example>Modules/ModuleList</example>
        /// </summary>
        /// <returns>List of modules and their classes</returns>
        // GET: Modules
        public ActionResult ModuleList()
        {

            string url = "ModuleData/GetModules";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ShowModule> SelectedModules = response.Content.ReadAsAsync<IEnumerable<ShowModule>>().Result;
                return View(SelectedModules);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// This method shows the information of the selected module which ID is given
        /// <example>GET: Modules/Details/5</example>
        /// <example>GET: Modules/Details/3</example>
        /// </summary>
        /// <param name="id">ID of selected module</param>
        /// <returns>Shows details of the selected module</returns>
        // 
        public ActionResult Details(int id)
        {
            ShowModule showModule = new ShowModule();
          
            //Get the current module
            string url = "ModuleData/FindModule/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            
            if (response.IsSuccessStatusCode)
            {
                ModuleDto SelectedModule = response.Content.ReadAsAsync<ModuleDto>().Result;
                showModule.module=SelectedModule;

                //Get the classe to which the current module belongs to
                url = "ModuleData/GetModuleClasse/" + id;
                response = client.GetAsync(url).Result;
                ClasseDto SelectedClasse = response.Content.ReadAsAsync<ClasseDto>().Result;
                showModule.classe = SelectedClasse;
                return View(showModule);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        /// <summary>
        /// This method displays information of the module to create
        /// </summary>
        /// <returns>Shows the information required for the module to be edited</returns>
       
        public ActionResult Create()
        {
            EditModule editModule = new EditModule();

            //Get all the classes for the dropdown list
            string url = "ModuleData/GetClasses";
            HttpResponseMessage response = client.GetAsync(url).Result;
            
            IEnumerable<ClasseDto> SelectedClasses = response.Content.ReadAsAsync<IEnumerable<ClasseDto>>().Result;
            editModule.allClasses = SelectedClasses;
            return View(editModule);
            
        }

        /// <summary>
        /// This method permits to create and save a new module
        /// <example>POST: Modules/Create</example>
        /// </summary>
        /// <param name="module">Module to be added to the database</param>
        /// <returns>Creates and saves the new module to the database</returns>
              
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Module module)//newModule
        {
            //Adding the new module to the database
            string url = "ModuleData/AddModule";
            HttpContent content = new StringContent(jss.Serialize(module));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ModuleList");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Thhis method permits to display the information of the module to be updated
        /// <example>Modules/Edit/1</example>
        /// <example>Modules/Edit/5</example>
        /// </summary>
        /// <param name="id">ID of the selected module</param>
        /// <returns>displays module for editing</returns>

        public ActionResult Edit(int id)
        {
            EditModule editModule = new EditModule();

            //Getting the module from the database
            string url = "ModuleData/FindModule/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                ModuleDto selectedModule = response.Content.ReadAsAsync<ModuleDto>().Result;
                editModule.module= selectedModule;

                //Getting from the database the Classe object to which the current module belongs to
                url = "ModuleData/GetClasses";
                response = client.GetAsync(url).Result;
                IEnumerable<ClasseDto> allClasses = response.Content.ReadAsAsync<IEnumerable<ClasseDto>>().Result;
                editModule.allClasses = allClasses;
                return View(editModule);
            }
            else
            {
                return RedirectToAction("Error");
            }

        }


        /// <summary>
        /// This method edits and saves the selected module
        /// <example>POST: Modules/Edit/1</example>
        /// <example>POST: Modules/Edit/4</example>
        /// </summary>
        /// <param name="id"> The ID of the selected module</param>
        /// <param name="module">The selected module to be edited</param>
        /// <returns>Updates and saves to the database the selected module</returns>
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Module module) //currentModule
        {

            string url = "ModuleData/UpdateModule/" + id;

            HttpContent content = new StringContent(jss.Serialize(module));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                // return RedirectToAction("Details", new { id = module.modId });
                return RedirectToAction("ModuleList");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }


        /// <summary>
        /// This method shows the selected module 
        /// <example>Modules/Delete/1</example>
        /// <example>Modules/Delete/2</example>
        /// </summary>
        /// <param name="id">ID of the selected module</param>
        /// <returns>Shows the selected module to the view</returns>
        public ActionResult Delete(int id)
        {
            //Get the selected module from the database
            string url = "ModuleData/FindModule/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
          
            if (response.IsSuccessStatusCode)
            {
                Module SelectedModule = response.Content.ReadAsAsync<Module>().Result;
                return View(SelectedModule);
            }
            else
            {
                return RedirectToAction("Error");
            }
          
        }

        /// <summary>
        /// This method deletes the selected module from the database
        /// <example>POST: Modules/Delete/5</example>
        /// <example>POST: Modules/Delete/2</example>
        /// </summary>
        /// <param name="id">Id of the selected module</param>
        /// <returns>Removes the selected module from the database</returns>
        // 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Remove the current module from the database
            string url = "ModuleData/DeleteModule/" + id;
            
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
           
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("ModuleList");
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
