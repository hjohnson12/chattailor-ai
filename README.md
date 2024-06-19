# ChatTailor AI

A C# desktop application using the Universal Windows Platform (UWP), used to interact with LLM's such as GPT, Claude, or Gemini for chat interactions and DALL-E for AI image generation. Available on the [Microsoft Store](https://apps.microsoft.com/detail/9pjrf3zz3khk?hl=en-US&gl=US) and requires an API key for the specific LLM provider

[![.NET CI Build](https://github.com/hjohnson12/chattailor-ai/actions/workflows/main.yml/badge.svg?branch=feature%2Fv2)](https://github.com/hjohnson12/chattailor-ai/actions/workflows/main.yml)

## Features

Click below to view a list of features in ChatTailor AI

<details>
  <summary>View</summary>


- **API Integration**: Simplified setup requiring only an API key to connect with OpenAI, Anthropic, or Google AI models.

- **Customizable Interactions**: Users can configure the system messages, choose between streaming or complete responses, and adjust AI behavior through detailed parameter settings such as max tokens, temperature, and penalties.

- **Image Generation**: Incorporates DALLE2 and DALLE3 for generating images directly from textual prompts.

- **Vision AI Support**: Features include the ability to upload and analyze images using models like gpt-4-vision-preview and Claude-3.

- **AI Assistants**: Utilizes OpenAI's Assistants API to create and manage custom AI assistants.

- **Conversation Management**: Supports creating and retaining detailed conversation histories, allowing for persistent chat sessions.

- **Prompts Management**: Users can store and manage commonly used prompts to facilitate repeated use.

- **Display Options**: Supports various display configurations, including full screen, picture-in-picture, and movable window modes.

- **Learning Tools**: Allows users to experiment with model parameters, aiding in educational purposes and deeper understanding of AI functionalities.

- **Chat Customization**: Provides tools for managing chat context limits, including manual curation of message retention.

- **Import/Export Functions**: Supports importing and exporting conversation data in text format for backup or analysis.

- **Text-to-Speech and Speech-to-Text**: Integration with Azure speech services to convert AI responses into speech and vice versa.

- **Continuous Improvement**: Continuous updates to add new features such as markdown support and more efficient conversation storage.

- **Spotify Integration**: Enables control of Spotify functionalities directly through the application.

- **Voice Selection**: Offers a wide range of voice options for text-to-speech, including over 400 choices from Azure and Eleven Labs, with support for custom cloned voices.

</details>


## Architecture

Utilizes a layered architecture to separate concerns and enhance maintainability, mainly following form of Model-View-View-Model (MVVM) and Service Oriented Architecture (SOA). Below is a description of each layer as represented in the project's directory structure:

- **ChatTailorAI.DataAccess**: This layer is responsible for all database interactions. It includes data models, data access logic, and repositories which facilitate the interaction with the underlying database.

- **ChatTailorAI.Migrations**: Manages database schema migrations, ensuring that changes to the database structure are versioned and applied consistently, facilitating updates and deployment across different environments.

- **ChatTailorAI.Services**: Contains the business logic of the application. This layer processes data retrieved from the DataAccess layer and applies business rules. It acts as a middleman between the presentation and the data persistence layers.

- **ChatTailorAI.Services.Uwp**: Specifically for Universal Windows Platform (UWP) applications, these handle business logic that might have specific dependencies or requirements unique to UWP.

- **ChatTailorAI.Shared**: This layer serves as a central hub for shared resources, including utilities, constants, common functions, view models, and service interfaces. The view models provide a data structure for the UI elements, encapsulating the presentation logic but without specific business logic implementations. Service interfaces define the contracts for the services which are implemented in the **ChatTailorAI.Services** layer.

- **ChatTailorAI.Uwp**: Represents the presentation layer specifically for UWP. This includes user interface components and the logic needed to handle user interactions within the UWP framework.

## Contributing

### Reporting Bugs

Found a bug? Open an issue on GitHub with a clear description of the problem and how to reproduce it.

### Suggesting Features

Have an idea for a new feature? Open an issue on GitHub with a detailed description of your suggestion and why it would be useful.

### Submitting Pull Requests

Want to contribute code? Awesome! Here’s how to get started:
1. **Fork the repository**.
2. **Clone your fork**: `git clone https://github.com/your-username/chattailor-ai.git`
3. **Create a branch**: `git checkout -b feature-or-bugfix-name`
4. **Make your changes**.
5. **Commit your changes**: `git commit -m "Description of the feature or fix"`
6. **Push to your branch**: `git push origin feature-or-bugfix-name`
7. **Create a pull request**.

If possible, please try to stick with the project's architecture and code style when making changes. 

## Ethical Use Guidelines

We are committed to promoting ethical use of AI technologies. Here are our guidelines to ensure responsible usage:

- **Transparency**: Always disclose AI involvement in communications and outputs.

- **Respect Privacy**: Do not use the AI to process personal data without consent.

- **Avoid Misuse**: Do not use the AI for generating misleading, harmful, or illegal content.

- **Be Mindful of Bias**: Be aware of potential biases in AI outputs and strive to minimize their impact.

- **Promote Positive Impact**: Use AI to support beneficial and innovative applications that improve human wellbeing.

- **Adhere to Laws**: Comply with all applicable laws and regulations regarding AI usage in your jurisdiction.

By using ChatTailor AI, you agree to uphold these principles and encourage ethical practices in all your AI interactions.
