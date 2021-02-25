﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PassionProjectMVP_Diarra.Models;
using PassionProjectMVP_Diarra.Models.ModelViews;

namespace PassionProjectMVP_Diarra.Controllers
{
    public class PupilDataController : ApiController
    {
        //This variable is our database access point
        private PassionDataContext db = new PassionDataContext();

        //This code is mostly scaffolded from the base models and database context
        //New > WebAPIController with Entity Framework Read/Write Actions
        //Choose model "Pupil"
        //Choose context "Passion Data Context"
        //Note: The base scaffolded code needs many improvements for a fully
        //functioning MVP.

        /// <summary>
        /// This method process the list of the pupils and return in form of list
        /// <example> GET: api/PupilData/GetPupils </example>
        /// </summary>
        /// <returns> The list of pupils with the first name, last name, classe and location from the database</returns>

        [ResponseType(typeof(IEnumerable<ShowPupil>))]
        //[ResponseType(typeof(IEnumerable<PupilDto>))]
        public IHttpActionResult GetPupils()
        {
            
            //return (IHttpActionResult)db.Pupils.ToList();
            List<Pupil> Pupils = db.Pupils.ToList();

            List<ShowPupil> PupilDtos = new List<ShowPupil> { };
            //List<PupilDto> PupilDtos = new List<PupilDto> { };

            //Here you can select the information to be transfered to the  API
            foreach (var Pupil in Pupils)
            {
                ShowPupil pupil = new ShowPupil();

                Classe classe = db.Classes.Where(c => c.Pupils.Any(m => m.pId == Pupil.pId)).FirstOrDefault();

                ClasseDto parentClass = new ClasseDto
                {
                    classId = classe.classId,
                    className = classe.className,
                    startDate = classe.startDate,
                    endDate = classe.endDate
                };

                Location location = db.Locations.Where(l => l.Pupils.Any(m => m.pId == Pupil.pId)).FirstOrDefault();

                LocationDto locationPlace = new LocationDto
                {
                    locId = location.locId,
                    city = location.city,
                    country = location.country,
                    incomeRange=location.incomeRange
                };

               

                PupilDto NewPupil = new PupilDto
                {
                    pId = Pupil.pId,
                    firstName = Pupil.firstName,
                    lastName = Pupil.lastName,
                    age=Pupil.age,
                    classId = Pupil.classId,
                    locId = Pupil.locId
                };

                pupil.pupil = NewPupil;
                pupil.classe = parentClass;
                pupil.location = locationPlace;
                PupilDtos.Add(pupil);
            }

            return Ok(PupilDtos);
        }

       /// <summary>
       /// This method gives the full list of classes
       /// </summary>
       /// <returns></returns>
        [ResponseType(typeof(IEnumerable<ClasseDto>))]
        public IHttpActionResult GetClasses()
        {

            //return (IHttpActionResult)db.Classes.ToList();
            List<Classe> Classes = db.Classes.ToList();
            List<ClasseDto> ClasseDtos = new List<ClasseDto> { };

            //Here you can select the information to be transfered to the  API
            foreach (var Classe in Classes)
            {
                ClasseDto NewClasse = new ClasseDto
                {
                    classId = Classe.classId,
                    className = Classe.className
                };
                ClasseDtos.Add(NewClasse);
            }

            return Ok(ClasseDtos);
        }

        /// <summary>
        /// This methods gives all the locations
        /// </summary>
        /// <returns>Location list</returns>
        [ResponseType(typeof(IEnumerable<LocationDto>))]
        public IHttpActionResult GetLocations()
        {

            //return (IHttpActionResult)db.Locations.ToList();
            List<Location> Locations = db.Locations.ToList();
            List<LocationDto> LocationDtos = new List<LocationDto> { };

            //Here you can select the information to be transfered to the  API
            foreach (var Location in Locations)
            {
                LocationDto NewLocation = new LocationDto
                {
                    locId = Location.locId,
                    city = Location.city,
                    country = Location.country,
                    incomeRange = Location.incomeRange
                };
                LocationDtos.Add(NewLocation);
            }

            return Ok(LocationDtos);
        }

