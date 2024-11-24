using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Services.Chat;
using ChatTailorAI.Shared.Services.Files;

namespace ChatTailorAI.Services.Chat
{
    public class ChatFileService : IChatFileService
    {
        private readonly IFileService _fileService;

        public ChatFileService(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task SaveMessagesToFileAsync(string filename, IEnumerable<ChatMessageDto> messages)
        {
            var fileContents = ConvertMessagesToFileFormat(messages);
            await _fileService.SaveToFileAsync(filename, fileContents);
        }

        public async Task<IEnumerable<ChatMessageDto>> LoadMessagesFromFileAsync()
        {
            var fileContents = await _fileService.ReadFromFileAsync();
            if (fileContents != null) return ParseMessagesFile(fileContents);
            return new List<ChatMessageDto>();
        }

        private string ConvertMessagesToFileFormat(IEnumerable<ChatMessageDto> messages)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var msg in messages)
            {
                sb.AppendLine($"{msg.Role}");
                if (msg.Role == "assistant")
                    sb.AppendLine("---------");
                else
                    sb.AppendLine("----");
                sb.AppendLine(msg.Content);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private IEnumerable<ChatMessageDto> ParseMessagesFile(string fileContents)
        {
            string[] lines = fileContents.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );

            var messages = new List<ChatMessageDto>();
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "user" || lines[i] == "assistant")
                {
                    var message = new ChatMessageDto();
                    message.Role = lines[i];

                    StringBuilder content = new StringBuilder();

                    // Ignore the "----" or "---------" line and move on to the content
                    i += 2;

                    // Keep adding to content until we reach a new "role" or the end of lines
                    while (i < lines.Length && lines[i] != "user" && lines[i] != "assistant")
                    {
                        content.AppendLine(lines[i]);
                        i++;
                    }

                    // We've either found a new role or reached the end, so let's back up one line for the outer loop
                    i--;

                    message.Content = content.ToString().TrimEnd('\r', '\n'); // Removing trailing newlines

                    messages.Add(message);
                }
            }

            return messages;
        }
    }
}
