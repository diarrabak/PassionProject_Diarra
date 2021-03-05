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
    public class LocationDataController : ApiController
    {
       
        //Connection to the database containing Classes, Modules, Locations and Pupils
        private PassionDataContext db = new PassionDataContext();

        /// <summary>
        /// This method gets the list of all the locations from the database
        /// <example> GET: api/LocationData/GetLocations </example>
        /// </summary>
        /// <returns> The list of Locations from the database</returns>

        [ResponseType(typeof(IEnumerable<LocationDto>))]
        public IHttpActionResult GetLocations()
        {
            //List of all the locations in the database
            List<Location> Locations = db.Locations.ToList();

            //Data transfer object used to display information of location object
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
        /// This method gets the list of all the pupils living in the selected location
        /// </summary>
        /// <param name="id">ID of the location</param>
        /// <returns>Pupil list of the current location</returns>
        [ResponseType(typeof(IEnumerable<PupilDto>))]
        public IHttpActionResult GetLocationPupils(int id)
        {
            //List of pupil which location ID is the same as the selected location ID
            List<Pupil> pupils = db.Pupils.Where(p => p.locId == id).ToList();
            List<PupilDto> PupilDtos = new List<PupilDto> { };

            foreach (var pupil in pupils)
            {
                PupilDto NewPupil = new PupilDto
                {
                    pId = pupil.pId,
                    firstName = pupil.firstName,
                    lastName = pupil.lastName,
                    age = pupil.age,
                };
                PupilDtos.Add(NewPupil);
            }

            return Ok(PupilDtos);
        }

        /// <summary>
        /// <example>GET: api/api/LocationData/FindLocation/1</example>
        /// <example>GET: api/api/LocationData/FindLocation/2</example>
        /// </summary>
        /// <param name="id"> ID of the selected Location</param>
        /// <returns> This method returns the Location which ID is given</returns>
        // 
        [HttpGet]
        [ResponseType(typeof(LocationDto))]
        public IHttpActionResult FindLocation(int id)
        {
            Location Location = db.Locations.Find(id);

            if (Location == null)
            {
                return NotFound();
            }
            LocationDto LocationTemp = new LocationDto
            {
                locId = Location.locId,
                city = Location.city,
                country = Location.country,
                incomeRange = Location.incomeRange
            };

            return Ok(LocationTemp);
        }

        /// <summary>
        /// This method permits to update the selected Location
        /// <example>api/LocationData/UpdateLocation/1</example>
        /// <example>api/LocationData/UpdateLocation/2</example>
        /// </summary>
        /// <param name="id">The ID of the Location</param>
        /// <param name="Location"></param>
        /// <returns>It updates and adds the selected Location to the database</returns>
        // PUT: 
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateLocation(int id, Location Location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Location.locId)
            {
                return BadRequest();
            }

            db.Entry(Location).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
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
        /// This method permits to add a new Location to the database
        /// <example>POST: api/LocationData/AddLocation</example>
        /// </summary>
        /// <param name="Location">Location object to be updated</param>
        /// <returns> It adds a new Location to the database</returns>

        [HttpPost]
        [ResponseType(typeof(Location))]
        public IHttpActionResult AddLocation(Location Location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Locations.Add(Location);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Location.locId }, Location);
        }

        /// <summary>
        /// This method deletes the Location which ID is given
        /// <example>api/LocationData/DeleteLocation/1</example>
        /// <example>api/LocationData/DeleteLocation/3</example>
        /// </summary>
        /// <param name="id">ID of the Location</param>
        /// <returns>Removes the selected location from the database</returns>

        [HttpPost]
        [ResponseType(typeof(Location))]
        public IHttpActionResult DeleteLocation(int id)
        {
            Location Location = db.Locations.Find(id);
            if (Location == null)
            {
                return NotFound();
            }

            db.Locations.Remove(Location);
            db.SaveChanges();

            return Ok(Location);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LocationExists(int id)
        {
            return db.Locations.Count(e => e.locId == id) > 0;
        }
    }
}