using ChatTailorAI.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;

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
