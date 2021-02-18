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
    public class ModuleDataController : ApiController
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
        /// This method process the list of the Modules and return in form of list
        /// <example> GET: api/ModuleData/GetModules </example>
        /// </summary>
        /// <returns> The list of Modules with the first name, last name, classe and Module from the database</returns>

        [ResponseType(typeof(IEnumerable<ModuleDto>))]
        public IHttpActionResult GetModules()
        {

            //return (IHttpActionResult)db.Modules.ToList();
            List<Module> Modules = db.Modules.ToList();
            List<ModuleDto> ModuleDtos = new List<ModuleDto> { };

            //Here you can select the information to be transfered to the  API
            foreach (var Module in Modules)
            {
                ModuleDto NewModule = new ModuleDto
                {
                    modId = Module.modId,
                    moduleName = Module.moduleName,
                    description = Module.description,
                    delivery = Module.delivery,
                    fees =Module.fees,
                    classId=Module.classId
                };
                ModuleDtos.Add(NewModule);
            }

            return Ok(ModuleDtos);
        }

        /// <summary>
        /// <example>GET: api/api/ModuleData/FindModule/1</example>
        /// <example>GET: api/api/ModuleData/FindModule/2</example>
        /// </summary>
        /// <param name="id"> The parameter being the ID of the Module</param>
        /// <returns> This method returns the Module which id is given</returns>
        // 
        [HttpGet]
        [ResponseType(typeof(ModuleDto))]
        public IHttpActionResult FindModule(int id)
        {
            Module Module = db.Modules.Find(id);

            if (Module == null)
            {
                return NotFound();
            }
            ModuleDto ModuleTemp = new ModuleDto
            {
                modId = Module.modId,
                moduleName = Module.moduleName,
                description = Module.description,
                delivery = Module.delivery,
                fees = Module.fees,
                classId = Module.classId
            };

            return Ok(ModuleTemp);
        }

        /// <summary>
        /// This method permits to update the selected Module
        /// <example>api/ModuleData/UpdateModule/1</example>
        /// <example>api/ModuleData/UpdateModule/2</example>
        /// </summary>
        /// <param name="id">The ID of the Module</param>
        /// <param name="Module"></param>
        /// <returns></returns>
        // PUT: 
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateModule(int id, Module Module)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Module.modId)
            {
                return BadRequest();
            }

            db.Entry(Module).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuleExists(id))
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
        /// This method permits to add a new Module to the database
        /// <example>POST: api/ModuleData/AddModule</example>
        /// </summary>
        /// <param name="Module"></param>
        /// <returns> It adds a new Module to the database</returns>

        [HttpPost]
        [ResponseType(typeof(Module))]
        public IHttpActionResult AddModule(Module Module)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Modules.Add(Module);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Module.modId }, Module);
        }

        /// <summary>
        /// This method deletes the Module which ID is given
        /// <example>api/ModuleData/DeleteModule/1</example>
        /// <example>api/ModuleData/DeleteModule/3</example>
        /// </summary>
        /// <param name="id">ID of the Module</param>
        /// <returns></returns>

        [HttpPost]
        [ResponseType(typeof(Module))]
        public IHttpActionResult DeleteModule(int id)
        {
            Module Module = db.Modules.Find(id);
            if (Module == null)
            {
                return NotFound();
            }

            db.Modules.Remove(Module);
            db.SaveChanges();

            return Ok(Module);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ModuleExists(int id)
        {
            return db.Modules.Count(e => e.modId == id) > 0;
        }
    }
}