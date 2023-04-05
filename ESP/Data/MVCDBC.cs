using ESP.Models.Domains;
using Microsoft.EntityFrameworkCore;

namespace ESP.Data
{
    //Klasa odpowiedzialna za łączność z bazą danych (most MVC - DB)
    public class MVCDBC : DbContext
    {
        public MVCDBC(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Question> Question { get; set; } //nazwa tabeli
    }
}
