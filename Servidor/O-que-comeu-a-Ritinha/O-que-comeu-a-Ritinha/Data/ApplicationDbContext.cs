using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using O_que_comeu_a_Ritinha.Migrations;
using O_que_comeu_a_Ritinha.Models;

namespace O_que_comeu_a_Ritinha.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			/* Esta instrução importa tudo o que está pre-definido
			 * na super classe
			 */
			base.OnModelCreating(builder);
			/* Adição de dados à Base de Dados
		
			 * Atribuir valores às ROLES
			 */
			builder.Entity<IdentityRole>().HasData(
				new IdentityRole { Id = "use", Name = "Utilizador", NormalizedName = "UTILIZADOR" },
				new IdentityRole { Id = "adm", Name = "Administrativo", NormalizedName = "ADMINISTRATIVO" }
				);
		}

		public DbSet<Ingredients> Ingredients { get; set; }
        public DbSet<IngredientsRecipes> IngredientsRecipes { get; set; }
        public DbSet<Recipes> Recipes { get; set; }
        public DbSet<RecipesTags> RecipesTags { get; set; }
        public DbSet<Favorites> Favorites { get; set; }
        public DbSet<Tags> Tags { get; set; }
        public DbSet<Utilizadores> Utilizadores { get; set; }
        public DbSet<Aboutus> Aboutus { get; set; }
        public DbSet<AboutusRecipes> AboutusRecipes { get; set; }
    }
}
