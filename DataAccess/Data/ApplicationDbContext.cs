using Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DataAccess.Data
{

    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

            //  We are "using" the EntityFramework(which supports several data sources like Oracle, MS SQL Server, MySQL) to "map" our local models to physical database tables

            //We register the  DBContextOptions as a "service" in the "build" pipeline within the Program.cs file 
        }

    }
}
