using ChatTailorAI.Shared.Base;
using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Models.Assistants;

namespace ChatTailorAI.Shared.ViewModels
{
    public class AssistantViewModel : Observable
    {
        private string _id;
        private string _externalAssistantId;
        private string _name;
        private string _model;
        private string _description;
        private string _createdAt;
        private string _instructions;
        private AssistantType _assistantType;

        public AssistantViewModel(AssistantDto dto)
        {
            _id = dto.Id;
            _externalAssistantId = dto.ExternalAssistantId;
            _name = dto.Name;
            _model = dto.Model;
            _description = dto.Description;
            _createdAt = dto.CreatedAt;
            _instructions = dto.Instructions;
            _assistantType = dto.AssistantType;
        }

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string ExternalAssistantId
        {
            get => _externalAssistantId;
            set => SetProperty(ref _externalAssistantId, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string CreatedAt
        {
            get => _createdAt;
            set => SetProperty(ref _createdAt, value);
        }

        public string Instructions
        {
            get => _instructions;
            set => SetProperty(ref _instructions, value);
        }

        public AssistantType AssistantType
        {
            get => _assistantType;
            set => SetProperty(ref _assistantType, value);
        }

        public void UpdateFromDto(AssistantDto dto)
        {
            Id = dto.Id;
            ExternalAssistantId = dto.ExternalAssistantId;
            Name = dto.Name;
            Model = dto.Model;
            Description = dto.Description;
            CreatedAt = dto.CreatedAt;
            Instructions = dto.Instructions;
            AssistantType = dto.AssistantType;

            // Notify the UI that all properties have been updated
            OnPropertyChanged(string.Empty);
        }

        public AssistantDto ToDto()
        {
            return new AssistantDto
            {
                Id = this.Id,
                ExternalAssistantId = this.ExternalAssistantId,
                Name = this.Name,
                Model = this.Model,
                Description = this.Description,
                CreatedAt = this.CreatedAt,
                Instructions = this.Instructions,
                AssistantType = this.AssistantType
            };
        }
    }
}
