using Domain.DTOs.Requests;
using Domain.DTOs.Requests.Validators;
using Domain.Mappers;
using Domain.Ports.Driving;
using Domain.UseCases;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Configuration
{
    public static class DomainConfiguratior
    {
        public static WebApplicationBuilder Configurate(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IUserMapper, UserMapper>();

            builder.Services.AddScoped<IValidator<AddUserDTO>, AddUserValidator>();
            builder.Services.AddScoped<IValidator<DeleteUserByIdDTO>, DeleteUserByIdValidator>();
            builder.Services.AddScoped<IValidator<DeleteUserByLoginDTO>, DeleteUserByLoginValidator>();
            builder.Services.AddScoped<IValidator<GetUserByIdDTO>, GetUserByIdValidator>();
            builder.Services.AddScoped<IValidator<GetUserByLoginDTO>, GetUserByLoginValidator>();

            return builder;
        }
    }
}
