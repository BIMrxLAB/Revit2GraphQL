using GraphQL;
using GraphQL.Language.AST;
using System.Collections.Generic;
using System.Linq;

namespace RevitGraphQLResolver.GraphQL
{
    public static class GraphQlHelpers
    {
        public static List<string> GetArgumentStrings(Field aField, string name)
        {
            var nameFilterArgument = aField.Arguments?.FirstOrDefault(x => x.Name == name);
            List<string> nameFilterStrings = new List<string>();
            if (nameFilterArgument != null)
            {
                nameFilterStrings = (nameFilterArgument.Value.Value as List<object>).Select(x => x.ToString()).ToList();
            }
            return nameFilterStrings;
        }
        public static double GetArgumentDouble(Field aField, string name)
        {
            var nameFilterArgument = aField.Arguments?.FirstOrDefault(x => x.Name == name);
            double argumentValueDouble = 0;
            if (nameFilterArgument != null)
            {
                argumentValueDouble = double.Parse(nameFilterArgument.Value.Value.ToString());
            }
            return argumentValueDouble;
        }

        //public static Field GetFieldFromSelectionSet(Field aField, string name)
        //{
        //    return aField.SelectionSet.Children.FirstOrDefault(x => (x as Field).Name == name) as Field;
        //}
        //public static Field GetFieldFromContext(IResolveFieldContext context, string name)
        //{
        //    return context.SubFields.FirstOrDefault(x => x.Key == name).Value as Field;
        //}

        public static Field GetFieldFromFieldOrContext(object aFieldOrContext, string name)
        {
            //https://systemoutofmemory.com/blogs/the-programmer-blog/c-sharp-switch-on-type
            switch (aFieldOrContext)
            {
                case Field aField:
                    return aField.SelectionSet.Children.FirstOrDefault(x => (x as Field).Name == name) as Field;
                case IResolveFieldContext aContext:
                    return aContext.SubFields.FirstOrDefault(x => x.Key == name).Value as Field;
                default:
                    return null;
            }
        }
    }
}
