using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ASPNetCoreApp.Models
{
	public partial class fnewsContext : IdentityDbContext<User>
	{
		#region Constructor
		public fnewsContext(DbContextOptions<fnewsContext>
		options)
		: base(options)
		{ }
		#endregion
		public virtual DbSet<Topic> Topic { get; set; }
		public virtual DbSet<Novelty> Novelty { get; set; }
		public virtual DbSet<Like> Likes { get; set; }
		public virtual DbSet<User> Users { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Topic>(entity =>
			{
				entity.Property(e => e.Url);
			});
			modelBuilder.Entity<Topic>(entity =>
			{
				entity.Property(e => e.Name).IsRequired();
			});
			modelBuilder.Entity<Novelty>(entity =>
			{
				entity.HasOne(d => d.Topic).WithMany(p => p.Novelty).HasForeignKey(d => d.TopicId).OnDelete(DeleteBehavior.Cascade);
			});
			modelBuilder.Entity<Novelty>()
				.HasOne(e => e.User)
				.WithMany(e => e.Novelties)
				.HasForeignKey(e => e.UserId);

			modelBuilder.Entity<Like>()
				.HasOne(e => e.User)
				.WithMany(e => e.Likes)
				.HasForeignKey(e => e.UserId);

			modelBuilder.Entity<Like>()
			   .HasOne(e => e.Novelty)
			   .WithMany(e => e.Likes)
			   .HasForeignKey(e => e.NoveltyId)
			   .IsRequired(false)
			   .OnDelete(DeleteBehavior.Cascade);
		}
	}
}
