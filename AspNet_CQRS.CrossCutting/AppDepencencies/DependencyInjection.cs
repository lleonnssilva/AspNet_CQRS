using AspNet_CQRS.Application.Members.Messages;
using AspNet_CQRS.Application.Members.Validations;
using AspNet_CQRS.Domain.Astractions;
using AspNet_CQRS.Infrastructure.Context;
using AspNet_CQRS.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Reflection;
namespace AspNet_CQRS.CrossCutting.AppDepencencies
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructre(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            var sqlConnection = configuration.GetConnectionString("DefaultConnection");
            var redisConnection = configuration.GetConnectionString("RedisConnection");
            var rmQConnection = configuration.GetConnectionString("RmQConnection");

            services.AddDbContext<AppDbContext>(options =>
                         options.UseSqlServer(sqlConnection, null));

            services.AddSingleton<IDbConnection>(provider =>
            {
                var connection = new SqlConnection(sqlConnection);
                connection.Open();
                return connection;
            });

            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMemberDapperRepository, MemberDapperRepository>();

            var myhandlers = AppDomain.CurrentDomain.Load("AspNet_CQRS.Application");
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(myhandlers);
                cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            });
            services.AddValidatorsFromAssembly(Assembly.Load("AspNet_CQRS.Application"));

            services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnection;
            });

            return services;
        }

    }
}
