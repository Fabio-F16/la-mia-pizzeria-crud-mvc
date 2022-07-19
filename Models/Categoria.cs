using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace la_mia_pizzeria_static.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Pizza> Pizzas { get; set; }


        public Categoria()
        {

        }
        public Categoria(string name)
        {
            Name = name;  
        }
    }
}
