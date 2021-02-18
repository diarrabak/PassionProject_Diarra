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
        // private PassionDataContext db = new PassionDataContext();

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


            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

        }


        // GET: Modules
        public ActionResult Index()
        {
            //var modules = db.Modules.Include(m => m.Classe);
            //return View(modules.ToList());

            string url = "ModuleData/GetModules";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<Module> SelectedModules = response.Content.ReadAsAsync<IEnumerable<Module>>().Result;
                return View(SelectedModules);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Modules/Details/5
        public ActionResult Details(int id)
        {
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
            string url = "Moduledata/FindModule/" + id;
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

        // GET: Modules/Create
        public ActionResult Create()
        {
           // ViewBag.classId = new SelectList(db.Classes, "classId", "className");
            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Module newModule)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Modules.Add(module);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //ViewBag.classId = new SelectList(db.Classes, "classId", "className", module.classId);
            //return View(module);

            string url = "ModuleData/AddModule";
            HttpContent content = new StringContent(jss.Serialize(newModule));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int modid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = modid });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Modules/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "ModuleData/FindModule/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                Module SelectedPlayer = response.Content.ReadAsAsync<Module>().Result;
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
            //Module module = db.Modules.Find(id);
            //if (module == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.classId = new SelectList(db.Classes, "classId", "className", module.classId);
            //return View(module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Module currentModule)
        {
            
            string url = "ModuleData/UpdateModule/" + id;
            
            HttpContent content = new StringContent(jss.Serialize(currentModule));
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
            //        db.Entry(module).State = EntityState.Modified;
            //        db.SaveChanges();
            //        return RedirectToAction("Index");
            //    }
            //    ViewBag.classId = new SelectList(db.Classes, "classId", "className", module.classId);
            //    return View(module);
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
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string url = "ModuleData/DeleteModule/" + id;
            //post body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
            //Module module = db.Modules.Find(id);
            //db.Modules.Remove(module);
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
