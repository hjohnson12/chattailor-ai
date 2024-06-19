using ChatTailorAI.Shared.Dto.Chat.Anthropic;
using ChatTailorAI.Shared.Dto.Chat;
using ChatTailorAI.Shared.Models.Chat;
using ChatTailorAI.Shared.Services.Files;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto.Chat.Google;
using ChatTailorAI.Shared.Models.Chat.Google.Content;

namespace ChatTailorAI.Shared.Transformers.Google
{
    public class GoogleChatMessageTransformer : IChatMessageTransformer
    {
        private readonly IImageFileService _imageFileService;

        public GoogleChatMessageTransformer(IImageFileService imageFileService)
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

        private async Task<GoogleBaseChatMessageDto> TransformToImageMessage(ChatImageMessageDto messageDto)
        {
            switch (messageDto.Role)
            {
                case "user":
                    return await TransformUserImageMessage(messageDto);
                case "model":
                    return await Task.FromResult(TransformAssistantMessage(messageDto));
                case "assistant":
                    return await Task.FromResult(TransformAssistantMessage(messageDto));
                default:
                    throw new InvalidOperationException("Unknown message role");
            }
        }

        private async Task<GoogleBaseChatMessageDto> TransformUserImageMessage(ChatImageMessageDto messageDto)
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
                        var mediaType = image.LocalUri.ToString().EndsWith(".png") ? "image/png" : "image/jpeg";

                        contentParts.Add(new
                        {
                            type = "image",
                            source = new
                            {
                                type = "base64",
                                media_type = mediaType,
                                data = base64Image,
                            }
                        });
                    }
                }

            }

            // Determine if content is just text or an array of content parts
            object content = contentParts.Count == 1 ? contentParts[0] : contentParts.ToArray();

            return new GoogleUserChatMessageDto
            {
                Role = messageDto.Role,
                Content = content,
            };
        }

        private GoogleAssistantChatMessageDto TransformAssistantMessage(ChatImageMessageDto imageMessageDto)
        {
            return new GoogleAssistantChatMessageDto
            {
                Role = imageMessageDto.Role,
                Content = imageMessageDto.Content,
                // Set Name, ToolCalls, and FunctionCall properties if available ?
                // Function call comes after initial response typically
            };
        }

        private GoogleBaseChatMessageDto TransformToTextMessage(ChatMessageDto messageDto)
        {
            switch (messageDto.Role)
            {
                case "user":
                    return TransformUserTextMessage(messageDto);
                case "model":
                    return TransformAssistantTextMessage(messageDto);
                case "assistant":
                    return TransformAssistantTextMessage(messageDto);
                default:
                    throw new InvalidOperationException("Unknown message role");
            }
        }

        private GoogleUserChatMessageDto TransformUserTextMessage(ChatMessageDto messageDto)
        {
            // All google gemini messages use an array of content parts
            var newContent = new List<GoogleTextContentPart>
            {
                new GoogleTextContentPart { Text = messageDto.Content }
            };

            return new GoogleUserChatMessageDto
            {
                Role = messageDto.Role,
                Content = newContent,
            };
        }

        private GoogleAssistantChatMessageDto TransformAssistantTextMessage(ChatMessageDto messageDto)
        {
            var newContent = new List<GoogleTextContentPart>
            {
                new GoogleTextContentPart { Text = messageDto.Content }
            };

            return new GoogleAssistantChatMessageDto
            {
                Role = (messageDto.Role.Equals("assistant")) ? "model" : messageDto.Role,
                Content = newContent,
                // Set Name, ToolCalls, and FunctionCall properties if available ?
                // Function call comes after initial response typically
            };
        }
    }
}
