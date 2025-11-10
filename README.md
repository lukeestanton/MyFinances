# MyFinances

MyFinances is a .NET 9 Blazor Server application that provides a modern dashboard for tracking personal spending. It uses [MudBlazor](https://mudblazor.com/) components for a polished UI, stores expenses locally with LiteDB, and can automatically categorize new expenses with the Azure OpenAI service.

## Features

- **Expense dashboard** – Review totals for the past six months, the top categories you spend in, and quick stats such as productive vs. unproductive spending from interactive MudBlazor charts.
- **Expense management** – Add and delete expenses with inline validation. Records are persisted in a local `expenses.db` LiteDB database so they survive restarts.
- **Automated categorization** – New expenses are categorized by calling Azure OpenAI (using the `gpt-3.5-turbo` deployment) when an API key is provided, with safe fallbacks if the service is unavailable.
- **Sample data seeding** – The app seeds a starter expense when the database is empty so charts render immediately on first launch.

## Getting started

### Prerequisites

- [.NET SDK 9.0](https://dotnet.microsoft.com/download)
- (Optional) An Azure OpenAI resource with a `gpt-3.5-turbo` deployment if you want automatic categorization.

### Installation

1. Restore dependencies:
   ```bash
   dotnet restore
   ```
2. (Optional) Export your Azure OpenAI API key so categorization can run:
   ```bash
   export OPENAI_API_KEY="<your-key>"
   ```

### Run the app

From the repository root, run:
```bash
dotnet run
```
The development server listens on the URL shown in the console (typically `https://localhost:5001`).

The first run creates an `expenses.db` file in the project root that contains expense documents in LiteDB format. Delete this file if you want to reset the data.

## Project structure

- `Program.cs` – Configures Blazor Server, registers services, and seeds sample data.
- `Components/` – Razor components, including the MudBlazor layouts and the expenses dashboard page.
- `Services/` – Application services for categorization, analytics, and LiteDB persistence.
- `Models/` – Expense and category domain models.

## Troubleshooting

- **Categorization errors** – If `OPENAI_API_KEY` is not set or the Azure OpenAI call fails, new expenses are still saved but default to the `Other` category. Check the server console for error messages.
- **Database locked or corrupted** – Stop the app and delete `expenses.db` to recreate a clean database.
