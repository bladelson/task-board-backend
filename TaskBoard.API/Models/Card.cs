using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace TaskBoard.API.Models
{
    public class Card
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        //todo color, priority, other stuff?
    }
}