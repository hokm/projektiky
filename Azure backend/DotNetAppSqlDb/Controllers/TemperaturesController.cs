using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using DotNetAppSqlDb.Models;
using Newtonsoft.Json;

namespace DotNetAppSqlDb.Controllers
{
    public class TemperaturesController : ApiController
    {
        private MyDatabaseContext db = new MyDatabaseContext();

        [Authorize]
        public IHttpActionResult GetTemperatures(int DeviceId, DateTime? from = null, DateTime? to = null)
        {
            if (!from.HasValue)
                from = DateTime.MinValue;
            if (!to.HasValue)
                to = DateTime.MaxValue;
            if (DeviceId == 0)
                return BadRequest("Arguments cannot be null. Required arguments are email, password, firstName, lastName");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //kto žiada?
            var identity = (ClaimsIdentity)User.Identity;
            var userId = int.Parse(identity.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var device = db.Devices.SingleOrDefault(d => d.UserID == userId && d.ID == DeviceId);
            if (device == null)
            {
                return BadRequest("You do not own this device, you can try to register it by logging in with your device");
            }
            var temperatures = db.Temperatures.Where(t => t.IoTDeviceID == DeviceId && t.Date >= from && t.Date <= to);

            return Ok(temperatures);
        }

        [Authorize]
        public IHttpActionResult PostTemperature(Temperature temperature)
        {
            if (temperature == null)
                return BadRequest("Arguments cannot be null. Required arguments are email, password, firstName, lastName");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //kto žiada?
            var identity = (ClaimsIdentity)User.Identity;
            var userId = int.Parse(identity.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            //vlastni vôbec to zariadenie? ak áno najdi ho
            var device = db.Devices.SingleOrDefault(d => d.UserID == userId && d.ID == temperature.IoTDeviceID);
            //var device = userDevices.SingleOrDefault(d => d.ID == temperature.IoTDeviceID);
            if(device == null)
            {
                return BadRequest("You do not own this device, you can try to register it using PUT /api/Devices");
            }
            else
            {
                db.Temperatures.Add(temperature);
                db.SaveChanges();
                return Created("DefaultApi", temperature);
            }
        }
    }
}