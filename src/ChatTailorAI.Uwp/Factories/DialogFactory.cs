using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using ChatTailorAI.Shared.Dto.Conversations;
using ChatTailorAI.Shared.Factories.ViewModels;
using ChatTailorAI.Shared.Factories.Interfaces;
using ChatTailorAI.Shared.ViewModels.Dialogs;
using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Uwp.Views.Dialogs;
using ChatTailorAI.Uwp.Views;

namespace ChatTailorAI.Uwp.Factories
{
    public class DialogFactory : IDialogFactory
    {
        private bool _isDialogOpen;
        private readonly IEditChatDialogViewModelFactory _editChatDialogViewModelFactory;

        public DialogFactory(
            IEditChatDialogViewModelFactory editChatDialogViewModelFactory)
        {
            _editChatDialogViewModelFactory = editChatDialogViewModelFactory;
        }

        public bool CheckForOpenDialog()
        {
            return _isDialogOpen;
        }

        public async Task ShowDialogAsync<T>(T dialog) where T : class
        {
            if (dialog is ContentDialog contentDialog)
            {
                if (_isDialogOpen)
                    return;

                _isDialogOpen = true;

                await contentDialog.ShowAsync();

                _isDialogOpen = false;
            }
        }

        public async Task<bool> ShowDeleteDialogAsync()
        {
            // TODO: pass in content from caller or different dialog types
            ContentDialog deleteDialog = new ContentDialog
            {
                Title = "Delete Confirmation",
                Content = "This action cannot be undone. Are you sure you want to delete?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteDialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }

        public async Task ShowFirstRunDialogAsync()
        {
            ContentDialog deleteDialog = new ContentDialog
            {
                Title = "Welcome to ChatTailor AI",
                Content = "To get started, go to Settings and enter your OpenAI, Anthropic, or Google API Key.",
                CloseButtonText = "Close"
            };

            await deleteDialog.ShowAsync();
        }

        public async Task ShowAppUpdatedDialogAsync()
        {
            var dialog = new AppUpdatedDialog();
            await dialog.ShowAsync();
        }

        public async Task ShowCreateAssistantDialogAsync()
        {
            var dialog = new CreateAssistantDialog();
            await dialog.ShowAsync();
        }

        public async Task ShowNewChatDialogAsync()
        {
            var dialog = new NewChatDialog();
            await dialog.ShowAsync();
        }

        public async Task<bool> ShowEditAssistantDialogAsync(AssistantDto assistant)
        {
            var viewModel = new EditAssistantDialogViewModel
            {
                CurrentAssistant = assistant
            };

            var dialog = new EditAssistantDialog
            {
                DataContext = viewModel
            };

            ContentDialogResult result = await dialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }

        public async Task<bool> ShowEditChatDialogAsync(ConversationDto conversation)
        {
            var viewModel = _editChatDialogViewModelFactory.Create(conversation);

            var dialog = new EditChatDialog
            {
                DataContext = viewModel
            };

            ContentDialogResult result = await dialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }

        public async Task<bool> ShowEditPromptDialogAsync(PromptDto prompt)
        {
            var viewModel = new EditPromptDialogViewModel
            {
                CurrentPrompt = prompt
            };

            var dialog = new EditPromptDialog
            {
                DataContext = viewModel
            };

            ContentDialogResult result = await dialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }

        public async Task ShowAssistantHelpDialog()
        {
            var dialog = new AssistantHelpDialog();
            await dialog.ShowAsync();
        }

        public async Task ShowChatHelpDialog()
        {
            var dialog = new ChatHelpDialog();
            await dialog.ShowAsync();
        }

        public async Task ShowGenerateImageDialogAsync()
        {
            var dialgo = new NewImageDialog();
            await dialgo.ShowAsync();
        }

        public async Task ShowCreatePromptDialogAsync()
        {
            var dialog = new NewPromptDialog();
            await dialog.ShowAsync();
        }

        public async Task ShowEditPromptDialogAsync()
        {
            var dialog = new EditPromptDialog();
            await dialog.ShowAsync();
        }
    }
}