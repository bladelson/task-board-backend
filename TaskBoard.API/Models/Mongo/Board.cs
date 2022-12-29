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

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; }

        [BsonElement("cards")]
        public List<Card> Cards { get; set; } = new List<Card>();
    }
}