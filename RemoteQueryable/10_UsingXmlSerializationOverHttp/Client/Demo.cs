﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

namespace Client
{
    using Remote.Linq;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class Demo
    {
        private readonly string _url;

        public Demo(string url)
        {
            _url = url;
        }

        public async Task RunAsync()
        {
            var repo = new RemoteRepository(_url);

            Console.WriteLine("\nGET ALL PRODUCTS:");
            foreach (var i in await repo.Products.ToListAsync())
            {
                Console.WriteLine($"  {i.Id} | {i.Name} | {i.Price:C}");
            }


            Console.WriteLine("\nCROSS JOIN:");
            Func<object, string> suffix = (x) => x + "ending";
            var crossJoinQuery =
                from c in repo.ProductCategories
                from p in repo.Products
                select new { Category = "#" + c.Name + suffix("-"), p.Name };
            var crossJoinResult = await crossJoinQuery.ToListAsync();
            foreach (var i in crossJoinResult)
            {
                Console.WriteLine($"  {i}");
            }


            Console.WriteLine("\nINNER JOIN:");
            var innerJoinQuery =
                from c in repo.ProductCategories
                join p in repo.Products on c.Id equals p.ProductCategoryId
                select new { c.Name, P = new { p.Price }, X = new { Y = string.Concat(c.Name, "-", p.Name) } };
            var innerJoinResult = await innerJoinQuery.ToListAsync();
            foreach (var i in innerJoinResult)
            {
                Console.WriteLine($"  {i}");
            }


            Console.WriteLine("\nSELECT IDs:");
            var productIdsQuery =
                from p in repo.Products
                orderby p.Price descending
                select p.Id;
            var productIds = await productIdsQuery.ToListAsync();
            foreach (var id in productIdsQuery)
            {
                Console.WriteLine($"  {id}");
            }


            Console.WriteLine("\nCOUNT:");
            var productsQuery =
                from p in repo.Products
                select p;
            Console.WriteLine($"  Count = {await productsQuery.CountAsync()}");


            Console.WriteLine("\nTOTAL AMOUNT BY CATEGORY:");
            var totalAmountByCategoryQuery =
                from c in repo.ProductCategories
                join p in repo.Products
                    on c.Id equals p.ProductCategoryId
                join i in repo.OrderItems
                    on p.Id equals i.ProductId
                group new { c, p, i } by c.Name into g
                select new
                {
                    Category = g.Key,
                    Amount = g.Sum(x => x.i.Quantity * x.p.Price),
                };

            var totalAmountByCategroyResult = await totalAmountByCategoryQuery.ToDictionaryAsync(x => x.Category);
            foreach (var i in totalAmountByCategroyResult)
            {
                Console.WriteLine($"  {i}");
            }


            Console.WriteLine("\nINVALID OPERATION:");
            try
            {
                var first = await totalAmountByCategoryQuery.FirstAsync(x => false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  {ex.Message}");
            }
        }
    }
}
