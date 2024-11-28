using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Dto.Chat.OpenAI;
using ChatTailorAI.Shared.Models.Chat;
using ChatTailorAI.Shared.Services.Files;

namespace ChatTailorAI.Shared.Transformers.OpenAI
{
    public class OpenAIChatMessageTransformer : IChatMessageTransformer
    {
        private readonly IImageFileService _imageFileService;

        public OpenAIChatMessageTransformer(IImageFileService imageFileService)
        {
            _imageFileService = imageFileService;
        }

        public async Task<IChatModelMessage> Transform(ChatMessageDto messageDto)
        {
            switch (messageDto)
            {
                case ChatImageMessageDto chatImageMessageDto:
                    return await TransformToImageMessage(chatImageMessageDto);
                case ChatMessageDto chatMessageDto:
                    return await Task.FromResult(TransformToTextMessage(chatMessageDto));
                default:
                    return await Task.FromResult(TransformToTextMessage(messageDto));
            }
        }

        private async Task<OpenAIBaseChatMessageDto> TransformToImageMessage(ChatImageMessageDto messageDto)
        {
            switch (messageDto.Role)
            {
                case "user":
                    return await TransformUserImageMessage(messageDto);
                case "assistant":
                    return await Task.FromResult(TransformAssistantMessage(messageDto));
                default:
                    throw new InvalidOperationException("Unknown message role");
            }
        }

        private async Task<OpenAIBaseChatMessageDto> TransformUserImageMessage(ChatImageMessageDto messageDto)
        {
            var contentParts = new List<object>();

            // If there is text, add it to the content parts
            if (!string.IsNullOrEmpty(messageDto.Content))
            {
                contentParts.Add(new { type = "text", text = messageDto.Content });
            }

            if (messageDto.Images != null && messageDto.Images.Count > 0)
            {
                foreach (var image in messageDto.Images)
                {
                    if (image.Url.Contains("http"))
                    {
                        contentParts.Add(new
                        {
                            type = "image_url",
                            image_url = new
                            {
                                url = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/dd/Gfp-wisconsin-madison-the-nature-boardwalk.jpg/2560px-Gfp-wisconsin-madison-the-nature-boardwalk.jpg",
                                detail = "high"
                            }
                        });
                    }
                    else
                    {
                        // TODO: Better approach as this is upping mem usage for UWP app
                        // Eventually use cloud service with URL, but for now just convert to base 64
                        var base64Image = await _imageFileService.ConvertImageToBase64(image.LocalUri.ToString());
                        string imageDataURI = $"data:image/jpeg;base64,{base64Image}";

                        contentParts.Add(new
                        {
                            type = "image_url",
                            image_url = new
                            {
                                url = imageDataURI,
                                detail = "high"
                            }
                        });
                    }
                }

            }

            // Determine if content is just text or an array of content parts
            object content = contentParts.Count == 1 ? contentParts[0] : contentParts.ToArray();

            return new OpenAIUserChatMessageDto
            {
                Role = messageDto.Role,
                Content = content,
            };
        }

        private OpenAIAssistantChatMessageDto TransformAssistantMessage(ChatImageMessageDto imageMessageDto)
        {
            return new OpenAIAssistantChatMessageDto
            {
                Role = imageMessageDto.Role,
                Content = imageMessageDto.Content,
                // Set Name, ToolCalls, and FunctionCall properties if available ?
                // Function call comes after initial response typically
            };
        }

        private OpenAIBaseChatMessageDto TransformToTextMessage(ChatMessageDto messageDto)
        {
            switch (messageDto.Role)
            {
                case "user":
                    return TransformUserTextMessage(messageDto);
                case "assistant":
                    return TransformAssistantTextMessage(messageDto);
                default:
                    throw new InvalidOperationException("Unknown message role");
            }
        }

        private OpenAIUserChatMessageDto TransformUserTextMessage(ChatMessageDto messageDto)
        {
            return new OpenAIUserChatMessageDto
            {
                Role = messageDto.Role,
                Content = messageDto.Content,
            };
        }

        private OpenAIAssistantChatMessageDto TransformAssistantTextMessage(ChatMessageDto messageDto)
        {
            return new OpenAIAssistantChatMessageDto
            {
                Role = messageDto.Role,
                Content = messageDto.Content,
                // Set Name, ToolCalls, and FunctionCall properties if available ?
                // Function call comes after initial response typically
            };
        }
    }
}
