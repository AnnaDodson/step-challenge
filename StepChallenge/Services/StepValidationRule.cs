using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL;
using GraphQL.Language.AST;
using GraphQL.Types;
using GraphQL.Validation;

namespace StepChallenge.Validation
{
    public class StepValidationRule : IValidationRule
    {
        public INodeVisitor Validate(ValidationContext context)
        {
            return new EnterLeaveListener(_ =>
            {
            
                _.Match<Argument>(argAst =>
                {
                    var argDef = context.TypeInfo.GetArgument();
                    if (argDef == null) return;

                    var type = argDef.ResolvedType;
                    if (type.IsInputType())
                    {
                        var fields = ((type as NonNullGraphType)?.ResolvedType as IComplexGraphType)?.Fields;
                        if (fields != null)
                        {
							//let's look for fields that have a specific metadata
                            foreach (var fieldType in fields.Where(f => f.HasMetadata(nameof(StepValidationRule))))
                            {
                                //now it's time to get the value
                                //var value = context.Inputs.GetValue(argAst.Name, fieldType.Name);
                                var value = context.Inputs.GetValue(argAst.Name, fieldType.Name);
                                if (value != null)
								{
									if (!value.IsValidDate())
                                    {
                                        context.ReportError(new ValidationError(context.OriginalQuery
                                            , "Invalid Date"
                                            , "The given date is outside of the step challenge"
                                            , argAst
                                        ));
                                    }	
								}
                            }
                        }
                    }
                });
            });
        }
    }
	
	public static class GraphqlExtensions
	{
		/// <summary>
        /// A method that gets a value from a given input object with provided argument and field type name to match with.
        /// </summary>
        /// <param name="input">Provide an Inputs type.</param>
        /// <param name="argumentName">Provide name of the argument.</param>
        /// <param name="fieldTypeName">Provide field type name.</param>
        /// <returns>Returns a value from dictionary.</returns>
        public static string GetValue(this Inputs input, string argumentName, string fieldTypeName)
        {
            if (input.ContainsKey(argumentName) )
            {
                var model = (Dictionary<string, object>)input[argumentName];
                if (model != null && model.ContainsKey(fieldTypeName))
                {
                    return model[fieldTypeName]?.ToString();
                }
            }

            return null;
        }
	}
	
	public static class StringExtensions
    {
        public static bool IsValidDate(this string stepDateStr)
        {
            var stepDate = DateTimeOffset.Parse(stepDateStr);
            var challengeStartDate = new DateTime(2019,09,16);
            var endOfChallengeDate = new DateTime(2019, 12, 05);
            
            var validDate =  stepDate >= challengeStartDate && stepDate < endOfChallengeDate;

            if (!validDate)
            {
                return false;
            }

            return true;
        }
    }
}

