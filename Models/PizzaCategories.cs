using Microsoft.AspNetCore.Mvc.Rendering;

namespace la_mia_pizzeria_static.Models
{
    public class PizzaCategories
    {
        public Pizza Pizza { get; set; }
        public List<Categoria>? Categorias { get; set; }


        //read
        public List<SelectListItem>? Ingredients { get; set; }

        //create update 
        public List<string>? SelectedIngredients { get; set; }
    }
}
