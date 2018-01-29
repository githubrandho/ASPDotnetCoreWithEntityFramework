using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DutchTreat.Models;
using DutchTreat.Data;
using Microsoft.AspNetCore.Authorization;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private readonly IDutchRepository _repository;
        public AppController(IDutchRepository repository)
        {
            _repository = repository;
        }
        public IActionResult Index()
        {
           /* throw new InvalidOperationException("Bad Thing happened")*/;
            return View();
        }
        [HttpGet("contact")]
        public IActionResult Contact()
        {
            //throw new InvalidOperationException("bad things happen");
            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            //if (ModelState.IsValid)
            //{
                
            //}
           
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Title = "About Page";
            return View();
        }

        [Authorize]
        public IActionResult Shop()
        {

            var results = _repository.GetAllProducts();
            return View(results);
        }
    }
}