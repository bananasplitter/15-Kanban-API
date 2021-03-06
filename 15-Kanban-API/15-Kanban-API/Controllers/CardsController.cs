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
using _15_Kanban_API;
using AutoMapper;
using _15_Kanban_API.Models;

namespace _15_Kanban_API.Controllers
{
    public class CardsController : ApiController
    {
        private KanbanDataLayerEntities db = new KanbanDataLayerEntities();

        // GET: api/Cards/
        public IEnumerable<CardModel> GetCards()
        {
            return Mapper.Map<IEnumerable<CardModel>>(db.Cards);
        }  

        // GET: api/Cards/5
        [ResponseType(typeof(CardModel))]
        public IHttpActionResult GetCard(int id)
        {
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<CardModel>(card));
        }

        // PUT: api/Cards/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCard(int id, CardModel card)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != card.CardId)
            {
                return BadRequest();
            }

            #region

            var dbCard = db.Cards.Find(id);
            dbCard.Update(card);
            db.Entry(dbCard).State = EntityState.Modified;

            #endregion

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardExists(id))
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

        // POST: api/Cards
        [ResponseType(typeof(CardModel))]
        public IHttpActionResult PostCard(CardModel card)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbCard = new Card(card);

            db.Cards.Add(dbCard);
            db.SaveChanges();

            card.CreatedDate = dbCard.CreatedDate;
            card.CardId = dbCard.CardId;

            return CreatedAtRoute("DefaultApi", new { id = dbCard.CardId }, card);
        }

        // DELETE: api/Cards/5
        [ResponseType(typeof(CardModel))]
        public IHttpActionResult DeleteCard(int id)
        {
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return NotFound();
            }

            db.Cards.Remove(card);
            db.SaveChanges();

            return Ok(Mapper.Map<CardModel>(card));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CardExists(int id)
        {
            return db.Cards.Count(e => e.CardId == id) > 0;
        }
    }
}