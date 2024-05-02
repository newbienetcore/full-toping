using System.Reflection;
using AutoMapper.Configuration;

namespace Infrastructure.Mappings;

public static class AutoMapperExtensions
{
    public static MappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(
        this MappingExpression<TSource, TDestination> expression)
    {
        var flags = BindingFlags.Public | BindingFlags.Instance;
        var sourceType = typeof(TSource);
        var destinationProperties = typeof(TDestination).GetProperties(flags);

        foreach (var property in destinationProperties)
        {
            if (sourceType.GetProperty(property.Name, flags) is null)
                expression.ForMember(property.Name, opt => opt.Ignore());
        }
        return expression;
    }
    
}