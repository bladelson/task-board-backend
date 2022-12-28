using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using TaskBoard.API.Models;

namespace TaskBoard.API.Http
{
    public class Cards
    {
        private readonly MongoClient _mongo;

        public Cards(MongoClient mongo)
        {
            _mongo = mongo;
        }

        [FunctionName("createCard")]
        public async Task<IActionResult> Post([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "cards")] HttpRequest request, ILogger log)
        {
            using var streamReader = new StreamReader(request.Body);
            string body = await streamReader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(body))
                return new BadRequestObjectResult("Empty request body");

            Card card = null;
            try
            {
                card = JsonSerializer.Deserialize<Card>(body);
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"failed to deserialize request body to card");
                return new BadRequestObjectResult("Invalid request body");
            }

            var db = _mongo.GetDatabase("cardDB");
            var coll = db.GetCollection<Card>("cards");
            await coll.InsertOneAsync(card);

            return new OkObjectResult(new { Id = card.Id });
        }
    }
}