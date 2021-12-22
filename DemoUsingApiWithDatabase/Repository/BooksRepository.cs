using AutoMapper;
using DemoUsingApiWithDatabase.Data;
using DemoUsingApiWithDatabase.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoUsingApiWithDatabase.Repository
{
    public class BooksRepository : IBooksRepository
    {
        private readonly BookStoreContext _bookStoreContext;
        private readonly IMapper _mapper;

        public BooksRepository(BookStoreContext bookStoreContext, IMapper mapper)
        {
            _bookStoreContext = bookStoreContext;
            _mapper = mapper;
        }

        public async Task<List<BooksModel>> GetAllBooks() {

            var records = await _bookStoreContext.Books.ToListAsync();

            //converts List<Books> -> List<BooksModel>
            return _mapper.Map<List<BooksModel>>(records);
        }



        public async Task<BooksModel> GetBookById(int bookId) {

            //var book = await _bookStoreContext.Books.Where(x => x.Id == bookId).Select(x=> new BooksModel() { 
            //    Id =x.Id,
            //    Title=x.Title,
            //    Description=x.Description 

            //}).FirstOrDefaultAsync();

            var book = await _bookStoreContext.Books.FindAsync(bookId);

            //converts book->BookModel
            return _mapper.Map<BooksModel>(book);
            // return book;
        }


        //Since we are using async programming we use FindAsync, Find/FindAsync will only work with primary key.
        //So if we want to fetch data based on some other columns we make use of where clause.  eg: Books.Where(x=>x.Id == bookId).Select().FirstOrDefaultAsync() 
        //we  use FirstOrDefault instead pf ToListAsync since it will return only 1 row
        //FirstAsync : will return error if book with particular id does not exist
        //FirstOrDefault : will return null if book with particular id does not exist


        public int AddBook(BooksModel bookModel) {

            var book = new Books()
            {
                Title = bookModel.Title,
                Description = bookModel.Description

            };
            _bookStoreContext.Books.Add(book);
            _bookStoreContext.SaveChanges();

            return book.Id;

        }


        public async Task<int> AddBookAsync(BooksModel booksModel) {


            Books books = new Books
            {
                Title = booksModel.Title,
                Description = booksModel.Description

            };

            _bookStoreContext.Books.Add(books);
            await _bookStoreContext.SaveChangesAsync();

            return books.Id;
        }

        //Updates db by accessing it twce on line no: 92 and 101 to avoid this we write another method
        public async Task UpdateBook(int bookId, BooksModel booksModel) {

            var book = await _bookStoreContext.Books.FindAsync(bookId);

            if (book != null) {

                book.Title = booksModel.Title;
                book.Description = booksModel.Description;

            }

            await _bookStoreContext.SaveChangesAsync();



        }

        public async Task UpdateBook2(int bookId, BooksModel bookModel) {

            var book = new Books() { Id = bookId, Description = bookModel.Description, Title = bookModel.Title };
            _bookStoreContext.Books.Update(book);
            await _bookStoreContext.SaveChangesAsync();

        }


        public async Task DeleteBook(int bookId) {

            Books book = new Books { Id = bookId };

            _bookStoreContext.Books.Remove(book);
            await _bookStoreContext.SaveChangesAsync();



        }

        public IEnumerable<BooksModel> GetBook(int bookId) {

            //returns book with bookId
            Books book = _bookStoreContext.Books.Find(bookId);

            //returns book with mention title (allquery returns same output irrespective of the fact that Books is mentioned in FirstOrDefault or not)
            Books book1 = _bookStoreContext.Books.Where(s => s.Title == "DSA").FirstOrDefault();
            Books book2 = _bookStoreContext.Books.Where(s => s.Title == "c#").FirstOrDefault<Books>();
            var book5 = _bookStoreContext.Books.Where(s => s.Title == "DSA");
            //return more than 1 row with title as C#. (But returns data of type IQueryable  but still is able to map to IEnumerableof) one is using lambda expression and another using sqllite expression
            var book3 = _bookStoreContext.Books.Where(s => s.Title == "c#");
            var book4 = from record in _bookStoreContext.Books where record.Title == "DSA" select record;


            //query to return ENTIRE TABLE as Enumerable. (converting from IQuerable to Enumerable)
            var entireTable = (from books in _bookStoreContext.Books select books).AsEnumerable();

            //Doubt: able to convert both as IEnumerator<BooksModel> despite both being of different type.
            var entireTable2 = _bookStoreContext.Books.AsEnumerable();
            var entireTable3 = _bookStoreContext.Books;



            //return IEnumerable<book> -> IEnumerator<BooksModel> ---entireTable2
            //returns DbSet<book> -> IEnumerator<BooksModel>      ---entireTable3
            //returns IQueryable<book> -> IEnumerator<BooksModel> ---book3, book4
            return _mapper.Map<IEnumerable<BooksModel>>(entireTable3);


        }


        public List<customersOrderModel> TestQueries() {

            var joinResult= _bookStoreContext.Books.AsEnumerable().GroupJoin(_bookStoreContext.EBooksTable.AsEnumerable(), 
                                                b => b.Id, 
                                                e => e.Id,

                                               (book, ebooks ) => new
                                               {

                                                   Book = book,
                                                   EBooks = ebooks,

                                               });


            var row = joinResult.Where(x => x.Book.Id == 1);
            CombinedModel finalResult = new CombinedModel();
            finalResult.EbooksList = new List<EBooks>();
            EBooks temp = new EBooks();

            foreach (var elem in row) {

               // System.Diagnostics.Debug.WriteLine(elem.Book.Id);
                finalResult.Id = elem.Book.Id;
                finalResult.Title = elem.Book.Title;
                finalResult.Description = elem.Book.Description;

                foreach (var ebook in elem.EBooks) {
               //     System.Diagnostics.Debug.WriteLine(ebook.Id);
                    temp.Id = ebook.Id;
                    temp.Name = ebook.Name;
                    temp.Price = ebook.Price;
                    //finalResult.EBooks = new List<EBooks>();
                    finalResult.EbooksList.Add(temp);
                    
                }

            }



            var innerJoin1 =             from d in _bookStoreContext.CustomersTable
                                         join o in _bookStoreContext.OrdersTable
                                         on d.Id equals o.CustomerID
                                         select new
                                         {
                                             Customer = new { customerName=d.CustomerName, customerId=d.Id },

                                             Order = o.OrderId
                                         };

            var innerJoin2 = (from d in _bookStoreContext.CustomersTable
                             join o in _bookStoreContext.OrdersTable
                             on d.Id equals o.CustomerID
                             select new
                             {
                                 d.Id,
                                 d.CustomerName,
                                 o.OrderId
                             }).ToList().Where(x=>x.Id == 1);

            List < customersOrderModel > customerAndOrders= new List<customersOrderModel>();

            customersOrderModel temp1;
            foreach (var data in innerJoin2) {
                temp1= new customersOrderModel();
                temp1.CustomerName = data.CustomerName;
                temp1.Id = data.Id;
                temp1.OrderId = data.OrderId;
                customerAndOrders.Add(temp1);
                          
            }





            var innerJoin3 = from d in _bookStoreContext.CustomersTable
                             join o in _bookStoreContext.OrdersTable
                             on d.Id equals o.CustomerID
                             select new
                             {
                                 Customer = d,
                                 Order = o
                             };



            return customerAndOrders;
        
        }


        public IEnumerable<IGrouping<string,Books>> TestLinQ() {

            List<int> Numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            IEnumerable<int> evenNumber = Numbers.Where(x => x%2 ==0);
            Console.WriteLine(evenNumber);

            //returns 678910
            //slect & where
            var selectedColFromTable = _bookStoreContext.Books.Where(x => x.Id > 5).Select(x => x.Id);

            //orderby : both returns same data.
            var data = _bookStoreContext.Books.AsEnumerable().OrderBy(x=>x.Title);
            //returns data in ascending order.
            var data1 = _bookStoreContext.Books.OrderBy(x => x.Title);

            var data2 = _bookStoreContext.Books.AsEnumerable().GroupBy(x=>x.Title);

            var data3 = _bookStoreContext.Books.AsEnumerable().GroupBy(x => x.Title).Count();


            //  return _mapper.Map<IEnumerable<int>>(data2);

            return data2;
        }


    }
}
