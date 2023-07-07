using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPIUAI.Models;

namespace WebAPIUAI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MateriasFacultades>()
                .HasKey(x => new { x.MateriaId, x.FacultadId });
            modelBuilder.Entity<MateriasProfesores>()
                .HasKey(x => new { x.MateriaId, x.ProfesorId });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Facultad> Facultades { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Materia> Materias { get; set; }

        public DbSet<MateriasFacultades> MateriasFacultades { get; set; }
        public DbSet<MateriasProfesores> MateriasProfesores { get; set; }
    }
}