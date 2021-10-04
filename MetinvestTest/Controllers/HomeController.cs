using MetinvestTest.DAL;
using MetinvestTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MetinvestTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext db;
        public HomeController(AppDbContext appDbContext)
        {
            this.db = appDbContext;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public JsonResult Create(Employee employee)
        {
            if (!ModelState.IsValid)
            {
               // return BadRequest(ModelState);
            }

            db.Employees.Add(employee);
            db.SaveChanges();

            return Json(new { });
        }
    }
}