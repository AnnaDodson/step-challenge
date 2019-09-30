using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.Validation;
using Microsoft.AspNetCore.Http;

namespace Model.GraphQL
{
    public class GraphQLSettings
    {
        public Func<HttpContext, Task<object>> BuildUserContext { get; set; }
        public object Root { get; set; }
        public List<IValidationRule> ValidationRules { get; } = new List<IValidationRule>();
    }
}