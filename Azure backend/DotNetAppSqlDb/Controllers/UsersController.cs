using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using DotNetAppSqlDb.Models;
using Newtonsoft.Json;

namespace DotNetAppSqlDb.Controllers
{
    public class UsersController : ApiController
    {
        private MyDatabaseContext db = new MyDatabaseContext();

        public IHttpActionResult PostUser(User user)
        {
            if(user == null)
                return BadRequest("Arguments cannot be null. Required arguments are email, password, firstName, lastName");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if(db.Users.Count() == 0)
            {
                user.Role = "admin";
                db.Users.Add(user);
                db.SaveChanges();
                return Created("DefaultApi", user);
            }
            if (db.Users.Where(u => u.Email == user.Email).Count() > 0)
                return BadRequest("Email is already used");

            user.Role = "user";
            db.Users.Add(user);
            db.SaveChanges();

            return Created("DefaultApi", user);
        }

    }
}
