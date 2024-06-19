using ChatTailorAI.DataAccess.Database;
using ChatTailorAI.DataAccess.Database.Providers.SQLite;
using ChatTailorAI.Shared.Models.Assistants;
using ChatTailorAI.Shared.Models.Chat;
using ChatTailorAI.Shared.Services.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.DataAccess.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly SQLiteDb _context;

        public MessageRepository(SQLiteDb context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _context.Assistants.AnyAsync(a => a.Id == id);
        }

        public async Task AddAsync(ChatMessage message)
        {
            if (await ExistsAsync(message.Id))
            {
                throw new InvalidOperationException("Message already exists.");
            }

            try
            {
                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error saving chat message.", ex);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);

                throw;
            }
        }

        public async Task DeleteAllAsync()
        {
            _context.Messages.RemoveRange(_context.Messages);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ChatMessage message)
        {
            var messageToDelete = await _context.Messages.FirstOrDefaultAsync(c => c.Id == message.Id);
            if (messageToDelete != null)
            {
                // Soft delete
                messageToDelete.IsDeleted = true;
                messageToDelete.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ChatMessage>> GetAllByConversationIdAsync(string conversationId)
        {
            var messages = await _context.Messages
                .Where(c => c.ConversationId == conversationId)
                .ToListAsync();

            foreach (var message in messages.OfType<ChatImageMessage>())
            {
                await _context.Entry(message).Collection(m => m.Images).LoadAsync();
            }

            return messages;
        }

        public async Task<ChatMessage> GetAsync(string id)
        {
            return await _context.Messages.FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task UpdateAsync(ChatMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
