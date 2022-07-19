using la_mia_pizzeria_static.Database;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                IQueryable<Pizza> listaPizze = context.Pizze.Include(p => p.Categoria);

                return View("Menu", listaPizze.ToList());
            } 
        }


        [HttpGet]
        public IActionResult DettaglioPizza(int id)
        {
            using (PizzaContext context = new PizzaContext())
            {
                Pizza current = context.Pizze.Where(pizza => pizza.ID == id).Include(p => p.Categoria).FirstOrDefault();

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

            using(PizzaContext context = new PizzaContext())
            {
                List<Categoria> categorias = context.Categorie.ToList();

                PizzaCategories pizzaCategories = new PizzaCategories();

                pizzaCategories.Categorias = categorias;
                pizzaCategories.Pizza = new Pizza();

                return View(pizzaCategories);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaCategories data)
        {
            using (PizzaContext context = new PizzaContext())
            {
                if (!ModelState.IsValid)
                {
                    data.Categorias = context.Categorie.ToList();
                    return View("Create", data);
                }
           
                context.Pizze.Add(data.Pizza);
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


        [HttpGet]
        public IActionResult Update(int id)
        {
            using (PizzaContext context = new PizzaContext())
            {
                Pizza pizza = context.Pizze.Where(pizza => pizza.ID == id).FirstOrDefault();

                if (pizza == null)
                {
                    return NotFound("La pizza non esiste");
                }
                else
                {
                    PizzaCategories model = new PizzaCategories();

                    model.Categorias = context.Categorie.ToList();
                    model.Pizza = pizza;
                    
                    return View(model);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaCategories data)
        {
            using (PizzaContext context = new PizzaContext())
            {
                    if (!ModelState.IsValid)
                {
                    data.Categorias = context.Categorie.ToList();
                    return View("Update", data);
                }
           
             
                Pizza editPizza = context.Pizze.Where(pizza => pizza.ID == id).FirstOrDefault();

                if(editPizza == null)
                {
                    return NotFound();
                }
                else
                {
                    editPizza.Description = data.Pizza.Description;
                    editPizza.Name = data.Pizza.Name;
                    editPizza.Price = data.Pizza.Price;
                    editPizza.Img = data.Pizza.Img;

                    editPizza.CategoriaId = data.Pizza.CategoriaId;
                    context.SaveChanges();
                }
                return RedirectToAction("Menu");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
           
            using (PizzaContext context = new PizzaContext())
            {

                Pizza pizza = context.Pizze.Where(pizza => pizza.ID == id).FirstOrDefault();
               
                if(pizza == null)
                {
                    return NotFound();
                }

                context.Pizze.Remove(pizza);
                context.SaveChanges();

                return RedirectToAction("Menu");
            }
        }


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