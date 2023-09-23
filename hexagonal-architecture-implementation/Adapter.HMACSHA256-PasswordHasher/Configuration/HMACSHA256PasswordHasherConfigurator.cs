using Domain.Ports.Driven;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace Adapter.HMACSHA256_PasswordHasher.Configuration
{
    public static class HMACSHA256PasswordHasherConfigurator
    {
        public static WebApplicationBuilder AddPasswordHasher(WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("HMACSHA256PasswordHasherConfiguration.json", false, true);
            builder.Services.Configure<HMACSHA256PasswordHasherConfiguration>(
                builder.Configuration.GetSection(nameof(HMACSHA256PasswordHasherConfiguration)));

            builder.Services.AddSingleton(c => c.GetRequiredService<IOptions<HMACSHA256PasswordHasherConfiguration>>().Value);

            builder.Services.AddScoped<IPasswordHasher, HMACSHA256PasswordHasher>();

            return builder;
        }
    }
}
