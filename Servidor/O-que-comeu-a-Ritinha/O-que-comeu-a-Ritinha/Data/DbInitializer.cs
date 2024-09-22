using EllipticCurve;
using Microsoft.AspNetCore.Identity;
using O_que_comeu_a_Ritinha.Models;

namespace O_que_comeu_a_Ritinha.Data
{
	internal class DbInitializer
	{
		internal static async void Initialize(ApplicationDbContext dbContext)
		{
			ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
			dbContext.Database.EnsureCreated();

			// var auxiliar
			bool haAdicao = false;


			// Se não houver Utilizadores Identity, cria-os
			var users = Array.Empty<IdentityUser>();
			//a hasher to hash the password before seeding the user to the db
			var hasher = new PasswordHasher<IdentityUser>();

			if (!dbContext.Users.Any())
			{
				users = [
					new IdentityUser{UserName="email.seis@mail.pt", NormalizedUserName="EMAIL.SEIS@MAIL.PT",
						Email="email.seis@mail.pt",NormalizedEmail="EMAIL.SEIS@MAIL.PT", EmailConfirmed=true,
						SecurityStamp="5ZPZEF6SBW7IU4M344XNLT4NN5RO4GRU", ConcurrencyStamp="c86d8254-dd50-44be-8561-d2d44d4bbb2f",
						PasswordHash=hasher.HashPassword(null,"Aa0_aa") },
					new IdentityUser{UserName="email.sete@mail.pt", NormalizedUserName="EMAIL.SETE@MAIL.PT",
						Email="email.sete@mail.pt",NormalizedEmail="EMAIL.SETE@MAIL.PT", EmailConfirmed=true,
						SecurityStamp="TW49PF6SBW7IU4M344XNLT4NN5RO4GRU", ConcurrencyStamp="d8254c86-dd50-44be-8561-d2d44d4bbb2f",
						PasswordHash=hasher.HashPassword(null,"Aa0_aa") }
				];
				await dbContext.Users.AddRangeAsync(users);
				await dbContext.Users.AddRangeAsync(users);
				haAdicao = true;
			}


			// Se não houver Utilizadores, cria-os
			var utilizador = Array.Empty<Utilizadores>();
			if (!dbContext.Utilizadores.Any())
			{
				utilizador = [
					new Utilizadores { Name="João Mendes", Birthday=DateOnly.Parse("1970-04-10"), Phone="919876543" , UserId=users[0].Id },
					new Utilizadores { Name="Maria Sousa", Birthday=DateOnly.Parse("1988-09-12"), Phone="918076543" , UserId=users[1].Id }
				];
				await dbContext.Utilizadores.AddRangeAsync(utilizador);
				haAdicao = true;
			}

			try
			{
				if (haAdicao)
				{
					// tornar persistentes os dados
					dbContext.SaveChanges();
				}
			}
			catch (Exception ex)
			{

				throw;
			}



		}
	}
}