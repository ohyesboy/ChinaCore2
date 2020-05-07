using ChinaCore.Controllers;
using Microsoft.EntityFrameworkCore;
namespace ChinaCore
{
	public class ChinaContext : DbContext
	{
		public ChinaContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Order> Orders { get; set; }

	}
}