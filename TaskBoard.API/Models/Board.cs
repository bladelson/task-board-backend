using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace TaskBoard.API.Models
{
    public class Board
    {
        [BsonId]
        public Guid Id { get; set; }

        [JsonPropertyName("title")]
        [BsonElement("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        [BsonElement("description")]
        public string Description { get; set; }

        [JsonPropertyName("isDeleted")]
        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; }
    }
}