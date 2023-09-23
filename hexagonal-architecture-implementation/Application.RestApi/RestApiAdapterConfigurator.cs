using Adapter.HMACSHA256_PasswordHasher.Configuration;
using Adapter.MsSqlServer;
using Adapter.MsSqlServer.Configuration;
using Domain.Configuration;
using Domain.Mappers;
using Domain.Ports.Driven;
using Domain.Ports.Driving;
using Domain.UseCases;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Application.RestApi
{
    public static class RestApiAdapterConfigurator
    {
        public static WebApplicationBuilder Configurate(WebApplicationBuilder builder)
        {
            DomainConfiguratior.Configurate(builder);

            MsSqlServerAdapterConfigurator.AddDatabase(builder);

            HMACSHA256PasswordHasherConfigurator.AddPasswordHasher(builder);

            return builder;
        }
    }
}