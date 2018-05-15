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
            answer.LastUsed = DateTime.Now;
            return Ok(answer);
        }
        public Answer GetA(string uid, int aid, int qid)
        {
            return (from t in db.Answers where ((uid == t.UId) & (qid == t.QId) & (aid == t.AId)) select t).FirstOrDefault();
        }
        public Answer GetAnswer(string uId, int aId)
        {
            Answer answer = (from t in db.Answers where ((uId == t.UId) & (aId == t.AId) & (1 == t.QId)) select t).FirstOrDefault();
            return answer;
        }
        // PUT: api/Answers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAnswer(string uId, Answer answer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (uId != answer.UId)
            {
                return BadRequest();
            }

            answer.LastUsed = DateTime.Now;

            db.Entry(answer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(uId))
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

            answer.Created = DateTime.Now;
            answer.LastUsed = DateTime.Now;

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
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return NotFound();
            }

            db.Answers.Remove(answer);
            db.SaveChanges();

            return Ok(answer);
        }
        public void DeleteAnswer(string uid,int aid,int qid)
        {
            Answer answer = db.Answers.Find(new object[] { uid, aid, qid });
            if (answer == null)
            {

            }
            else
            {
                db.Answers.Remove(answer);
                db.SaveChanges();
            }
        }
        [ResponseType(typeof(Answer))]
        public IHttpActionResult DeleteWholeAnswer(string uid,int aid)
        {
            int temp = 0, temp1 = 0;
            string tempo = "";
            bool check = false;
            int i = 1;
            Answer answer = db.Answers.Find(new object[] { uid, aid, i });
            if (answer == null)
            {
                return NotFound();
            }
            int count = CountAPU(uid)-1;
            if (count > answer.AId)
            {
                temp = answer.AId;
                temp1 = count;
                tempo = answer.UId;
                check = true;
            }
            for(int e = 1;e<= Convert.ToInt32(MvcApplication.environment.GetQuestionCount());++e)
            {
                DeleteAnswer(uid,aid,e);
            }
            if(check)
            {
                for(i =1;i<=Convert.ToInt32(MvcApplication.environment.GetQuestionCount());++i)
                {
                    Answer a = new Answer(GetA(tempo,count,i));

                    a.AId = temp;
                    DeleteAnswer(tempo, temp1, i);
                    DeleteAnswer(tempo, count, i);
                    PostAnswer(a);
                }
            }

            return Ok(answer);
        }

        // returns next available AId for new survey
        public int GetNextAId(string uId)
        {
            var result = (from Answers in db.Answers
                          orderby Answers.AId
                          where Answers.UId == uId
                          select Answers.AId).Distinct().ToList();

            return result.Count();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AnswerExists(string uId)
        {
            return db.Answers.Count(e => e.UId == uId) > 0;
        }
        public int CountAPS(string uid, int aid)
        {
            return db.Answers.Count(e => e.UId == uid & e.AId == aid);
        }
        public int CountAPU(string uid)
        {
            return db.Answers.Count(e => e.UId == uid & e.QId == 1);
        }
    }
}