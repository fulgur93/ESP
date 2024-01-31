using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel;

namespace ESP.Areas.Identity.Data;
//Kontekst do bazy danych (połączenie)
public class ForumDBUsers : IdentityDbContext<ForumUser>
{
    public ForumDBUsers(DbContextOptions<ForumDBUsers> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.ApplyConfiguration(new ForumUserEntityConfiguration());
    }
}
//Klasa fdziedzicząca z użttkownika
public class ForumUserEntityConfiguration : IEntityTypeConfiguration<ForumUser>
{
    public void Configure(EntityTypeBuilder<ForumUser> builder)
    {
        builder.Property(u => u.FirstName).HasMaxLength(40);
        builder.Property(u => u.LastName).HasMaxLength(40);
    }
}