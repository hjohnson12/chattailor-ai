using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChatTailorAI.DataAccess.Database.Providers.SQLite;
using ChatTailorAI.Shared.Models.Chat;
using ChatTailorAI.Shared.Repositories;

namespace ChatTailorAI.DataAccess.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly SQLiteDb _context;

        public ImageRepository(SQLiteDb context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _context.Images.AnyAsync(i => i.Id == id);
        }

        public async Task AddAsync(ChatImage image)
        {
            if (await ExistsAsync(image.Id))
            {
                throw new InvalidOperationException("Image already exists.");
            }

            try
            {
                await _context.Images.AddAsync(image);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error saving image data.", ex);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);

                throw;
            }
        }

        public async Task DeleteAsync(ChatImage image)
        {
            // Currently, image is a new instance and runs into the
            // tracking error. This is a workaround to delete the image
            var existingImage = await _context.Images.FirstOrDefaultAsync(i => i.Id == image.Id);
            if (existingImage != null)
            {
                _context.Images.Remove(existingImage);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ChatImage>> GetAllAsync()
        {
            return await _context.Images.Include(i => i.Prompt).ToListAsync();
        }

        public async Task<ChatImage> GetAsync(int id)
        {
            return await _context.Images.FindAsync(id);
        }

        public Task UpdateAsync(ChatImage image)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ChatImage>> GetAllByMessageId(string messageId)
        {
            return await _context.Images.Where(i => i.MessageId == messageId).ToListAsync();
        }
    }
}