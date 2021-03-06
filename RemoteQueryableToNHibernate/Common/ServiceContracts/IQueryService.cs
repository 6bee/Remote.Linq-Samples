﻿// Copyright (c) Christof Senn. All rights reserved. 

namespace Common.ServiceContracts
{
    using Aqua.Dynamic;
    using Remote.Linq.Expressions;
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract]
    public interface IQueryService
    {
        [OperationContract]
        IEnumerable<DynamicObject> ExecuteQuery(Expression queryExpression);
    }
}
