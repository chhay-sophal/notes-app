using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class NoteService
    {
        private readonly AppDbContext _context;

        public NoteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<NoteItem>> GetNotes(string userId)
        {
            return await _context.NoteItems
                .Where(x => x.UserId == userId && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<NoteItem> GetNoteById(int id)
        {
            return await _context.NoteItems.FindAsync(id);
        }

        public async Task CreateNote(NoteItem note)
        {
            _context.NoteItems.Add(note);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNote(NoteItem note)
        {
            _context.NoteItems.Update(note);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNote(int id)
        {
            var note = await _context.NoteItems.FindAsync(id);

            if (note == null)
            {
                return;
            }

            note.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}