﻿// Copyright (c) Christof Senn. All rights reserved. 

using Common.DataContract;
using Common.ServiceContract;
using Remote.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Server
{
    public class RemoteLinqDataService : IRemoteLinqDataService
    {
        public IEnumerable<Product> GetProducts(Query<Product> query)
            => DataSource.Products
                .ApplyQuery(query)
                .ToList();

        public IEnumerable<Order> GetOrders(Query<Order> query)
            => DataSource.Orders
                .ApplyQuery(query)
                .ToList();

        public IEnumerable<object> GetData(IQuery query)
            => (IEnumerable<object>)typeof(RemoteLinqDataService)
                .GetMethod(nameof(OpenTypeQuery), BindingFlags.Instance | BindingFlags.NonPublic)
                .MakeGenericMethod((Type)query.Type)
                .Invoke(this, new object[] { query });

        private IEnumerable<T> OpenTypeQuery<T>(IQuery query)
        {
            Query<T> genericQuery = Query<T>.CreateFromNonGeneric(query);
            return DataSource.Query<T>()
                .ApplyQuery(genericQuery)
                .ToList();
        }
    }
}
