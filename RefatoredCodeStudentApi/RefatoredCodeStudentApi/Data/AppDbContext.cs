using Microsoft.EntityFrameworkCore;
using RefatoredCodeStudentApi.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace RefatoredCodeStudentApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<TrainingTrack> TrainingTracks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enrollment>()
                .HasOne(d => d.Student)
                .WithMany(d => d.Enrollments)
                .HasForeignKey(d => d.StudentId)
                .IsRequired();


            modelBuilder.Entity<Enrollment>()
                .HasOne(d => d.TrainingTrack)
                .WithMany(d => d.Enrollments)
                .HasForeignKey(d => d.TrainingTrackId)
                .IsRequired();

            modelBuilder.Entity<Payment>()
                .HasOne(d => d.Enrollment)
                .WithMany(d => d.Payments)
                .HasForeignKey(d => d.EnrollmentId)
                .IsRequired();

            modelBuilder.Entity<Student>()
                .HasIndex(d => d.Email)
                .IsUnique();


            modelBuilder.Entity<TrainingTrack>()
                .HasIndex(d => d.Code)
                .IsUnique();

        }
    }
}

