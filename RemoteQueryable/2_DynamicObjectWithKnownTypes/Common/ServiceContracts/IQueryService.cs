﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

namespace Common.ServiceContracts
{
    using Aqua.Dynamic;
    using Model;
    using Remote.Linq.Expressions;
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract]
    public interface IQueryService
    {
        [OperationContract]
        [ServiceKnownType(typeof(OrderItem))]
        [ServiceKnownType(typeof(Product))]
        [ServiceKnownType(typeof(ProductCategory))]
        IEnumerable<DynamicObject> ExecuteQuery(Expression queryExpression);
    }
}
