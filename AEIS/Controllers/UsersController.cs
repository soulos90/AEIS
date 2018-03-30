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
using StateTemplateV5Beta.Models;
using System.Text;
using System.Security.Cryptography;

namespace StateTemplateV5Beta.Controllers
{
    public class UsersController : ApiController
    {
        private DBUContext db = new DBUContext();

        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(string id)
        {
            User user = db.Users.Find(id);
            user.LastUsed = new DateTime().Date;

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        public User GetU(string id)
        {
            User user = db.Users.Find(id);
            if (user != null)
                PutUser(id, user);
            return user;
        }
        public void Jank()//somewhere to put queries to modify db structure
        {
            db.Users.SqlQuery("ALTER TABLE Users ADD PassSalt string; ");
        }
        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(string id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.ID)
            {
                return BadRequest();
            }
            if(db.Users.Find(id)==null)
                user.created = new DateTime().Date;
            user.LastUsed = new DateTime().Date;

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (db.Users.Find(user.ID) == null)
                user.created = new DateTime().Date;
            user.LastUsed = new DateTime().Date;

            db.Users.Add(user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = user.ID }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(string id)
        {

            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(string id)
        {
            return db.Users.Count(e => e.ID == id) > 0;
        }

        public string GenerateSalt()
        {
            return Guid.NewGuid().ToString("N");
        }

        public string HashPassword(string password, string salt)
        {
            string combined = password + salt;
            return HashString(combined);
        }

        public bool CheckPassword(string password, string salt, string hash)
        {
            return HashPassword(password, salt) == hash;
        }

        private string HashString(string toHash)
        {
            using (SHA512CryptoServiceProvider sha = new SHA512CryptoServiceProvider())
            {
                byte[] dataToHash = Encoding.UTF8.GetBytes(toHash);
                byte[] hashed = sha.ComputeHash(dataToHash);
                return Convert.ToBase64String(hashed);
            }
        }

    }
}