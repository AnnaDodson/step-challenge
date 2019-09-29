using System.Security.Claims;
using GraphQL.Authorization;

namespace Model.GraphQL
{
    public class GraphQLUserContext : IProvideClaimsPrincipal
    {
        public ClaimsPrincipal User { get; set; }
    }
}
