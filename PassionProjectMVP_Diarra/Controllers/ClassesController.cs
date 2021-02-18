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


            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

        }


        // GET: Classes
        public ActionResult ClasseList()
        {
            //var Classes = db.Classes.Include(m => m.Classe);
            //return View(Classes.ToList());

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

        // GET: Classes/Details/5
        public ActionResult Details(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Classe Classe = db.Classes.Find(id);
            //if (Classe == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(Classe);
            ShowClasse ViewModel = new ShowClasse();
            string url = "ClasseData/FindClasse/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
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
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
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
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
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

        // GET: Classes/Create
        public ActionResult Create()
        {
            // ViewBag.classId = new SelectList(db.Classes, "classId", "className");
            return View();
        }

        // POST: Classes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Classe newClasse)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Classes.Add(Classe);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //ViewBag.classId = new SelectList(db.Classes, "classId", "className", Classe.classId);
            //return View(Classe);

            string url = "ClasseData/AddClasse";
            HttpContent content = new StringContent(jss.Serialize(newClasse));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

              //  int modid = response.Content.ReadAsAsync<int>().Result;
                //return RedirectToAction("Details", new { id = modid });
                return RedirectToAction("ClasseList");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Classes/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "ClasseData/FindClasse/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
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
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Classe Classe = db.Classes.Find(id);
            //if (Classe == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.classId = new SelectList(db.Classes, "classId", "className", Classe.classId);
            //return View(Classe);
        }

        // POST: Classes/Edit/5
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
            //    if (ModelState.IsValid)
            //    {
            //        db.Entry(Classe).State = EntityState.Modified;
            //        db.SaveChanges();
            //        return RedirectToAction("Index");
            //    }
            //    ViewBag.classId = new SelectList(db.Classes, "classId", "className", Classe.classId);
            //    return View(Classe);
        }

        // GET: Classes/Delete/5

        public ActionResult Delete(int id)
        {
            string url = "ClasseData/FindClasse/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                Classe SelectedClasse = response.Content.ReadAsAsync<Classe>().Result;
                return View(SelectedClasse);
            }
            else
            {
                return RedirectToAction("Error");
            }
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Classe Classe = db.Classes.Find(id);
            //if (Classe == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(Classe);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string url = "ClasseData/DeleteClasse/" + id;
            //post body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("ClasseList");
            }
            else
            {
                return RedirectToAction("Error");
            }
            //Classe Classe = db.Classes.Find(id);
            //db.Classes.Remove(Classe);
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
