using LibraryApi.Models;
using LibraryApi.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly LibraryDbContext _db;

        public AuthorsController(LibraryDbContext context)
        {
            _db = context;
        }

        // GET: api/authors
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_db.Authors);
        }

        // GET api/authors/{id}
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var result = _db.Authors.Find(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST api/authors
        [HttpPost]
        public ActionResult Post(Author author)
        {
            _db.Authors.Add(author);
            _db.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = author.Id }, author);
        }

        // PUT api/authors/{id}
        [HttpPut("{id}")]
        public ActionResult Put(int id, Author author)
        {
            if (id != author.Id) return BadRequest();

            _db.Entry(author).State = EntityState.Modified;
            _db.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = author.Id }, author);
        }
    }
}
