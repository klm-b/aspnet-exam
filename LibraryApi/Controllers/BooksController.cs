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
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _db;

        public BooksController(LibraryDbContext context)
        {
            _db = context;
        }

        // GET: api/books
        [HttpGet]
        public ActionResult Get()
        {
            var result = _db.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Select(b => new
                {
                    b.Id,
                    b.Title,
                    Author = b.Author.SurnameNP,
                    Category = b.Category.Name,
                    b.Year,
                    b.Price,
                    b.Amount
                });

            return Ok(result);
        }

        // GET api/books/{id}
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var result = _db.Books.Find(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST api/books
        [HttpPost]
        public ActionResult Post(Book book)
        {
            _db.Books.Add(book);
            _db.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
        }

        // PUT api/books/{id}
        [HttpPut("{id}")]
        public ActionResult Put(int id, Book book)
        {
            if (id != book.Id) return BadRequest();

            _db.Entry(book).State = EntityState.Modified;
            _db.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
        }

        // DELETE api/books/{id}
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var book = _db.Books.Find(id);

            if (book == null) return NotFound();
            
            _db.Books.Remove(book);
            _db.SaveChanges();

            return Ok(book);
        }
    }
}
