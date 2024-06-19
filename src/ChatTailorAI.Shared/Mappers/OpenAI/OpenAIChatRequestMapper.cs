using ChatTailorAI.Shared.Dto.Chat.OpenAI;
using ChatTailorAI.Shared.Mappers.Interfaces;
using ChatTailorAI.Shared.Models.Chat.OpenAI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Mappers.OpenAI
{
    public class OpenAIChatRequestMapper : IOpenAIChatRequestMapper
    {
        public OpenAIChatRequestDto MapToDto(OpenAIChatRequest request)
        {
            return new OpenAIChatRequestDto
            {
                Model = request.Model,
                Instructions = request.Instructions,
                Messages = request.Messages,
                Settings = MapSettingsToDto(request.Settings)
            };
        }

        private OpenAIChatSettingsDto MapSettingsToDto(OpenAIChatSettings settings)
        {
            return new OpenAIChatSettingsDto
            {
                MaxTokens = settings.MaxTokens,
                Temperature = settings.Temperature,
                TopP = settings.TopP,
                FrequencyPenalty = settings.FrequencyPenalty,
                PresencePenalty = settings.PresencePenalty,
                Stream = settings.Stream,
                StopSequences = settings.StopSequences,
            };
        }
    }
}
