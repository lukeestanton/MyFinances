using System;
using System.Threading.Tasks;
using Azure;
using Azure.AI.OpenAI;
using MyFinances.Models;

namespace MyFinances.Services
{
    public class ExpenseCategorizationService
    {
        private static readonly string[] _categories = Enum.GetNames(typeof(ExpenseCategory));
        private readonly OpenAIClient _openAIClient;

        public ExpenseCategorizationService()
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("OPENAI_API_KEY environment variable is not set");
            }
            _openAIClient = new OpenAIClient(apiKey, new OpenAIClientOptions());
        }

        public async Task<ExpenseCategory> CategorizeExpenseAsync(Expense expense)
        {
            try
            {
                var prompt = $@"Given the following expense description: '{expense.Description}', categorize it into one of these EXACT categories: {string.Join(", ", _categories)}.

                Categories are organized as follows:
                - Housing: Rent, Mortgage, PropertyTax, HomeInsurance, Utilities, HomeMaintenance, HomeImprovement, Furniture
                - Transportation: CarPayment, CarInsurance, Gas, PublicTransportation, CarMaintenance, Parking, Tolls
                - Food: Groceries, DiningOut, CoffeeShops, FastFood
                - Personal Care: Clothing, PersonalGrooming, Healthcare, MedicalInsurance, Dental, Vision, Prescriptions
                - Entertainment: Movies, Concerts, Subscriptions, Hobbies, Sports, Travel
                - Technology: Phone, Internet, Software, Electronics
                - Education: Tuition, Books, StudentLoans, Courses
                - Financial: CreditCardPayments, Loans, Investments, Savings
                - Business: BusinessExpenses, OfficeSupplies, ProfessionalServices
                - Miscellaneous: Gifts, CharitableDonations, PetExpenses, Childcare, Other

                IMPORTANT: Return ONLY the exact category name from the list above. Do not return group names or any other text.
                For example:
                - For 'Netflix subscription' return 'Subscriptions'
                - For 'Walmart groceries' return 'Groceries'
                - For 'Shell gas station' return 'Gas'
                - For 'Nintendo Switch game' return 'Hobbies'
                - For 'Starbucks coffee' return 'CoffeeShops'";

                var chatCompletionsOptions = new ChatCompletionsOptions()
                {
                    Messages =
                    {
                        new ChatRequestUserMessage("You are a financial assistant that categorizes expenses. You must return ONLY the exact category name from the provided list. Do not return group names or any other text."),
                        new ChatRequestUserMessage(prompt)
                    },
                    MaxTokens = 50,
                    Temperature = 0.1f,
                    DeploymentName = "gpt-3.5-turbo"
                };

                var response = await _openAIClient.GetChatCompletionsAsync(chatCompletionsOptions);
                var categoryName = response.Value.Choices[0].Message.Content.Trim();

                if (Enum.TryParse<ExpenseCategory>(categoryName, true, out var category))
                {
                    return category;
                }

                return ExpenseCategory.Other;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ExpenseCategory.Other;
            }
        }
    }
} 