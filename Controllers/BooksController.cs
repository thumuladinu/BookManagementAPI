using Microsoft.AspNetCore.Mvc;
using BookManagementAPI.Models;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

namespace BookManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private static readonly string FilePath = "books.json";
        private static List<Book> _books = LoadFromFile();
        private static int _nextId = _books.Any() ? _books.Max(b => b.Id) + 1 : 1;

        private static List<Book> LoadFromFile()
        {
            if (!System.IO.File.Exists(FilePath))
                return new List<Book>();

            var json = System.IO.File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
        }

        private static void SaveToFile() =>
            System.IO.File.WriteAllText(FilePath, JsonSerializer.Serialize(_books));

        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetBooks() => _books;

        [HttpPost]
        public ActionResult<Book> PostBook(Book book)
        {
            book.Id = _nextId++;
            _books.Add(book);
            SaveToFile();
            return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public IActionResult PutBook(int id, Book updated)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();

            book.Title = updated.Title;
            book.Author = updated.Author;
            book.Year = updated.Year;
            SaveToFile();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();

            _books.Remove(book);
            SaveToFile();
            return NoContent();
        }
    }
}
