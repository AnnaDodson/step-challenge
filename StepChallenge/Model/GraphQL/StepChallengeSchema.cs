using GraphQL;
using GraphQL.Types;
using StepChallenge.Mutation;
using StepChallenge.Query;

namespace Model.GraphQL
{
    public class StepSchema : Schema
    {
        public StepSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<StepChallengeQuery>();
            Mutation = resolver.Resolve<StepChallengeMutation>();
        }
        
    }
}
