﻿// Copyright (c) Christof Senn. All rights reserved. 

namespace Server
{
    using Aqua.Dynamic;
    using Common.ServiceContracts;
    using Remote.Linq.EntityFrameworkCore;
    using Remote.Linq.Expressions;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class QueryService : IQueryService
    {
        public async Task<IEnumerable<DynamicObject>> ExecuteQueryAsync(Expression queryExpression)
        {
            using var efContext = new EFContext();
            return await queryExpression.ExecuteWithEntityFrameworkCoreAsync(efContext);
        }
    }
}
