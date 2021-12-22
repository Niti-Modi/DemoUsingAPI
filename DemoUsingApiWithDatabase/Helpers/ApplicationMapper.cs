using AutoMapper;
using DemoUsingApiWithDatabase.Data;
using DemoUsingApiWithDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DemoUsingApiWithDatabase.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() {

            CreateMap<Books, BooksModel>();
        }
    }
}
