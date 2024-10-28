using Microsoft.EntityFrameworkCore;
using Data.Model;

/*
Сори, но я это сразу не запомню:

Для миграции: dotnet ef migrations add init --project ../Data --startup-project . --output-dir ../Data/Migrations 
Для отмены миграции: dotnet ef migrations remove init --project ../Data --startup-project . --output-dir ../Data/Migrations 
Для update: dotnet ef database update --project ../Data --startup-project .

*/

namespace Data.Contexts
{
    public class TFOMSReesterContext : DbContext
    {
        public TFOMSReesterContext (DbContextOptions dbContextOptions) : base(dbContextOptions) // Наследуемся от базового конструктора
        {   
        }

    }
}