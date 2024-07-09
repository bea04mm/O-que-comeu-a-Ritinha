using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using O_que_comeu_a_Ritinha.Models;

namespace O_que_comeu_a_Ritinha.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Ingredients> Ingredients { get; set; }
		public DbSet<IngredientsRecipes> IngredientsRecipes { get; set; }
		public DbSet<Recipes> Recipes { get; set; }
		public DbSet<RecipesTags> RecipesTags { get; set; }
		public DbSet<Favorites> RecipesUtilizadores { get; set; }
		public DbSet<Tags> Tags { get; set; }
        public DbSet<Utilizadores> Utilizadores { get; set; }
        public DbSet<Aboutus> Aboutus { get; set; }
		public DbSet<AboutusRecipes> AboutusRecipes { get; set; }
	}
}
