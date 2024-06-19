using ChatTailorAI.Shared.Base;
using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Dto.Conversations;
using ChatTailorAI.Shared.Dto.Conversations.OpenAI;
using ChatTailorAI.Shared.Models.Assistants;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Linq;

namespace ChatTailorAI.Shared.ViewModels
{
    public class ConversationViewModel : Observable
    {
        private string _title;
        private string _model;
        private string _instructions;
        private string _conversationType;
        private DateTime _createdAt;
        private string _id;

        // Properties specific to certain conversation types
        private string _assistantId;
        private AssistantType _assistantType;
        private string _threadId;

        public ConversationViewModel()
        {
            
        }

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        public string Instructions
        {
            get => _instructions;
            set => SetProperty(ref _instructions, value);
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set => SetProperty(ref _createdAt, value);
        }

        public string ConversationType
        {
            get => _conversationType;
            set
            {
                SetProperty(ref _conversationType, value);
            }
        }
       
        public string AssistantId 
        { 
            get => _assistantId; 
            set => SetProperty(ref _assistantId, value); 
        }

        public AssistantType AssistantType 
        { 
            get => _assistantType; 
            set => SetProperty(ref _assistantType, value); 
        }

        public string ThreadId 
        { 
            get => _threadId;
            set => SetProperty(ref _threadId, value); 
        }

        // Workaround since assistant name isnt stored in conversation
        private string _assistantName;
        public string AssistantName
        {
            get => _assistantName;
            set => SetProperty(ref _assistantName, value);
        }

        public ConversationDto ToDto()
        {
            switch (ConversationType)
            {
                case "OpenAI":
                    return new OpenAIConversationDto
                    {
                        Id = Id,
                        Title = Title,
                        Model = Model,
                        Instructions = Instructions,
                        ConversationType = ConversationType,
                        CreatedAt = CreatedAt
                    };

                case "Assistant":
                    return new AssistantConversationDto
                    {
                        Id = Id,
                        Title = Title,
                        Model = Model,
                        Instructions = Instructions,
                        ConversationType = ConversationType,
                        CreatedAt = CreatedAt,
                        AssistantId = AssistantId,
                        AssistantType = AssistantType
                    };

                case "OpenAI Assistant":
                    return new OpenAIAssistantConversationDto
                    {
                        Id = Id,
                        Title = Title,
                        Model = Model,
                        Instructions = Instructions,
                        ConversationType = ConversationType,
                        CreatedAt = CreatedAt,
                        AssistantId = AssistantId,
                        AssistantType = AssistantType,
                        ThreadId = ThreadId
                    };

                default:
                    return new ConversationDto
                    {
                        Id = Id,
                        Title = Title,
                        Model = Model,
                        Instructions = Instructions,
                        ConversationType = ConversationType,
                        CreatedAt = CreatedAt
                    };
            }
        }

        public static ConversationViewModel FromDto(ConversationDto dto, string assistantName = null)
        {
            var viewModel = new ConversationViewModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Model = dto.Model,
                Instructions = dto.Instructions,
                ConversationType = dto.ConversationType,
                CreatedAt = dto.CreatedAt
            };

            if (dto is OpenAIAssistantConversationDto openAIAssistantDto)
            {
                viewModel.AssistantId = openAIAssistantDto.AssistantId;
                viewModel.AssistantType = openAIAssistantDto.AssistantType;
                viewModel.ThreadId = openAIAssistantDto.ThreadId;
                viewModel.AssistantName = assistantName;
            }
            else if (dto is AssistantConversationDto assistantDto)
            {
                viewModel.AssistantId = assistantDto.AssistantId;
                viewModel.AssistantType = assistantDto.AssistantType;
                viewModel.AssistantName = assistantName;
            }

            return viewModel;
        }
    }

    // TODO: Possibly split out into separate view models & avoid 
    // factory method / type checks (adding claude/gemini later)

    public class OpenAIConversationViewModel : ConversationViewModel
    {
    }

    public class AssistantConversationViewModel : ConversationViewModel
    {
    }

    public class OpenAIAssistantConversationViewModel : AssistantConversationViewModel
    {
    }
}