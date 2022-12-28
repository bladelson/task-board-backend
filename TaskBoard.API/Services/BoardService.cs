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
            var collection = GetCollection<Board>(BOARD_COLLECTION);

            await collection.InsertOneAsync(board);
        }

        public IEnumerable<Board> GetAllBoards()
        {
            var collection = GetCollection<Board>(BOARD_COLLECTION);
            return collection.AsQueryable().Where(x => !x.IsDeleted);
        }

        public async Task<Board> GetBoardByIdAsync(Guid boardId)
        {
            var collection = GetCollection<Board>(BOARD_COLLECTION);
            return await collection.AsQueryable().FirstOrDefaultAsync(x => x.Id == boardId);
        }
    }
}