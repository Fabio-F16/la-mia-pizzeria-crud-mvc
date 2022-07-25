using la_mia_pizzeria_static.Database;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace la_mia_pizzeria_static.Controllers
{
    [Authorize]
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
                IQueryable<Pizza> listaPizze = context.Pizze.Include(p => p.Categoria).Include(p => p.ListaIngredienti);

                return View("Menu", listaPizze.ToList());
            } 
        }


        [HttpGet]
        public IActionResult DettaglioPizza(int id)
        {
            using (PizzaContext context = new PizzaContext())
            {
                Pizza current = context.Pizze.Where(pizza => pizza.ID == id).Include(p => p.Categoria).Include(p => p.ListaIngredienti).FirstOrDefault();

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

                PizzaCategories pizzaModel = new PizzaCategories();

                List<SelectListItem> listIngredients = new List<SelectListItem>();

                List<Ingrediente> ingredients = context.ListaIngredienti.ToList();


                foreach (Ingrediente ingrediente in ingredients)
                {
                    listIngredients.Add(new SelectListItem() { Text = ingrediente.Name, Value = ingrediente.Id.ToString() });
                }

                pizzaModel.Ingredients = listIngredients;

                pizzaModel.Categorias = categorias;
                pizzaModel.Pizza = new Pizza();

                return View(pizzaModel);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaCategories pizzaModel)
        {
            using (PizzaContext context = new PizzaContext())
            {
                if (!ModelState.IsValid)
                {

                    List<Categoria> categories = context.Categorie.ToList();
                    pizzaModel.Categorias = categories;



                    List<SelectListItem> listIngredients = new List<SelectListItem>();
                    List<Ingrediente> ingredients = context.ListaIngredienti.ToList();

                    foreach (Ingrediente ingrediente in ingredients)
                    {
                        listIngredients.Add(new SelectListItem() { Text = ingrediente.Name, Value = ingrediente.Id.ToString() });
                    }

                    pizzaModel.Ingredients = listIngredients;

                    return View("Create", pizzaModel);
                }

                Pizza newPizza = new Pizza();

                newPizza.Name = pizzaModel.Pizza.Name;
                newPizza.Description = pizzaModel.Pizza.Description;
                newPizza.Img = pizzaModel.Pizza.Img;
                newPizza.CategoriaId = pizzaModel.Pizza.CategoriaId;
                newPizza.Price = pizzaModel.Pizza.Price;

                newPizza.ListaIngredienti = new List<Ingrediente>();

                if(pizzaModel.SelectedIngredients != null)
                {
                    foreach(string ingredienteString in pizzaModel.SelectedIngredients)
                    {
                        int ingredienteId = int.Parse(ingredienteString);
                        
                        Ingrediente ingrediente = context.ListaIngredienti.Where(p => p.Id == ingredienteId).FirstOrDefault();
                        newPizza.ListaIngredienti.Add(ingrediente);
                    }
                }



                context.Pizze.Add(newPizza);
                context.SaveChanges();
            }

            return RedirectToAction("Menu");
        }



        // UPDATE GET-----------------------
        [HttpGet]
        public IActionResult Update(int id)
        {
            using (PizzaContext context = new PizzaContext())
            {
                Pizza pizzaToEdit = context.Pizze.Where(pizza => pizza.ID == id).Include(p => p.Categoria).Include(p => p.ListaIngredienti).FirstOrDefault();

                if (pizzaToEdit == null)
                {
                    return NotFound("La pizza non esiste");
                }
                else
                {
                    
                    PizzaCategories pizzaModel = new PizzaCategories();

                    List<SelectListItem> listIngredients = new List<SelectListItem>();
                    List<Ingrediente> ingredients = context.ListaIngredienti.ToList();

                    foreach (Ingrediente ingrediente in ingredients)
                    {
                        listIngredients.Add(new SelectListItem() { Text = ingrediente.Name, Value = ingrediente.Id.ToString() });
                    }

                    pizzaModel.Ingredients = listIngredients;

                    List<Categoria> categorias = context.Categorie.ToList();
                    pizzaModel.Categorias = categorias;
                    pizzaModel.Pizza = pizzaToEdit;
                    pizzaModel.SelectedIngredients = new List<string>();

                    foreach(Ingrediente ingr in pizzaToEdit.ListaIngredienti)
                    {
                        pizzaModel.SelectedIngredients.Add(ingr.Id.ToString());
                    }


                    return View(pizzaModel);
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

                    List<SelectListItem> listIngredients = new List<SelectListItem>();
                    List<Ingrediente> ingredients = context.ListaIngredienti.ToList();

                    foreach (Ingrediente ingrediente in ingredients)
                    {
                        listIngredients.Add(new SelectListItem() { Text = ingrediente.Name, Value = ingrediente.Id.ToString() });
                    }

                    data.Ingredients = listIngredients;

                    return View("Update", data);
                }


                Pizza editPizza = context.Pizze.Where(pizza => pizza.ID == id).Include(p => p.Categoria).Include(p => p.ListaIngredienti).FirstOrDefault();

                if (editPizza == null)
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

                    editPizza.ListaIngredienti.Clear();


                    foreach (string ingredienteString in data.SelectedIngredients)
                    {
                        int ingredienteId = int.Parse(ingredienteString);

                        Ingrediente ingrediente = context.ListaIngredienti.Where(p => p.Id == ingredienteId).FirstOrDefault();
                        editPizza.ListaIngredienti.Add(ingrediente);
                    }


                    context.SaveChanges();
                }
                return RedirectToAction("Menu");
            }
        }


        // delete --------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {

            using (PizzaContext context = new PizzaContext())
            {

                Pizza pizza = context.Pizze.Where(pizza => pizza.ID == id).FirstOrDefault();

                if (pizza == null)
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