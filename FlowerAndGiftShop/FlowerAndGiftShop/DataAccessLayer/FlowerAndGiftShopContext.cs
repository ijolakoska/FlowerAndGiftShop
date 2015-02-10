using FlowerAndGiftShop.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FlowerAndGiftShop.DataAccessLayer
{
    public class FlowerAndGiftShopContext: DbContext
    {

        public FlowerAndGiftShopContext(): base("FlowerAndGiftShopContext")
        {
        }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<Customer> Customer { get; set; }        
        public DbSet<Shop> Shop { get; set; }       
        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Flower> Flower { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<ShopCart> ShopCart { get; set; }
        public DbSet<OrderPackages> OrderPackages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}