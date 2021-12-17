using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIEFSample.Data;
using WebAPIEFSample.Models;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using System.Web.Http.Cors;

namespace WebAPIEFSample.Controllers
{
    [Route("api/[controller]")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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
                return NotFound();
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

                return book;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return NotFound();
            }
        }
        // POST: api/Books
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(int id, string title, int pagecount, string excerpt, DateTime publishdate)
        {
            try
            {
                Book book = new Book();
                book.Id = id;
                book.Title = title;
                book.PageCount = pagecount;
                book.Excerpt = excerpt;
                book.PublishDate = publishdate;
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                HttpResponseMessage response = await client.PostAsync("https://fakerestapi.azurewebsites.net/api/v1/Books", httpContent);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Book bookResponse = JsonConvert.DeserializeObject<Book>(responseBody);
                return bookResponse;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return Problem();
            }

        }


        // PUT: api/Books/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult> PutBook(int id, string title, int pagecount, string excerpt, DateTime publishdate)
        {
            try
            {
                Book book = new Book();
                book.Id = id;
                book.Title = title;
                book.PageCount = pagecount;
                book.Excerpt = excerpt;
                book.PublishDate = publishdate;
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PutAsync($"https://fakerestapi.azurewebsites.net/api/v1/Books/{book.Id}", httpContent);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                return Ok();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return Problem();
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
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return Problem();
            }

        }
    }
}
