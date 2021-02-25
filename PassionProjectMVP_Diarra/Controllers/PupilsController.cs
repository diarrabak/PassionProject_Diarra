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
using PassionProjectMVP_Diarra.Models;
using PassionProjectMVP_Diarra.Models.ModelViews;

namespace PassionProjectMVP_Diarra.Controllers
{
    public class PupilsController : Controller
    {
        private PassionDataContext db = new PassionDataContext();

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


            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

        }


        // GET: Pupils
        public ActionResult PupilList()
        {
            //var Pupils = db.Pupils.Include(m => m.Classe);
            //return View(Pupils.ToList());

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

        // GET: Pupils/Details/5
        public ActionResult Details(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Pupil Pupil = db.Pupils.Find(id);
            //if (Pupil == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(Pupil);
            ShowPupil showPupil = new ShowPupil();
            string url = "Pupildata/FindPupil/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                PupilDto SelectedPupil = response.Content.ReadAsAsync<PupilDto>().Result;
                showPupil.pupil=SelectedPupil;

                url = "PupilData/GetPupilClasse/" + id;
                response = client.GetAsync(url).Result;
                ClasseDto SelectedClasse = response.Content.ReadAsAsync<ClasseDto>().Result;
                showPupil.classe = SelectedClasse;

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

        // GET: Pupils/Create
        public ActionResult Create()
        {
            // ViewBag.classId = new SelectList(db.Pupils, "classId", "className");
            //return View();
            EditPupil editPupil = new EditPupil();
            string url = "PupilData/GetClasses";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ClasseDto> SelectedClasses = response.Content.ReadAsAsync<IEnumerable<ClasseDto>>().Result;
            editPupil.allClasses = SelectedClasses;

            url = "PupilData/GetLocations";
            response = client.GetAsync(url).Result;
           
            IEnumerable<LocationDto> SelectedLocations = response.Content.ReadAsAsync<IEnumerable<LocationDto>>().Result;
            editPupil.allLocations = SelectedLocations;

            return View(editPupil);
        }

        // POST: Pupils/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pupil Pupil)//newPupil
        {
            //if (ModelState.IsValid)
            //{
            //    db.Pupils.Add(Pupil);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //ViewBag.classId = new SelectList(db.Pupils, "classId", "className", Pupil.classId);
            //return View(Pupil);

            string url = "PupilData/AddPupil";
            HttpContent content = new StringContent(jss.Serialize(Pupil));
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

        // GET: Pupils/Edit/5
        public ActionResult Edit(int id)
        {
            EditPupil newPupil = new EditPupil();
            string url = "PupilData/FindPupil/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                PupilDto SelectedPupil = response.Content.ReadAsAsync<PupilDto>().Result;
                newPupil.pupil = SelectedPupil;
            }
            else
            {
                return RedirectToAction("Error");
            }

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


            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Pupil Pupil = db.Pupils.Find(id);
            //if (Pupil == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.classId = new SelectList(db.Pupils, "classId", "className", Pupil.classId);
            //return View(Pupil);
        }

        // POST: Pupils/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Pupil Pupil)//currentPupil
        {

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



            //if (ModelState.IsValid)
            //{
            //    db.Entry(Pupil).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.classId = new SelectList(db.Pupils, "classId", "className", Pupil.classId);
            //return View(Pupil);
        }

        // GET: Pupils/Delete/5

        public ActionResult Delete(int id)
        {
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
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Pupil Pupil = db.Pupils.Find(id);
            //if (Pupil == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(Pupil);
        }

        // POST: Pupils/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string url = "PupilData/DeletePupil/" + id;
            //post body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("PupilList");
            }
            else
            {
                return RedirectToAction("Error");
            }
            //Pupil Pupil = db.Pupils.Find(id);
            //db.Pupils.Remove(Pupil);
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

