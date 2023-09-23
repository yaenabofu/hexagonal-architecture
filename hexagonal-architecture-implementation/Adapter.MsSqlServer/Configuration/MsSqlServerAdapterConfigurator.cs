using Adapter.MsSqlServer.Contexts;
using Domain.Ports.Driven;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Adapter.MsSqlServer.Migrations;

namespace Adapter.MsSqlServer.Configuration
{
    public class MsSqlServerAdapterConfigurator
    {
        public static WebApplicationBuilder AddDatabase(WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("MsSqlServerAdapterConfiguration.json", false, true);

            builder.Services.Configure<MsSqlServerAdapterConfiguration>(builder.Configuration.GetSection(nameof(MsSqlServerAdapterConfiguration)));
            builder.Services.AddSingleton(c => c.GetRequiredService<IOptions<MsSqlServerAdapterConfiguration>>().Value);

            string connectionString = nameof(MsSqlServerAdapterConfiguration) + ":" + nameof(MsSqlServerAdapterConfiguration.ConnectionString);

            builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(builder.Configuration.GetValue<string>(connectionString)));

            builder.Services.AddHostedService<MigratorHostedService>();
            builder.Services.AddScoped<IUserRepository, MsSqlServerAdapter>();

            return builder;
        }
    }
}
