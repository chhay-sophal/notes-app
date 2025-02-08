using Backend.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class NoteService
    {
        private readonly IDbConnection _dbConnection;

        public NoteService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<List<NoteItem>> GetActiveNotes(string userId)
        {
            var sql = "SELECT * FROM NoteItems WHERE UserId = @UserId AND IsDeleted = 0";
            var notes = await _dbConnection.QueryAsync<NoteItem>(sql, new { UserId = userId });
            return notes.ToList();
        }

        public async Task<List<NoteItem>> GetAllNotes(string userId)
        {
            var sql = "SELECT * FROM NoteItems WHERE UserId = @UserId";
            var notes = await _dbConnection.QueryAsync<NoteItem>(sql, new { UserId = userId });
            return notes.ToList();
        }

        public async Task<NoteItem> GetNoteById(int id)
        {
            var sql = "SELECT * FROM NoteItems WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<NoteItem>(sql, new { Id = id });
        }

        public async Task CreateNote(NoteItem note)
        {
            var sql = "INSERT INTO NoteItems (Title, Content, UserId, IsDeleted) VALUES (@Title, @Content, @UserId, @IsDeleted)";
            await _dbConnection.ExecuteAsync(sql, note);
        }

        public async Task UpdateNote(NoteItem note)
        {
            var sql = "UPDATE NoteItems SET Title = @Title, Content = @Content, IsDeleted = @IsDeleted WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, note);
        }

        public async Task DeleteNote(int id)
        {
            var sql = "UPDATE NoteItems SET IsDeleted = 1 WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }
    }
}