﻿using AutoMapper;
using System.Reflection; 

namespace Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);

            var methodInfo = type.GetMethod("Mapping")
                ?? type.GetInterface("IMapFrom`1")?.GetMethod("Mapping"); // Fallback para interfaces explícitas

            if (methodInfo != null)
            {
                methodInfo.Invoke(instance, new object[] { this });
            }
            else
            {

                // Here, 'type' is the DTO (destination) and 'sourceType' is the entity (source).
                // The source type is obtained from the IMapFrom<TSource> interface
                var iMapFromType = type.GetInterfaces().FirstOrDefault(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>));

                if (iMapFromType != null)
                {
                    var sourceType = iMapFromType.GetGenericArguments()[0];
                    CreateMap(sourceType, type); // Mapping from SourceEntity -> Dto
                }
            }
        }
    }
}
