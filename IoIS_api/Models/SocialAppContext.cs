using Microsoft.EntityFrameworkCore;
using SocialApp.API.WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialApp.API.WebAPI.Models
{
    /// <summary>
    /// The context is basically our db schema defined in C# code.
    /// It contains a reference for each database table.
    /// </summary>
    public class SocialAppContext : DbContext
    {
        // Init context with the super classes options
        public SocialAppContext(DbContextOptions<SocialAppContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasOne(e => e.CreatedBy)
                .WithMany(u => u.CreatedEvents)
                .HasForeignKey(e => e.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.Users)
                .WithMany(u => u.EventsParticipating)
                .UsingEntity(eu => eu.ToTable("UsersEvents"));

            modelBuilder.Entity<Group>()
                .HasOne(g => g.CreatedBy)
                .WithMany(u => u.CreatedGroups)
                .HasForeignKey(g => g.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.CreatedBy)
                .WithMany(u => u.CreatedPosts)
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friend>()
                .HasKey(f => new { f.RequesterId, f.AddresseeId });

            modelBuilder.Entity<UserGroup>()
                .HasKey(f => new { f.UserId, f.GroupId });

            modelBuilder.Entity<Friend>()
              .HasOne(f => f.Requester)
              .WithMany(u => u.Requesters)
              .HasForeignKey(f => f.RequesterId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.Addressee)
                .WithMany(u => u.Addressees)
                .HasForeignKey(f => f.AddresseeId);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.FromUser)
                .WithMany(u => u.MessageFrom)
                .HasForeignKey(m => m.FromUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.ToUser)
                .WithMany(u => u.MessageTo)
                .HasForeignKey(m => m.ToUserId);

            base.OnModelCreating(modelBuilder);
        }

        // Here go the entities the following way
        public DbSet<Event> Events { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
    }
}
