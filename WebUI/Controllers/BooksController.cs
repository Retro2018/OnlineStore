﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassLibrary.Abstract;
using WebUI.Models;
using ClassLibrary.Concrete;

namespace WebUI.Controllers
{
    public class BooksController : Controller
    {
        private IBookRepository _repository;
        public int pageSize = 4;// Number of books per page
        
        public BooksController()
        {
            _repository = new EFBookRepository();
        }

        public ViewResult List(string genre, int page = 1)
        {
            BooksListViewModel model = new BooksListViewModel
            {
                Books = _repository.Books
                .Where(b => genre == null || b.Genre == genre)
                .OrderBy(book => book.BookId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = genre == null ?
                        _repository.Books.Count() :
                        _repository.Books.Where(book => book.Genre == genre).Count()
                },
                CurrentGenre = genre
            };

            return View(model);
        }
    }
}