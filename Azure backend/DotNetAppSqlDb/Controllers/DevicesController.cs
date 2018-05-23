using DotNetAppSqlDb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace DotNetAppSqlDb.Controllers
{
    public class DevicesController : ApiController
    {
        private MyDatabaseContext db = new MyDatabaseContext();

        [Authorize]
        public IHttpActionResult GetDevice(int? id = null)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var userId = int.Parse(identity.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            dynamic device;
            if(id != null)
                device = db.Devices.Where(d => d.UserID == userId && d.ID == id).Include(c => c.Temperatures).SingleOrDefault();
            else
                device = db.Devices.Where(d => d.UserID == userId).Include(c => c.Temperatures);

            if (device == null)
            {
                return NotFound();
            }

            return Ok(device);
        }


        [Authorize]
        public IHttpActionResult PutDevice(IoTDevice ioTdevice)
        {
            if (ioTdevice == null)
                return BadRequest("Arguments cannot be null.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var identity = (ClaimsIdentity)User.Identity;
            var userId = int.Parse(identity.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if(userId != 0)
            {
                ioTdevice.UserID = userId;
                var device = db.Devices.Include(c => c.Temperatures).SingleOrDefault(d => d.Name == ioTdevice.Name);
                //device = ioTdevice;
                if(device == null)
                {
                    device = ioTdevice;
                    db.Devices.Add(device);
                }
                else
                {
                    device.Name = ioTdevice.Name;
                    device.MaxTemp = ioTdevice.MaxTemp;
                    device.MinTemp = ioTdevice.MinTemp;
                    device.UserID = ioTdevice.UserID;
                    db.Devices.Attach(device);
                    db.Entry(device).State = EntityState.Modified;
                }

                db.SaveChanges();
                return Created("DefaultApi", device);
            }
            return BadRequest("Sumting went wong");

        }
    }
}
