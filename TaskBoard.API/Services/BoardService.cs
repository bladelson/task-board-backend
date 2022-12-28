using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using TaskBoard.API.Models;
using MongoDB.Driver.Linq;
using TaskBoard.API.Services.Abstractions;

namespace TaskBoard.API.Services
{
    public class BoardService : MongoService, IBoardService
    {
        private readonly ILogger<BoardService> _log;
        private const string BOARD_COLLECTION = "boards";

        public BoardService(IMongoClient mongo, ILogger<BoardService> log)
        : base(mongo)
        {
            _log = log;
        }

        public async Task InsertBoardAsync(Board board)
        {
            board = board ?? throw new ArgumentNullException(nameof(board));
            var collection = GetBoardCollection();

            await collection.InsertOneAsync(board);
        }

        public IEnumerable<Board> GetAllBoards()
        {
            var collection = GetBoardCollection();
            return collection.AsQueryable().Where(x => !x.IsDeleted);
        }

        public async Task<Board> GetBoardByIdAsync(Guid boardId)
        {
            var collection = GetBoardCollection();
            return await collection.AsQueryable().FirstOrDefaultAsync(x => x.Id == boardId);
        }

        public async Task<bool> AddCardToBoardAsync(Guid boardId, Card card)
        {
            var filter = Builders<Board>.Filter.Eq(x => x.Id, boardId);
            var update = Builders<Board>.Update.AddToSet(x => x.Cards, card);

            var collection = GetBoardCollection();
            var result = await collection.UpdateOneAsync(filter, update);

            return result.IsAcknowledged;
        }

        public async Task<IEnumerable<Card>> GetCardsForBoard(Guid boardId)
        {
            var board = await GetBoardByIdAsync(boardId);
            return board.Cards;
        }

        private IMongoCollection<Board> GetBoardCollection() => GetCollection<Board>(BOARD_COLLECTION);
    }
}