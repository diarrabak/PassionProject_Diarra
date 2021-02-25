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
        //NB: This code is inspired from the Christine Bittle course, professor at Humber college.

        //private PassionDataContext db = new PassionDataContext();

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
            //change this to match your own local port number
            client.BaseAddress = new Uri("https://localhost:44327/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));


        }

        /// <summary>
        /// This method displays the list of all modules and the classes they belong
        /// <example>Modules/ModuleList</example>
        /// </summary>
        /// <returns></returns>
        // GET: Modules
        public ActionResult ModuleList()
        {
            //var modules = db.Modules.Include(m => m.Classe);
            //return View(modules.ToList());

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
        /// This method shows the information in the selected module which ID is given
        /// <example>GET: Modules/Details/5</example>
        /// <example>GET: Modules/Details/3</example>
        /// </summary>
        /// <param name="id">ID of selected module</param>
        /// <returns></returns>
        // 
        public ActionResult Details(int id)
        {
            ShowModule showModule = new ShowModule();
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Module module = db.Modules.Find(id);
            //if (module == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(module);


            string url = "ModuleData/FindModule/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                ModuleDto SelectedModule = response.Content.ReadAsAsync<ModuleDto>().Result;
                showModule.module=SelectedModule;
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
        /// <returns></returns>
        // GET: Modules/Create
        public ActionResult Create()
        {
            EditModule editModule = new EditModule();
            string url = "ModuleData/GetClasses";
            HttpResponseMessage response = client.GetAsync(url).Result;
            
            IEnumerable<ClasseDto> SelectedClasses = response.Content.ReadAsAsync<IEnumerable<ClasseDto>>().Result;
            editModule.allClasses = SelectedClasses;
            return View(editModule);
            
           
            //ViewBag.classId = new SelectList(db.Classes, "classId", "className");
            //return View();
        }

        /// <summary>
        /// This method permits to create and save a new module
        /// <example>POST: Modules/Create</example>
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        // 
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Module module)//newModule
        {
            //if (ModelState.IsValid)
            //{
            //    db.Modules.Add(module);
            //    db.SaveChanges();
            //    return RedirectToAction("ModuleList");
            //}

            //ViewBag.classId = new SelectList(db.Classes, "classId", "className", module.classId);
            //return View(module);

            string url = "ModuleData/AddModule";
            HttpContent content = new StringContent(jss.Serialize(module));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                //int modid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("ModuleList");
                //return RedirectToAction("Details", new { id = module.modId });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Modules/Edit/5
        public ActionResult Edit(int id)
        {
            EditModule editModule = new EditModule();
            string url = "ModuleData/FindModule/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                ModuleDto selectedModule = response.Content.ReadAsAsync<ModuleDto>().Result;
                editModule.module= selectedModule;
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


            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Module module = db.Modules.Find(id);
            //if (module == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.classId = new SelectList(db.Classes, "classId", "className", module.classId);
            //return View(module);
        }


        /// <summary>
        /// This method edits and saves the selected module
        /// <example>POST: Modules/Edit/1</example>
        /// <example>POST: Modules/Edit/4</example>
        /// </summary>
        /// <param name="id"> The ID of the selected module</param>
        /// <param name="module"></param>
        /// <returns></returns>
        // 
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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


            //if (ModelState.IsValid)
            //{
            //    db.Entry(module).State = EntityState.Modified;
            //    db.SaveChanges();
            //    //return RedirectToAction("ModuleList");
            //    return RedirectToAction("Details", new { id = module.modId });
            //}
            //ViewBag.classId = new SelectList(db.Classes, "classId", "className", module.classId);
            //return View(module);
        }

            // GET: Modules/Delete/5

            public ActionResult Delete(int id)
        {
            string url = "ModuleData/FindModule/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
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
        /// <returns></returns>
        // 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string url = "ModuleData/DeleteModule/" + id;
            //post body is empty
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
