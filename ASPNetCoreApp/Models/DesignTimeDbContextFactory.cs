using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ASPNetCoreApp.Models
{
	public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<fnewsContext>
	{
		public fnewsContext CreateDbContext(string[] args)
		{
			IConfigurationRoot configuration = new
			ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
			var builder = new DbContextOptionsBuilder<fnewsContext>();
			var connectionString = configuration.GetConnectionString("DefaultConnection");
			builder.UseSqlServer(connectionString);
			return new fnewsContext(builder.Options);
		}
	}
}