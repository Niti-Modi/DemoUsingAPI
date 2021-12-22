using DemoUsingApiWithDatabase.Models;
using DemoUsingApiWithDatabase.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoUsingApiWithDatabase.Controllers
{
    //added a branch under main
    //added data to feature file
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBooksRepository _bookRepository;

        public BooksController(IBooksRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        //since we are making this action as async we need to write await, and hence bookrepo.getAllBooks should also be asyncronous using async await
        [HttpGet("Get all books")]
        public async Task<List<BooksModel>> GetAllBooks() {

            var listOfBooks = await _bookRepository.GetAllBooks();
            return listOfBooks;
        }


        //here we are sending 2 types of data 1 is ActionResult and another is of type bookModel 
        //Since we are using Asynchronous programming we use Task<> now if we want to return only actions then we write 
        //Task<IActionResult> or Task<ActionResult>
        //But if we want to return object along with action we use Task<ActionResult<tyoe of object>>

        [HttpGet("{id}")]
        public async Task<ActionResult<BooksModel>> GetBookById([FromRoute] int id) {

            var book = await _bookRepository.GetBookById(id);
            if (book == null) {

                return NotFound();
            }


            return book;
        }
        //creating http post without using async await  and Task.
        [HttpPost("addbook")]
        public int AddBook(BooksModel book) {

            var id = _bookRepository.AddBook(book);
            return id;
        }
        //since we are adding a new resource we need status 201 status ok
        [HttpPost("async")]
        public async Task<IActionResult> AddBookAsync(BooksModel booksModel) {

            var id = await _bookRepository.AddBookAsync(booksModel);

            return CreatedAtAction(nameof(GetBookById), new { id = id, controller = "books" }, id);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook([FromBody] BooksModel booksModel, [FromRoute] int id) {

            await _bookRepository.UpdateBook(id, booksModel);
            return Ok();

        }

        [HttpPut("api/{id}")]
        public async Task<IActionResult> UpdateBook2([FromBody] BooksModel bookModel, [FromRoute] int id) {

            await _bookRepository.UpdateBook2(id, bookModel);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook([FromRoute] int id) {

            await _bookRepository.DeleteBook(id);

            return Ok();
        
        }

        [HttpGet("test/{id}")]
        public ActionResult<IEnumerable<BooksModel>> GetBook(int  id) {

            var tableData = _bookRepository.GetBook(id);

            return Ok(tableData);
        }

        [HttpGet("Test")]
        public void Test()
        {
            _bookRepository.TestQueries();

        }

        [HttpGet("TestLinq")]
        public IActionResult TestLinQ() {

            var data = _bookRepository.TestLinQ();
            return Ok(data);
        }

        [HttpGet("TestQuery")]
        public IActionResult TestQuery()
        {

           var data =  _bookRepository.TestQueries();
           return Ok(data);
        }


    }
}
