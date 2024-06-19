using System;
using Microsoft.EntityFrameworkCore;
using ChatTailorAI.Shared.Models.Assistants;
using ChatTailorAI.Shared.Models.Chat;
using ChatTailorAI.Shared.Models.Conversations;
using ChatTailorAI.Shared.Models.Prompts;
using ChatTailorAI.Shared.Models.Conversations.OpenAI;

namespace ChatTailorAI.DataAccess.Database
{
    public class Db : DbContext
    {
        public DbSet<Assistant> Assistants { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ArchivedConversation> ArchivedConversations { get; set; }
        public DbSet<ChatMessage> Messages { get; set; }
        public DbSet<ChatImage> Images { get; set; }
        public DbSet<Prompt> Prompts { get; set; }

        public Db(Func<Interfaces.IDbContextOptions> dbContextOptionsFactory) : base(dbContextOptionsFactory().Options) { }
        public Db(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            OnAssistantModelCreating(mb);
            OnPromptModelCreating(mb);
            OnChatModelCreating(mb);
            OnArchivedChatModelCreating(mb);
            OnChatMessageModelCreating(mb);
            OnChatImageModelCreating(mb);
        }

        private void OnAssistantModelCreating(ModelBuilder mb)
        {
            mb.Entity<Assistant>()
                    .Property(a => a.ExternalAssistantId)
                    .IsRequired(false);

            mb.Entity<Assistant>()
               .Property(a => a.IsDeleted)
               .HasDefaultValue(false);

            mb.Entity<Assistant>()
                .HasQueryFilter(a => !a.IsDeleted);
        }

        private void OnChatModelCreating(ModelBuilder mb)
        {
            mb.Entity<Conversation>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Conversation>("Standard")
                .HasValue<OpenAIConversation>("OpenAI")
                .HasValue<AssistantConversation>("Assistant")
                .HasValue<OpenAIAssistantConversation>("OpenAIAssistant");

            mb.Entity<Conversation>()
                .Property(c => c.IsDeleted)
                .HasDefaultValue(false);

            mb.Entity<Conversation>()
                .HasQueryFilter(c => !c.IsDeleted);

            // To prevent deletion of AssistantConversations on
            // deletion of the Assistant
            // Note: Possibly replace with .OnDelete to restrict cascade delete, if it works as expected
            mb.Entity<AssistantConversation>()
                .Property(ac => ac.AssistantId)
                .IsRequired(false);
        }

        private void OnArchivedChatModelCreating(ModelBuilder mb)
        {
            mb.Entity<ArchivedConversation>()
                .Property(ac => ac.AssistantType)
                .IsRequired(false);

            mb.Entity<ArchivedConversation>()
                .Property(ac => ac.AssistantId)
                .IsRequired(false);

            mb.Entity<ArchivedConversation>()
                .Property(ac => ac.ThreadId)
                .IsRequired(false);

            // TODO: Possibly keep Table-Per-Hierarchy (TPH) structure similar to Conversation
            // Depends how I end up allowing users to query archived data
            //mb.Entity<ArchivedConversation>()
            //    .HasDiscriminator<string>("ConversationType")
            //    .HasValue<ArchivedConversation>("Standard")
            //    .HasValue<ArchivedOpenAIConversation>("OpenAI")
            //    .HasValue<ArchivedAssistantConversation>("Assistant")
            //    .HasValue<ArchivedOpenAIAssistantConversation>("OpenAIAssistant");
        }

        private void OnChatMessageModelCreating(ModelBuilder mb)
        {
            mb.Entity<ChatMessage>()
                .Property(e => e.MessageType)
                .HasConversion<string>();

            mb.Entity<ChatMessage>()
                .HasDiscriminator<string>("Discriminator") 
                .HasValue<ChatMessage>("ChatMessage")
                .HasValue<ChatImageMessage>("ChatImageMessage");

            mb.Entity<ChatImageMessage>()
                .HasMany(cim => cim.Images)
                .WithOne(ci => ci.ChatImageMessage)
                .HasForeignKey(ci => ci.MessageId);

            mb.Entity<ChatMessage>()
               .Property(ac => ac.ExternalMessageId)
               .IsRequired(false);
        }

        private void OnChatImageModelCreating(ModelBuilder mb)
        {
            mb.Entity<ChatImage>(entity =>
            {
                entity.HasKey(e => e.Id); // Primary key
                entity.Property(e => e.Url).IsRequired();

                entity.HasOne(e => e.Prompt) // One prompt associated with image
                    .WithMany() // Prompt can be associated with many ChatImages
                    .HasForeignKey(e => e.PromptId) // Foreign key
                    // Avoid deleting images when deleting prompts
                    // Sets the foreign key to null when the primary key is deleted (keep?)
                    .OnDelete(DeleteBehavior.ClientSetNull); 
            });
        }

        private void OnPromptModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Prompt>(entity =>
            {
                // Won't be mapped to the database
                entity.Ignore(c => c.IsActive);

                // Primary key
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Content).IsRequired();
            });
        }
    }
}