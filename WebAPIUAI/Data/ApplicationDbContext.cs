using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPIUAI.Models;

namespace WebAPIUAI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<MateriasFacultades>()
            //     .HasKey(x => new { x.MateriaId, x.FacultadId });
            // modelBuilder.Entity<MateriasProfesores>()
            //     .HasKey(x => new { x.MateriaId, x.ProfesorId });
            // modelBuilder.Entity<MateriasSedes>()
            //     .HasKey(x => new { x.MateriaId, x.SedeId });


            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>(b =>
            {
                b.ToTable("Usuarios");
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.ToTable("UsuarioClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.ToTable("UsuarioLogins");
            });

            modelBuilder.Entity<IdentityUserToken<string>>(b =>
            {
                b.ToTable("UsuarioTokens");
            });

            modelBuilder.Entity<IdentityRole>(b =>
            {
                b.ToTable("Roles");
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.ToTable("RolClaims");
            });

            modelBuilder.Entity<IdentityUserRole<string>>(b =>
            {
                b.ToTable("UsuarioRoles");
            });
        }
        public DbSet<Facultad> Facultades { get; set; }
        public DbSet<Carrera> Carreras { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}