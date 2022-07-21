using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace la_mia_pizzeria_static.Models
{
    public class Ingrediente
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Pizza> Pizzas { get; set; }


        public Ingrediente()
        {

        }
        public Ingrediente(string name)
        {
            Name = name;
        }
    }
}
