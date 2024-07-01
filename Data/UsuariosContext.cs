using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TO_DO___API.Models;

namespace TO_DO___API.Data;

public class UsuariosContext : IdentityDbContext<Usuario>
{
    public UsuariosContext(DbContextOptions<UsuariosContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração para todas as tabelas do Identity

        // Tabela ApplicationUser (AspNetUsers)
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("aspnetusers"); // Nome da tabela 'AspNetUsers' como 'aspnetusers'
            entity.Property(e => e.Id).HasMaxLength(110);
            entity.Property(e => e.Email).HasMaxLength(127);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(127);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(127);
            entity.Property(e => e.UserName).HasMaxLength(127);
        });

        // Tabela IdentityRole (AspNetRoles)
        modelBuilder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable("aspnetroles"); // Nome da tabela 'AspNetRoles' como 'aspnetroles'
            entity.Property(e => e.Id).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(127);
            entity.Property(e => e.NormalizedName).HasMaxLength(127);
        });

        // Tabela IdentityUserClaim<string> (AspNetUserClaims)
        modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable("aspnetuserclaims"); // Nome da tabela 'AspNetUserClaims' como 'aspnetuserclaims'
        });

        // Tabela IdentityUserRole<string> (AspNetUserRoles)
        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("aspnetuserroles"); // Nome da tabela 'AspNetUserRoles' como 'aspnetuserroles'
            entity.HasKey(ur => new { ur.UserId, ur.RoleId });
        });

        // Tabela IdentityUserLogin<string> (AspNetUserLogins)
        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable("aspnetuserlogins"); // Nome da tabela 'AspNetUserLogins' como 'aspnetuserlogins'
            entity.HasKey(ul => new { ul.LoginProvider, ul.ProviderKey });
        });

        // Tabela IdentityUserToken<string> (AspNetUserTokens)
        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable("aspnetusertokens"); // Nome da tabela 'AspNetUserTokens' como 'aspnetusertokens'
            entity.HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });
        });

        // Tabela IdentityRoleClaim<string> (AspNetRoleClaims)
        modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable("aspnetroleclaims"); // Nome da tabela 'AspNetRoleClaims' como 'aspnetroleclaims'
        });
    }
}
