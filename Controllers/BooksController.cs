using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIEFSample.Data;
using WebAPIEFSample.Models;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

namespace WebAPIEFSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        static readonly HttpClient client = new HttpClient();
        private readonly WebAPIEFSampleContext _context;


        public BooksController(WebAPIEFSampleContext context)
        {
            this._context = context;

        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBook()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://fakerestapi.azurewebsites.net/api/v1/Books");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                IEnumerable<Book> books = JsonConvert.DeserializeObject<IEnumerable<Book>>(responseBody);
                return books.ToList();

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return null;
            }

        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"https://fakerestapi.azurewebsites.net/api/v1/Books/{id}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Book book = JsonConvert.DeserializeObject<Book>(responseBody);

                if (book == null)
                {
                    return NotFound();
                }

                return book;
            }
            catch
            {
                return NoContent();
            }
        }
        // POST: api/Books
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            try
            {
                Book libro = new Book();
                DateTime dateTime = new DateTime();
                libro.Id = 1;
                libro.Title = "Book 1";
                libro.PageCount = 100;
                libro.Excerpt = "Sadipscing accusam elitr dolores lorem";
                libro.PublishDate = dateTime.Date;
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(libro), Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                HttpResponseMessage response = await client.PostAsync("https://fakerestapi.azurewebsites.net/api/v1/Books", httpContent);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Book bookResponse = JsonConvert.DeserializeObject<Book>(responseBody);
                return bookResponse;
            }
            catch
            {
                return NoContent();
            }

        }


        // PUT: api/Books/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult> PutBook(Book book)
        {
            try
            {
                Book libro = new Book();
                DateTime dateTime = new DateTime();
                libro.Id = 1;
                libro.Title = "Book 1";
                libro.PageCount = 100;
                libro.Excerpt = "Sadipscing accusam elitr dolores lorem";
                libro.PublishDate = dateTime.Date;
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(libro), Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PutAsync($"https://fakerestapi.azurewebsites.net/api/v1/Books/{libro.Id}", httpContent);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                return Ok();
            }
            catch
            {
                return NoContent();
            }

        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync($"https://fakerestapi.azurewebsites.net/api/v1/Books/{id}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                return Ok(response.Content);
            }
            catch
            {
                return NotFound();
            }

        }
    

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
