using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using TaskBoard.API.Models;
using TaskBoard.API.Models.DTO;
using TaskBoard.API.Services.Abstractions;

namespace TaskBoard.API.Http
{
    public class CardFunctions
    {
        private readonly IBoardService _boards;

        public CardFunctions(IBoardService boards)
        {
            _boards = boards;
        }

        [FunctionName("createCard")]
        public async Task<IActionResult> CreateCardAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "boards/{boardId:Guid}/cards")] HttpRequest request, Guid boardId, ILogger log)
        {
            //verify real board
            var board = await _boards.GetBoardByIdAsync(boardId);
            if (board == null)
                return new NotFoundObjectResult("board not found");

            var body = await request.GetBodyAsync();
            CardDTO newCard;
            try
            {
                newCard = JsonSerializer.Deserialize<CardDTO>(body);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "failed to deserialize card request body");
                return new BadRequestObjectResult("invalid request body");
            }

            var mongoCard = new Card()
            {
                Id = Guid.NewGuid(),
                Title = newCard.Title,
                Description = newCard.Description,
                IsDeleted = false
            };

            var result = await _boards.AddCardToBoardAsync(boardId, mongoCard);

            if (result)
                return new OkObjectResult(new { Id = mongoCard.Id });
            else
                return new ObjectResult("Failed to add card") { StatusCode = 500 };
        }

        [FunctionName("getCards")]
        public async Task<IActionResult> GetCardsForBoardAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "boards/{boardId:Guid}/cards")] HttpRequest request, Guid boardId, ILogger log)
        {
            var board = await _boards.GetBoardByIdAsync(boardId);
            if (board == null)
                return new NotFoundObjectResult("board not found");

            var cards = await _boards.GetCardsForBoard(boardId);

            if (!cards.Any())
                return new NoContentResult();

            return new OkObjectResult(cards.Select(x => new CardDTO()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description
            }));
        }

        [FunctionName("getCardDetail")]
        public async Task<IActionResult> GetCardDetailAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "boards/{boardId:Guid}/cards/{cardId:Guid}")] HttpRequest request, Guid boardId, Guid cardId, ILogger log)
        {
            var board = await _boards.GetBoardByIdAsync(boardId);
            if (board == null)
                return new NotFoundObjectResult("board not found");

            var cards = await _boards.GetCardsForBoard(boardId);
            var card = cards.FirstOrDefault(x => x.Id == cardId); //todo maybe filter this in mongo
            if (card == null)
                return new NotFoundObjectResult("card not found");

            return new OkObjectResult(new CardDTO()
            {
                Id = card.Id,
                Description = card.Description,
                Title = card.Title
            });
        }
    }
}