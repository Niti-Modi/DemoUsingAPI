using DemoUsingApiWithDatabase.Data;
using DemoUsingApiWithDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoUsingApiWithDatabase.Repository
{
    public interface IBooksRepository
    {
        public  Task<List<BooksModel>> GetAllBooks();
        public Task<BooksModel> GetBookById(int bookId);

        public int AddBook(BooksModel bookModel);

        Task<int> AddBookAsync(BooksModel booksModel);

        Task UpdateBook(int bookId, BooksModel booksModel);

        public Task UpdateBook2(int bookId, BooksModel bookModel);

        public Task DeleteBook(int bookId);


        IEnumerable<BooksModel> GetBook(int bookId);


        

        IEnumerable<IGrouping<string, Books>> TestLinQ();

        List<customersOrderModel> TestQueries();
    }
}
