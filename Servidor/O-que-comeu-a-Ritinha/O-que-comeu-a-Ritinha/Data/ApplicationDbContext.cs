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
        //public DbSet<Blog> Blog { get; set; }
        //public DbSet<Books> Books { get; set; }
        //public DbSet<Comments> Comments { get; set; }
        //public DbSet<Favoriteorbuy> Favoriteorbuy { get; set; }
        public DbSet<Ingredients> Ingredients { get; set; }
        //public DbSet<Likes> Likes { get; set; }
        //public DbSet<Posts> Posts { get; set; }
        public DbSet<Recipes> Recipes { get; set; }
        //public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Tags> Tags { get; set; }
        //public DbSet<Utilizadores> Utilizadores { get; set; }
    }
}
