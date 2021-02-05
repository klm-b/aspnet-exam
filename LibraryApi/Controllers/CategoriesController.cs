using LibraryApi.Models;
using LibraryApi.Models.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly LibraryDbContext _db;

        public CategoriesController(LibraryDbContext context)
        {
            _db = context;
        }

        // GET: api/categories
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_db.Categories);
        }

        // GET api/categories/{id}
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var result = _db.Categories.Find(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST api/categories
        [HttpPost]
        public ActionResult Post(Category category)
        {
            _db.Categories.Add(category);
            _db.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }

        // PUT api/categories/{id}
        [HttpPut("{id}")]
        public ActionResult Put(int id, Category category)
        {
            if (id != category.Id) return BadRequest();

            _db.Entry(category).State = EntityState.Modified;
            _db.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }
    }
}
