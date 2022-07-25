using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_static.Database
{
    public class PizzaContext : IdentityDbContext<IdentityUser>
    {
        public PizzaContext(DbContextOptions<PizzaContext> options) : base(options)
        {

        }
     public PizzaContext()
        {

        }
        public DbSet<Pizza> Pizze { get; set; }
        public DbSet<Ingrediente> ListaIngredienti { get; set; }
        public DbSet<Categoria> Categorie { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=pizzeria-db;Integrated Security=True");
        }
    }
}
