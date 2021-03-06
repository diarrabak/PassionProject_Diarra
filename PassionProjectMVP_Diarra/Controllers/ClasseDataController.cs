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

namespace PassionProjectMVP_Diarra.Controllers
{
    public class ClasseDataController : ApiController
    {
        //This variable is the access to my database which contains classes, modules,locations and pupils
        private PassionDataContext db = new PassionDataContext();


        /// <summary>
        /// This method gets from the database the list of the all Classes
        /// <example> GET: api/ClasseData/GetClasses </example>
        /// </summary>
        /// <returns> The list of Classes from the database</returns>

        [ResponseType(typeof(IEnumerable<ClasseDto>))]
        public IHttpActionResult GetClasses()
        {

            //Getting the list of Classe objects from the databse
            List<Classe> Classes = db.Classes.ToList();

            //Here a data transfer model is used to keep only the information to be deisplayed about a Classe object
            List<ClasseDto> ClasseDtos = new List<ClasseDto> { };

            //Transfering Classe object to data transfer object
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
        /// This method allows getting all the modules present in a Classe object
        /// <example>GET: api/ClasseData/GetClasseModules/1</example>
        /// <example>GET: api/ClasseData/GetClasseModules/2</example>
        /// </summary>
        /// <param name="id">ID of the selected Classe object</param>
        /// <returns> The list of modules in the class which ID is given</returns>

        [ResponseType(typeof(IEnumerable<ModuleDto>))]
        public IHttpActionResult GetClasseModules(int id)
        {
            //All modules which classId is the same as the current Classe object classId
            List<Module> Modules = db.Modules.Where(p => p.classId == id).ToList();
            List<ModuleDto> ModuleDtos = new List<ModuleDto> { };

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
        /// This method gets all the pupils registered for the current Classe (course)
        /// <example>GET: api/ClasseData/GetClassePupils/1</example>
        /// <example>GET: api/ClasseData/GetClassePupils/1</example>
        /// </summary>
        /// <param name="id">ID of the selected Classe object</param>
        /// <returns>The list of pupils in the Classe object which ID is given</returns>

        [ResponseType(typeof(IEnumerable<PupilDto>))]
        public IHttpActionResult GetClassePupils(int id)
        {
            //List of all pupils registered in the current Classe (course)
            List<Pupil> Pupils = db.Pupils.Where(p => p.classId == id).ToList();
            List<PupilDto> PupilDtos = new List<PupilDto> { };

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
        /// This method allows getting the specified Classe object
        /// <example>GET: api/ClasseData/FindClasse/1</example>
        /// <example>GET: api/ClasseData/FindClasse/2</example>
        /// </summary>
        /// <param name="id"> ID of the selected Classe</param>
        /// <returns> This method returns the Classe object which id is given</returns>
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
            //A data transfer object model used to show only most important information about the Classe
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
        /// This method permits to update the selected Classe object
        /// <example>api/ClasseData/UpdateClasse/1</example>
        /// <example>api/ClasseData/UpdateClasse/2</example>
        /// </summary>
        /// <param name="id">The ID of the Classe</param>
        /// <param name="Classe">The current Classe Object itself</param>
        /// <returns>Saves the current Classe with new values to the database</returns>
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
        /// This method permits to add a new Classe object to the database
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
        /// This method deletes the Classe object which ID is given
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