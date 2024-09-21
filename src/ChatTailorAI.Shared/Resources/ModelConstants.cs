using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Resources
{
    public static class ModelConstants
    {
        public static readonly string[] DefaultChatModels = new string[]
        {
            "gpt-3.5-turbo",
            "gpt-3.5-turbo-0613",
            "gpt-3.5-turbo-16k",
            "gpt-3.5-turbo-16k-0613",
            "gpt-3.5-turbo-1106",
            "gpt-3.5-turbo-0125",
            "gpt-4",
            "gpt-4-0613",
            "gpt-4-0314",
            "gpt-4-1106-preview",
            "gpt-4-0125-preview",
            "gpt-4-turbo-preview",
            "gpt-4-vision-preview",
            "gpt-4-turbo",
            "gpt-4-turbo-2024-04-09",
            "gpt-4o",
            "gpt-4o-2024-05-13",
            "gpt-4o-mini",
            "gpt-4o-2024-08-06",
            "chatgpt-4o-latest",
            "o1-preview",
            "o1-preview-2024-09-12",
            "o1-mini",
            "o1-mini-2024-09-12",
            "claude-3-5-sonnet-20240620",
            "claude-3-opus-20240229",
            "claude-3-sonnet-20240229",
            "claude-3-haiku-20240307",
            "claude-2.1",
            "claude-instant-1.2",
            "gemini-pro",
            "gemini-1.0-pro",
            "gemini-1.0-pro-001",
            "gemini-1.0-pro-latest",
            "gemini-1.5-pro-latest",
            "gemini-1.5-flash-latest"
            // Uncomment when functionality to support non multi-turn conversations is added
            // since the vision model for gemini only supports text + image input and not multi-turn conversations
            //"gemini-pro-vision"
        };

        public static readonly string[] AssistantChatModels = new string[]
        {
            "gpt-3.5-turbo",
            "gpt-3.5-turbo-0613",
            "gpt-3.5-turbo-16k",
            "gpt-3.5-turbo-16k-0613",
            "gpt-3.5-turbo-1106",
            "gpt-3.5-turbo-0125",
            "gpt-4",
            "gpt-4-0613",
            "gpt-4-0314",
            "gpt-4-1106-preview",
            "gpt-4-0125-preview",
            "gpt-4-turbo-preview",
            "gpt-4o",
            "gpt-4o-2024-05-13",
            "gpt-4o-mini",
            "gpt-4o-2024-08-06",
        };

        public static readonly string[] VisionModels = new string[]
        {
            "gpt-4-vision-preview",
            "claude-3-sonnet-20240229",
            "claude-3-opus-20240229",
            "claude-3-haiku-20240307",
            "claude-3-5-sonnet-20240620",
            "gpt-4-turbo",
            "gpt-4-turbo-2024-04-09",
            "gpt-4o",
            "gpt-4o-2024-05-13",
            "gpt-4o-mini",
            "gpt-4o-2024-08-06",
        };

        public static readonly string[] ImageModels = new string[]
        {
            "dall-e-3",
            "dall-e-2"
        };

        public static readonly string[] ModelsWithoutFunctionSupport = new string[]
        {
            "gpt-4-vision-preview",
            "chatgpt-4o-latest",
            "o1-preview",
            "o1-preview-2024-09-12",
            "o1-mini",
            "o1-mini-2024-09-12",
        };

        public static readonly string[] ModelsWithoutStreamingSupport = new string[]
        {
            "o1-preview",
            "o1-preview-2024-09-12",
            "o1-mini",
            "o1-mini-2024-09-12"
        };
    }
}