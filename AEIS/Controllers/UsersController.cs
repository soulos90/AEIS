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
        public IHttpActionResult GetUser(string uId)
        {
            User user = db.Users.Find(uId);
            user.LastUsed = DateTime.Now;

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        public User GetU(string uId)
        {
            User user = db.Users.Find(uId);
            return user;
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
            user.LastUsed = DateTime.Now;

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
        public bool PutUser( User user)
        {
            user.LastUsed = DateTime.Now;

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.ID))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            user.LastUsed = user.Created = DateTime.Now;
            user.PassSalt = GenerateSalt();
            user.PassHash = HashPassword(user.PassHash,user.PassSalt);
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