using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskBoard.API.Models;

namespace TaskBoard.API.Services.Abstractions
{
    public interface IBoardService
    {
        Task InsertBoardAsync(Board board);
        Task<Board> GetBoardByIdAsync(Guid boardId);
        IEnumerable<Board> GetAllBoards();
    }
}