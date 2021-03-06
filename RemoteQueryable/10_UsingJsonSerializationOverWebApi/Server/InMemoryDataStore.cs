﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

namespace Server
{
    using Common.Model;
    using System.Collections.Generic;

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
                new Product { Id = 10, Name = "Apple", Price = 1.0, ProductCategoryId = 1 },
                new Product { Id = 11, Name = "Pear", Price = 2.0, ProductCategoryId = 1 },
                new Product { Id = 12, Name = "Pineapple", Price = 3.0, ProductCategoryId = 1 },
                new Product { Id = 13, Name = "Car", Price = 33999.0, ProductCategoryId = 2 },
                new Product { Id = 14, Name = "Bicycle", Price = 150.0, ProductCategoryId = 2 },
            };

            _orderItems = new[]
            {
                new OrderItem { Id = 100, ProductId = 10, Quantity = 2 },
                new OrderItem { Id = 101, ProductId = 11, Quantity = 3 },
                new OrderItem { Id = 102, ProductId = 14, Quantity = 4 },
            };
        }

        public static InMemoryDataStore Instance { get; } = new InMemoryDataStore();

        public IEnumerable<ProductCategory> ProductCategories => _productCategories;

        public IEnumerable<Product> Products => _products;

        public IEnumerable<OrderItem> OrderItems => _orderItems;
    }
}
