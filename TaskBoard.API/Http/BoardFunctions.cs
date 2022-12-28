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
using MongoDB.Driver;
using TaskBoard.API.Models;
using TaskBoard.API.Models.DTO;
using TaskBoard.API.Services.Abstractions;

namespace TaskBoard.API.Http
{
    public class BoardFunctions
    {
        private readonly IBoardService _boards;

        public BoardFunctions(IBoardService boards)
        {
            _boards = boards;
        }

        [FunctionName("createBoard")]
        public async Task<IActionResult> CreateAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "boards")] HttpRequest request, ILogger log)
        {
            var body = await request.GetBodyAsync();

            BoardDTO newBoard;
            try
            {
                newBoard = JsonSerializer.Deserialize<BoardDTO>(body);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "failed to deserialize board");
                return new BadRequestObjectResult("invalid request body");
            }

            var mongoBoard = new Board()
            {
                Id = Guid.NewGuid(),
                Title = newBoard.Title,
                Description = newBoard.Description,
                IsDeleted = false
            };

            await _boards.InsertBoardAsync(mongoBoard);

            return new OkObjectResult(new { Id = mongoBoard.Id });
        }

        [FunctionName("getAllBoards")]
        public IActionResult GetAllBoards([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "boards")] HttpRequest request, ILogger log)
        {
            var boards = _boards.GetAllBoards();
            if (!boards.Any())
                return new NoContentResult();

            return new OkObjectResult(boards.Select(x => new BoardDTO
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description
            }));
        }

        [FunctionName("getBoardById")]
        public async Task<IActionResult> GetByIdAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "boards/{boardId:Guid}")] HttpRequest request, Guid boardId, ILogger log)
        {
            if (boardId == Guid.Empty)
                return new BadRequestObjectResult("invalid board id");

            var board = await _boards.GetBoardByIdAsync(boardId);

            if (board == null)
                return new NotFoundObjectResult("board not found");

            return new OkObjectResult(new BoardDTO()
            {
                Id = board.Id,
                Title = board.Title,
                Description = board.Description
            });
        }
    }
}