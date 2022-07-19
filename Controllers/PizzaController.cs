using la_mia_pizzeria_static.Database;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace la_mia_pizzeria_static.Controllers
{
    public class PizzaController : Controller
    {
        private readonly ILogger<PizzaController> _logger;

        public PizzaController(ILogger<PizzaController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Menu()
        {
            using(PizzaContext context = new PizzaContext())
            {
                List<Pizza> listaPizze = context.Pizze.ToList();

                return View("Menu", listaPizze);
            }
           
        }


        [HttpGet]
        public IActionResult DettaglioPizza(int id)
        {
            using (PizzaContext context = new PizzaContext())
            {
                Pizza current = context.Pizze.Where(pizza => pizza.ID == id).FirstOrDefault();

                if(current == null)
                {
                    return NotFound("La pizza non esiste");
                }
                else
                {
                    return View(current);
                } 
            }
        }

        [HttpGet]
        public IActionResult ChiSiamo()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pizza pizza)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", pizza);
            }
            using (PizzaContext context = new PizzaContext())
            {
                context.Pizze.Add(pizza);
                context.SaveChanges();
            }

            return RedirectToAction("Menu");
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(string name, string description, string img, double price)
        //{

        //    using (PizzaContext context = new PizzaContext())
        //    {
        //        Pizza nuovaPizza = new Pizza();
        //        nuovaPizza.Name = name;
        //        nuovaPizza.Description = description;
        //        nuovaPizza.Img = img;

        //        if(price > 0)
        //        {
        //            nuovaPizza.Price = price;
        //        }


        //        context.Pizze.Add(nuovaPizza);
        //        context.SaveChanges();
        //    }

        //    return RedirectToAction("Menu");
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}