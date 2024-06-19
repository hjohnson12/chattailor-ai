using ChatTailorAI.DataAccess.Database;
using ChatTailorAI.DataAccess.Database.Providers.SQLite;
using ChatTailorAI.Shared.Models.Assistants;
using ChatTailorAI.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.DataAccess.Repositories
{
    public class AssistantRepository : IAssistantRepository
    {
        private readonly SQLiteDb _context;

        public AssistantRepository(SQLiteDb context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _context.Assistants.AnyAsync(a => a.Id == id);
        }

        public async Task<List<Assistant>> GetAllAsync()
        {
            return await _context.Assistants.ToListAsync();
        }

        public async Task AddAssistantAsync(Assistant assistant)
        {
            if (await ExistsAsync(assistant.Id))
            {
                throw new InvalidOperationException("Assistant already exists.");
            }

            try
            {
                await _context.Assistants.AddAsync(assistant);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error saving assistant data.", ex);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);

                throw;
            }
        }

        public async Task DeleteAllAsync()
        {
            _context.Assistants.RemoveRange(_context.Assistants);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Assistant assistant)
        {
            // Soft delete, keep record in db to allow retrieving archived items
            // and a permanent delete thereafter
            assistant.IsDeleted = true;
            assistant.DeletedAt = DateTime.UtcNow;
            _context.Entry(assistant).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string assistantId)
        {
            var entity = new Assistant { Id = assistantId };
            _context.Assistants.Attach(entity);
            _context.Assistants.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Assistant> GetAsync(string id)
        {
            return await _context.Assistants.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task UpdateAsync(Assistant assistant)
        {
            //_context.Assistants.Update(assistant);

            // Note: This seems to be the only way to update an entity that is not attached to the context
            // Or else we get an exception: "The instance of entity type 'Assistant' cannot be tracked because another instance with the same key value for {'Id'} is already being tracked."
            _context.Attach(assistant).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateExistingAsync(Assistant assistant)
        {
            var existingAssistant = _context.Assistants.Local.FirstOrDefault(a => a.Id == assistant.Id);
            if (existingAssistant != null)
            {
                _context.Entry(existingAssistant).CurrentValues.SetValues(assistant);
            }
            else
            {
                _context.Assistants.Update(assistant);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<Assistant>> GetByIdsAsync(IEnumerable<string> ids)
        {
            return await _context.Assistants.Where(a => ids.Contains(a.Id)).ToListAsync();
        }
    }
}