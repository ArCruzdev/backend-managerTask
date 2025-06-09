using Application.Common.Behaviors;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Registrar todos los handlers de MediatR desde este assembly
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // Registrar AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Registrar validadores con FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        //para ejecutar mis validadores antes de los handler
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
