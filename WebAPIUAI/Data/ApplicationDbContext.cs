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
            modelBuilder.Entity<MateriasFacultades>()
                .HasKey(x => new { x.MateriaId, x.FacultadId });
            modelBuilder.Entity<MateriasProfesores>()
                .HasKey(x => new { x.MateriaId, x.ProfesorId });
            modelBuilder.Entity<MateriasSedes>()
                .HasKey(x => new { x.MateriaId, x.SedeId });

            SeedData(modelBuilder);

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

        private void SeedData(ModelBuilder modelBuilder)
        {
            var rolAdminId = Guid.NewGuid().ToString();
            var usuarioAdminId = Guid.NewGuid().ToString();

            var rolAdmin = new IdentityRole
            {
                Id = rolAdminId,
                Name = "Admin",
                NormalizedName = "ADMIN"
            };

            var passwordHasher = new PasswordHasher<IdentityUser>();

            var username = "gabriel@mail.com";

            var usuarioAdmin = new IdentityUser
            {
                Id = usuarioAdminId,
                UserName = username,
                NormalizedUserName = username.ToUpper(),
                Email = username,
                NormalizedEmail = username.ToUpper(),
                PasswordHash = passwordHasher.HashPassword(null, "123456")
            };

            // modelBuilder.Entity<IdentityRole>().HasData(rolAdmin);
            // modelBuilder.Entity<IdentityUser>().HasData(usuarioAdmin);
            // modelBuilder.Entity<IdentityUserClaim<string>>().HasData(new IdentityUserClaim<string>
            // {
            //     Id = 1,
            //     ClaimType = ClaimTypes.Role,
            //     UserId = usuarioAdminId,
            //     ClaimValue = "Admin"
            // });
        }

        public DbSet<Facultad> Facultades { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<Sede> Sedes { get; set; }
        public DbSet<MateriasFacultades> MateriasFacultades { get; set; }
        public DbSet<MateriasProfesores> MateriasProfesores { get; set; }
        public DbSet<MateriasSedes> MateriasSedes { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}