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

namespace StateTemplateV5Beta.Controllers
{
    public class AnswersController : ApiController
    {
        private DBAContext db = new DBAContext();
        private DBUContext dbu = new DBUContext();
        // GET: api/Answers
        public IQueryable<Answer> GetAnswers()
        {
            return db.Answers;
        }

        // GET: api/Answers/5
        [ResponseType(typeof(Answer))]
        public IHttpActionResult GetAnswer(string id)
        {
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return NotFound();
            }
            answer.LastUsed = new DateTime().Date;
            return Ok(answer);
        }
        public Answer GetA(string id)
        {
            Answer answer = db.Answers.Find(id);
            if(answer!=null)
                PutAnswer(id, answer);
            return answer;
        }
        // PUT: api/Answers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAnswer(string id, Answer answer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != answer.UId)
            {
                return BadRequest();
            }
            if (db.Answers.Find(id) == null)
                answer.Created = new DateTime().Date;
            answer.LastUsed = new DateTime().Date;
            db.Entry(answer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(id))
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

        // POST: api/Answers
        [ResponseType(typeof(Answer))]
        public IHttpActionResult PostAnswer(Answer answer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (db.Answers.Find(new {answer.UId, answer.AId }) == null)
                answer.Created = new DateTime().Date;
            answer.LastUsed = new DateTime().Date;
            db.Answers.Add(answer);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AnswerExists(answer.UId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = answer.UId }, answer);
        }

        // DELETE: api/Answers/5
        [ResponseType(typeof(Answer))]
        public IHttpActionResult DeleteAnswer(string id)
        {
            int temp=0,temp1=0;
            string tempo="";
            bool check = false;
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return NotFound();
            }
            if(db.Answers.SqlQuery("SELECT AId FROM Answers WHERE UId = " + answer.UId + ";").Count()-1 > answer.AId)
            {
                temp = answer.AId;
                temp1 = db.Answers.SqlQuery("SELECT AId FROM Answers WHERE UId = " + answer.UId + ";").Count() - 1;
                tempo = answer.UId;
                check = true;
            }
            db.Answers.Remove(answer);
            if(check)
            {
                PutAnswer(id,db.Answers.Find(new { tempo, temp1 }));
                db.Answers.Remove(db.Answers.Find(new { tempo, temp1 }));
            }
            db.SaveChanges();

            return Ok(answer);
        }
        // returns index of next answer id (fun fact it won't always be chronological)
        public int Next(string id)//this id is just email not a touple
        {
            
            return db.Answers.SqlQuery("SELECT AId FROM Answers WHERE UId = " + id + ";").Count();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AnswerExists(string id)
        {
            return db.Answers.Count(e => e.UId == id) > 0;
        }

    }
}