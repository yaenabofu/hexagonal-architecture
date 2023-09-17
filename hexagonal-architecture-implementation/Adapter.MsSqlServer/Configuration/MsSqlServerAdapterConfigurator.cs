using Adapter.MsSqlServer.Contexts;
using Domain.Ports.Driven;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Adapter.MsSqlServer.Migrations;

namespace Adapter.MsSqlServer.Configuration
{
    public static class MsSqlServerAdapterConfigurator
    {
        public static WebApplicationBuilder AddDatabase(WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("msSqlServerAdapterConfiguration.json", false, true);
            builder.Services.Configure<MsSqlServerAdapterConfiguration>(builder.Configuration.GetSection(nameof(MsSqlServerAdapterConfiguration)));

            builder.Services.AddSingleton(c => c.GetRequiredService<IOptions<MsSqlServerAdapterConfiguration>>().Value);

            string connectionString = builder.Configuration.GetSection(nameof(MsSqlServerAdapterConfiguration.ConnectionString)).Value;
            builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddHostedService<MigratorHostedService>();
            builder.Services.AddScoped<IUserRepository, MsSqlServerAdapter>();

            return builder;
        }
    }
}
