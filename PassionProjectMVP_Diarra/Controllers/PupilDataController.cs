using System;
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
        //Connection to the databse
        private PassionDataContext db = new PassionDataContext();

       
        /// <summary>
        /// This method gets the list of all pupils from the database
        /// <example> GET: api/PupilData/GetPupils </example>
        /// </summary>
        /// <returns> The list of pupils, their location  and Classe they belong to from the database</returns>

        [ResponseType(typeof(IEnumerable<ShowPupil>))]
        public IHttpActionResult GetPupils()
        {
            
            //List of the pupils from the database
            List<Pupil> Pupils = db.Pupils.ToList();

            //Data transfer object to show information about the pupil
            List<ShowPupil> PupilDtos = new List<ShowPupil> { };

            foreach (var Pupil in Pupils)
            {
                ShowPupil pupil = new ShowPupil();

                //Get the Classe to which the Pupil belongs to
                Classe classe = db.Classes.Where(c => c.Pupils.Any(m => m.pId == Pupil.pId)).FirstOrDefault();

                ClasseDto parentClass = new ClasseDto
                {
                    classId = classe.classId,
                    className = classe.className,
                    startDate = classe.startDate,
                    endDate = classe.endDate
                };
                //Get the location of pupil
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
        /// This method gives the full list of all Classe object
        /// <example>api/PupilData/GetClasses</example>
        /// </summary>
        /// <returns> the list of all Classe objects from the database</returns>
        [ResponseType(typeof(IEnumerable<ClasseDto>))]
        public IHttpActionResult GetClasses()
        {

            //Get all the Classe objects from the databse
            List<Classe> Classes = db.Classes.ToList();

            //Data transfer model use to display selected information about Classe object
            List<ClasseDto> ClasseDtos = new List<ClasseDto> { };

          
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
        /// <example>aoi/PupilData/GetLocations</example>
        /// </summary>
        /// <returns>All the Location list from database</returns>
        [ResponseType(typeof(IEnumerable<LocationDto>))]
        public IHttpActionResult GetLocations()
        {
            //Get all location objects from the database
            List<Location> Locations = db.Locations.ToList();
            List<LocationDto> LocationDtos = new List<LocationDto> { };

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
        /// <param name="id">ID of the selected pupil</param>
        /// <returns>The selected pupil which ID is given</returns>

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
        /// This method provides the Classe to which the current module belongs
        /// <example>api/PupilData/GetPupilClasse/1</example>
        /// <example>api/PupilData/GetPupilClasse/3</example>
        /// </summary>
        /// <param name="id">ID of the selected pupil</param>
        /// <returns>The Classe to which current pupil belongs</returns>

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
        /// <example>api/PupilData/GetPupilLocation/1</example>
        /// <example>api/PupilData/GetPupilLocation/4</example>
        /// </summary>
        /// <param name="id">Id of the selected pupil</param>
        /// <returns>Current pupil location</returns>
        [ResponseType(typeof(LocationDto))]
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
        /// <param name="id">The ID of the current pupil</param>
        /// <param name="pupil">current pupil to be updated</param>
        /// <returns>Updates and saves current pupil to the database</returns>
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
        /// <param name="pupil">new pupil to be added to the database</param>
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
        /// <returns>Removes the selected pupil from the database</returns>

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