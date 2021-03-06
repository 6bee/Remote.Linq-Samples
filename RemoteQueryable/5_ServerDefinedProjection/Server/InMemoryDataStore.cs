﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

namespace Server
{
    using Common.Model;
    using System.Linq;

    public sealed class InMemoryDataStore
    {
        private readonly ProductCategory[] _productCategories;
        private readonly Product[] _products;
        private readonly OrderItem[] _orderItems;

        private InMemoryDataStore()
        {
            _productCategories = new[]
            {
                new ProductCategory { Id = 1, Name = "Fruits" },
                new ProductCategory { Id = 2, Name = "Vehicles" },
            };

            _products = new[]
            {
                new Product { Id = 10, Name = "Apple", Price = 1m, ProductCategoryId = 1 },
                new Product { Id = 11, Name = "Pear", Price = 2m, ProductCategoryId = 1 },
                new Product { Id = 12, Name = "Pineapple", Price = 3m, ProductCategoryId = 1 },
                new Product { Id = 13, Name = "Car", Price = 33999m, ProductCategoryId = 2 },
                new Product { Id = 14, Name = "Bicycle", Price = 150m, ProductCategoryId = 2 },
            };

            _orderItems = new[]
            {
                new OrderItem { Id = 100, ProductId = 10, Quantity = 2 },
                new OrderItem { Id = 101, ProductId = 11, Quantity = 3 },
                new OrderItem { Id = 102, ProductId = 14, Quantity = 4 },
            };
        }

        public static InMemoryDataStore Instance { get; } = new InMemoryDataStore();

        public IQueryable<ProductCategory> ProductCategories => _productCategories.AsQueryable();

        public IQueryable<Product> Products => _products.AsQueryable();

        public IQueryable<OrderItem> OrderItems => _orderItems.AsQueryable();
    }
}
