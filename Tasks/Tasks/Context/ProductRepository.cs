using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tasks.Models;

namespace Tasks.Context
{

    public class ProductRepository : IDisposable
    {
        public ProductRepository()
        {
            prContext = new ProductsEntities();
        }

        private ProductsEntities prContext;

        public ProductsEntities PRContext
        {
            get
            {
                return prContext;
            }
        }
      
       
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (prContext != null)
                        prContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public async System.Threading.Tasks.Task Add(List<Product> products)
        {
            if (PRContext.Products.Any())
                PRContext.Database.ExecuteSqlCommand("DELETE FROM Products");

            PRContext.Configuration.AutoDetectChangesEnabled=false;

            PRContext.Products.AddRange(products);
            await PRContext.SaveChangesAsync();
        }
    }
}