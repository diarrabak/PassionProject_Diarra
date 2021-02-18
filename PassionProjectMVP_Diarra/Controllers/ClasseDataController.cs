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

namespace PassionProjectMVP_Diarra.Controllers
{
    public class ClasseDataController : ApiController
    {
        //This variable is our database access point
        private PassionDataContext db = new PassionDataContext();

        //This code is mostly scaffolded from the base models and database context
        //New > WebAPIController with Entity Framework Read/Write Actions
        //Choose model "Player"
        //Choose context "Passion Data Context"
        //Note: The base scaffolded code needs many improvements for a fully
        //functioning MVP.

        /// <summary>
        /// This method process the list of the Classes and return in form of list
        /// <example> GET: api/ClasseData/GetClasses </example>
        /// </summary>
        /// <returns> The list of Classes with the first name, last name, classe and location from the database</returns>

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
                    className = Classe.className,
                    startDate=Classe.startDate,
                    endDate=Classe.endDate
                };
                ClasseDtos.Add(NewClasse);
            }

            return Ok(ClasseDtos);
        }

        /// <summary>
        /// <example>GET: api/ClasseData/GetClasseModules/1</example>
        /// <example>GET: api/ClasseData/GetClasseModules/2</example>
        /// </summary>
        /// <param name="id"></param>
        /// <returns> The list of modules in the class which ID is given</returns>

        [ResponseType(typeof(IEnumerable<ModuleDto>))]
        public IHttpActionResult GetClasseModules(int id)
        {
            List<Module> Modules = db.Modules.Where(p => p.classId == id).ToList();
            List<ModuleDto> ModuleDtos = new List<ModuleDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Module in Modules)
            {
                ModuleDto NewModule = new ModuleDto
                {
                    modId = Module.modId,
                    moduleName = Module.moduleName,
                    description = Module.description,
                    delivery = Module.delivery,
                    fees = Module.fees,
                    classId= Module.classId
                };
                ModuleDtos.Add(NewModule);
            }

            return Ok(ModuleDtos);
        }


        /// <summary>
        /// <example>GET: api/ClasseData/GetClassePupils/1</example>
        /// <example>GET: api/ClasseData/GetClassePupils/1</example>
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The list of pupils in the class which ID is given</returns>

        [ResponseType(typeof(IEnumerable<PupilDto>))]
        public IHttpActionResult GetClassePupils(int id)
        {
            List<Pupil> Pupils = db.Pupils.Where(p => p.classId == id).ToList();
            List<PupilDto> PupilDtos = new List<PupilDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Pupil in Pupils)
            {
                PupilDto NewPupil = new PupilDto
                {
                    pId = Pupil.pId,
                    firstName = Pupil.firstName,
                    lastName = Pupil.lastName,
                    age = Pupil.age,
                    classId = Pupil.classId,
                    locId = Pupil.locId
                };
                PupilDtos.Add(NewPupil);
            }

            return Ok(PupilDtos);
        }




        /// <summary>
        /// <example>GET: api/ClasseData/FindClasse/1</example>
        /// <example>GET: api/ClasseData/FindClasse/2</example>
        /// </summary>
        /// <param name="id"> The parameter being the ID of the Classe</param>
        /// <returns> This method returns the Classe which id is given</returns>
        // 
        [HttpGet]
        [ResponseType(typeof(ClasseDto))]
        public IHttpActionResult FindClasse(int id)
        {
            Classe Classe = db.Classes.Find(id);

            if (Classe == null)
            {
                return NotFound();
            }
            ClasseDto ClasseTemp = new ClasseDto
            {
                classId = Classe.classId,
                className = Classe.className,
                startDate = Classe.startDate,
                endDate = Classe.endDate
            };

            return Ok(ClasseTemp);
        }

        /// <summary>
        /// This method permits to update the selected Classe
        /// <example>api/ClasseData/UpdateClasse/1</example>
        /// <example>api/ClasseData/UpdateClasse/2</example>
        /// </summary>
        /// <param name="id">The ID of the Classe</param>
        /// <param name="Classe"></param>
        /// <returns></returns>
        // PUT: 
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateClasse(int id, Classe Classe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Classe.classId)
            {
                return BadRequest();
            }

            db.Entry(Classe).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClasseExists(id))
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
        /// This method permits to add a new Classe to the database
        /// <example>POST: api/ClasseData/AddClasse</example>
        /// </summary>
        /// <param name="Classe"></param>
        /// <returns> It adds a new Classe to the database</returns>

        [HttpPost]
        [ResponseType(typeof(Classe))]
        public IHttpActionResult AddClasse(Classe Classe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Classes.Add(Classe);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Classe.classId }, Classe);
        }

        /// <summary>
        /// This method deletes the Classe which ID is given
        /// <example>api/ClasseData/DeleteClasse/1</example>
        /// <example>api/ClasseData/DeleteClasse/3</example>
        /// </summary>
        /// <param name="id">ID of the Classe</param>
        /// <returns></returns>

        [HttpPost]
        [ResponseType(typeof(Classe))]
        public IHttpActionResult DeleteClasse(int id)
        {
            Classe Classe = db.Classes.Find(id);
            if (Classe == null)
            {
                return NotFound();
            }

            db.Classes.Remove(Classe);
            db.SaveChanges();

            return Ok(Classe);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClasseExists(int id)
        {
            return db.Classes.Count(e => e.classId == id) > 0;
        }
    }
}