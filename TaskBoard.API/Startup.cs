using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using TaskBoard.API.Models;
using TaskBoard.API.Services;
using TaskBoard.API.Services.Abstractions;

[assembly: FunctionsStartup(typeof(Taskboard.Startup))]

namespace Taskboard
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var ctx = builder.GetContext();
            var config = ctx.Configuration;
            BuildServices(builder.Services, config);
        }

        private void BuildServices(IServiceCollection services, IConfiguration config)
        {
            services.AddLogging();

            var mongoConnectionString = config.GetValue<string>("MongoConnectionString");
            var mongoClient = new MongoClient(mongoConnectionString);
            services.AddSingleton(typeof(IMongoClient), mongoClient);

            services.AddSingleton<IBoardService, BoardService>();
        }
    }
}