        /// <summary>
        /// This method permits to get a pupil which ID is provided
        /// <example>GET api/PupilData/FindPupil/2</example>
        /// <example>GET api/PupilData/FindPupil/5</example>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        [ResponseType(typeof(PupilDto))]
        public IHttpActionResult FindPupil(int id)
        {
            Pupil pupil = db.Pupils.Find(id);
           
            if (pupil == null)
            {
                return NotFound();
            }
            PupilDto pupilTemp = new PupilDto
            {
                pId = pupil.pId,
                    firstName = pupil.firstName,
                    lastName = pupil.lastName,
                    age=pupil.age,
                    classId = pupil.classId,
                    locId = pupil.locId
            };

            return Ok(pupilTemp);
        }

        /// <summary>
        /// This method provides the classe to which the current module belongs
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [ResponseType(typeof(ClasseDto))]
        public IHttpActionResult GetPupilClasse(int id)
        {

            //Find the classe to which the current pupil belongs
            Classe classe = db.Classes.Where(c => c.Pupils.Any(p =>p.pId == id)).FirstOrDefault();

            //In case this classe does not exist
            if (classe == null)
            {

                return NotFound();
            }

            ClasseDto parentClass = new ClasseDto
            {
                classId = classe.classId,
                className = classe.className,
                startDate = classe.startDate,
                endDate = classe.endDate
            };

            return Ok(parentClass);
        }

        /// <summary>
        /// This method gives the location of the current pupil
        /// </summary>
        /// <param name="id">Id of the pupil</param>
        /// <returns>Pupil's location</returns>
        [ResponseType(typeof(ClasseDto))]
        public IHttpActionResult GetPupilLocation(int id)
        {

            //Find the classe to which the current pupil belongs
            Location location = db.Locations.Where(l => l.Pupils.Any(p => p.pId == id)).FirstOrDefault();

            //In case this classe does not exist
            if (location == null)
            {

                return NotFound();
            }

            LocationDto parentLocation = new LocationDto
            {
                locId = location.locId,
                city = location.city,
                country = location.country,
                incomeRange = location.incomeRange
            };

            return Ok(parentLocation);
        }


        /// <summary>
        /// This method permits to update the selected pupil
        /// <example>api/PupilData/UpdatePupil/1</example>
        /// <example>api/PupilData/UpdatePupil/2</example>
        /// </summary>
        /// <param name="id">The ID of the pupil</param>
        /// <param name="pupil"></param>
        /// <returns></returns>
        // PUT: 
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdatePupil(int id, Pupil pupil)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pupil.pId)
            {
                return BadRequest();
            }

            db.Entry(pupil).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PupilExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// This method permits to add a new pupil to the database
        /// <example>POST: api/PupilData/AddPupil</example>
        /// </summary>
        /// <param name="pupil"></param>
        /// <returns> It adds a new pupil to the database</returns>
        
        [HttpPost]
        [ResponseType(typeof(Pupil))]
        public IHttpActionResult AddPupil(Pupil pupil)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pupils.Add(pupil);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = pupil.pId }, pupil);
        }

        /// <summary>
        /// This method deletes the pupil which ID is given
        /// <example>api/PupilData/DeletePupil/1</example>
        /// <example>api/PupilData/DeletePupil/3</example>
        /// </summary>
        /// <param name="id">ID of the pupil</param>
        /// <returns></returns>

        [HttpPost]
        [ResponseType(typeof(Pupil))]
        public IHttpActionResult DeletePupil(int id)
        {
            Pupil pupil = db.Pupils.Find(id);
            if (pupil == null)
            {
                return NotFound();
            }

            db.Pupils.Remove(pupil);
            db.SaveChanges();

            return Ok(pupil);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PupilExists(int id)
        {
            return db.Pupils.Count(e => e.pId == id) > 0;
        }
    }
}