using ChatTailorAI.Shared.Dto;

namespace ChatTailorAI.Shared.ViewModels.Dialogs
{
    public class EditPromptDialogViewModel
    {
        public EditPromptDialogViewModel()
        {
        }

        public PromptDto CurrentPrompt { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
    }
}
