using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Models
{
    public class Pizza
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio!")]
        [StringLength(50, ErrorMessage ="Superato limite caratteri")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio!")]
        [StringLength(150, ErrorMessage = "Superato limite caratteri")]  
        public string Description { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio!")]
        [StringLength(200, ErrorMessage = "Superato limite caratteri")]
        public string Img { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio!")]
        [Range(1, 15, ErrorMessage ="Non può essere zero")]
        public double Price { get; set; }

        public List<Ingrediente>? ListaIngredienti { get; set; }

        public int? CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        public Pizza()
        {

        }
        public Pizza(string name, string description, string img, double price)
        {
            Name = name;
            Description = description;
            Img = img;
            Price = price;
        }
    }
}
