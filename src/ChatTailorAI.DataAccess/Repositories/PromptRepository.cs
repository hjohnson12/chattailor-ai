using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChatTailorAI.DataAccess.Database.Providers.SQLite;
using ChatTailorAI.Shared.Models.Prompts;
using ChatTailorAI.Shared.Repositories;

namespace ChatTailorAI.DataAccess.Repositories
{
    public class PromptRepository : IPromptRepository
    {
        private readonly SQLiteDb _context;

        public PromptRepository(SQLiteDb context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _context.Prompts.AnyAsync(a => a.Id == id);
        }

        public async Task AddPromptAsync(Prompt prompt)
        {
            if (await ExistsAsync(prompt.Id))
            {
                throw new InvalidOperationException("Prompt already exists");
            }

            try
            {
                await _context.Prompts.AddAsync(prompt);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error saving prompt data.", ex);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);

                throw;
            }
        }

        public Task DeleteAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Prompt prompt)
        {
            _context.Prompts.Remove(prompt);
            return _context.SaveChangesAsync();
        }

        public Task<List<Prompt>> GetAllAsync()
        {
            return _context.Prompts.ToListAsync();
        }

        public Task<Prompt> GetAsync(string id)
        {
            return _context.Prompts.FirstOrDefaultAsync(a => a.Id == id);
        }

        public Task UpdateAsync(Prompt prompt)
        {
            _context.Prompts.Update(prompt);
            return _context.SaveChangesAsync();
        }
    }
}
