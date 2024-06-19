using ChatTailorAI.DataAccess.Database;
using ChatTailorAI.DataAccess.Database.Providers.SQLite;
using ChatTailorAI.Shared.Models.Assistants;
using ChatTailorAI.Shared.Models.Conversations;
using ChatTailorAI.Shared.Repositories;
using ChatTailorAI.Shared.Services.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.DataAccess.Repositories
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly SQLiteDb _context;

        public ConversationRepository(SQLiteDb context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _context.Conversations.AnyAsync(a => a.Id == id);
        }

        public async Task<List<Conversation>> GetAllAsync()
        {
            return await _context.Conversations.ToListAsync();
        }

        public async Task AddConversationAsync(Conversation conversation)
        {
            try
            {
                await _context.Conversations.AddAsync(conversation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // TODO: Add output of inner exception to error message
                throw new InvalidOperationException($"Error saving chat data: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);

                throw;
            }
        }

        public async Task<Conversation> GetConversationAsync(string id)
        {
            return await _context.Conversations.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task DeleteAsync(string conversationId)
        {
            var conversation = await _context.Conversations.FindAsync(conversationId);
            if (conversation != null)
            {
                // Soft delete to allow for archived items, and a permanent delete thereafter
                conversation.IsDeleted = true;
                conversation.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("The conversation with the specified ID was not found.");
            }
        }

        public async Task<Conversation> GetAsync(string id)
        {
            return await _context.Conversations.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task UpdateAsync(Conversation conversation)
        {
            // Note: This seems to be the only way to update an entity that is not attached to the context
            // Or else we get an exception: "The instance of entity type 'Conversation' cannot be tracked because another instance with the same key value for {'Id'} is already being tracked."
            _context.Attach(conversation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
