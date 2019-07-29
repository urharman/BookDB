using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BookDB.Models;
using Microsoft.AspNetCore.Http;
using BookDB.DAL;
using Microsoft.EntityFrameworkCore;
using BookDB.Models;
using BookDB.DAL;

namespace BookDB.Controllers
{
    public class HomeController : Controller
    {
        public List<string[]> display = new List<string[]> { };
        public List<string> DataString = new List<string> { };
        public List<Book> list = new List<Book>();
        public readonly mySQLdbContext _context;

        public string[] Data_S;

        public HomeController(mySQLdbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
        
            return View();
        }

        [HttpPost]
        public ActionResult Create(IFormCollection formCollection)
        {
            string bid = Request.Form["bid"];
            string bname = Request.Form["bname"];
            string bauthor = Request.Form["bauthor"];
            string bprice = Request.Form["bprice"];

            try
            {
                Book book = new Book();
                book.ID_Book = bid;
                book.Name_Book = bname;
                book.Name_Author = bauthor;
                book.Price_Book = bprice;
                _context.Add(book);
                _context.SaveChanges();

            }catch(Exception e)
            {
                /*
                 * Error handling
                 */
                Data_S = rData();
                String data = bid + ',' + bname + ',' + bauthor + ',' + bprice;
                sData(data);
            }

            return View();
           
       }

        [HttpGet]
        public ActionResult Read()
        {
            try
            {
                list = _context.Books.ToList();
                var bID = _context.Books.Include(e=> e.ID_Book);
                var bName = _context.Books.Include(e => e.Name_Book);
                var bAuth = _context.Books.Include(e => e.Name_Author);
                var bPrice = _context.Books.Include(e => e.Price_Book);
        }
            catch (Exception ex)
            {
                Data_S = rData();
                foreach (var line in Data_S)
                {
                    string[] localData = new string[4];
                    localData = line.Split(",");
                    display.Add(localData);
                }
                ViewData["Display"] = display;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Delete(IFormCollection formCollection)
        {

            string bid = Request.Form["bid"];
            
            try
            {

                Book book = _context.Books.Where(e => e.ID_Book == bid).FirstOrDefault();
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                /*
                 * Error handling
                 */
                findBook(bid);
            }

            return View();

        }

        [HttpPost]
        public ActionResult Update(IFormCollection formCollection)
        {
            string bid = Request.Form["bid"];
            string bname = Request.Form["bname"];
            string bauthor = Request.Form["bauthor"];
            string bprice = Request.Form["bprice"];

            try
            {
                var book = _context.Books.SingleOrDefault(e => e.ID_Book == bid);

                if (book != null)
                {
                    book.ID_Book = bid;
                    book.Name_Book = bname;
                    book.Price_Book = bauthor;
                    book.Name_Author = bprice;
                    _context.SaveChanges();
                }

            }
            catch (Exception)
            {
                findBook(bid);
                String data = bid + ',' + bname + ',' + bauthor + ',' + bprice;
                sData(data);
            }
         

            return View();

        }

        private string[] rData()
        {
            return System.IO.File.ReadAllLines("mylib.dat");
        }

        private void sData(String Data)
        {
            System.IO.File.AppendAllText("mylib.dat", Data + Environment.NewLine);
        }

        private void findBook(string bid)
        {
            Data_S = rData();
            foreach (var line in Data_S)
            {
                DataString.Add(line.Split(",")[0]);
            }

            int counter = 0;
            foreach (string j in DataString)
            {
                if (j == bid)
                {
                    break;
                }
                counter++;
            }
            DataString = Data_S.ToList();
            try
            {
                DataString.RemoveAt(counter);
            }
            catch (Exception)
            {
                //
            }
            System.IO.File.WriteAllLines("mylib.dat", DataString);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
