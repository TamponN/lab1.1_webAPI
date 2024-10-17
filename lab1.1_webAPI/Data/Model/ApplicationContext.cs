using Microsoft.EntityFrameworkCore;

/*
Сори, но я это сразу не запомню:

Для миграции: dotnet ef migrations add init --project ../Data --startup-project . --output-dir ../Data/Migrations 
Для отмены миграции: dotnet ef migrations remove init --project ../Data --startup-project . --output-dir ../Data/Migrations 
Для update: dotnet ef database update --project ../Data --startup-project .

*/

namespace Data.Model
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions dbContextOptions) : base(dbContextOptions) // Наследуемся от базового конструктора
        {   
        }

        public DbSet<V008Entity> V008Entities { get; set; }

    }
}