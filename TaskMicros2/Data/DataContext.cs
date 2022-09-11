using Microsoft.EntityFrameworkCore;
using TaskMicros2.Models;

namespace TaskMicros2.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Categories> Categories { get; set; }
        public DbSet<Types> Types { get; set; }

        public DbSet<Users> Users { get; set; }

       public DbSet<Datas> Data { get; set; }



    }
}
