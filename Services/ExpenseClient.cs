using MyFinances.Models;

namespace MyFinances.Services;

public interface IExpenseClient
{
    Task<List<Expense>> GetExpensesAsync();
    Task AddExpenseAsync(Expense expense);
    Task DeleteExpenseAsync(Expense expense);
}

public class ExpenseClient : IExpenseClient
{
    private List<Expense> _expenses;

    public ExpenseClient()
    {
        // Initialize with sample expenses
        _expenses = new List<Expense>
        {
            new() { Description = "Netflix Subscription", Amount = 15.99m, Date = DateTime.Now.AddDays(-2) },
            new() { Description = "Walmart Groceries", Amount = 85.50m, Date = DateTime.Now.AddDays(-1) },
            new() { Description = "Shell Gas Station", Amount = 45.00m, Date = DateTime.Now.AddDays(-3) },
            new() { Description = "Starbucks Coffee", Amount = 4.95m, Date = DateTime.Now },
            new() { Description = "Amazon Prime", Amount = 14.99m, Date = DateTime.Now.AddDays(-5) },
            new() { Description = "Electric Bill", Amount = 120.00m, Date = DateTime.Now.AddDays(-7) },
            new() { Description = "Gym Membership", Amount = 29.99m, Date = DateTime.Now.AddDays(-4) },
            new() { Description = "Spotify Premium", Amount = 9.99m, Date = DateTime.Now.AddDays(-6) }
        };
    }

    public Task<List<Expense>> GetExpensesAsync()
    {
        return Task.FromResult(_expenses);
    }

    public Task AddExpenseAsync(Expense expense)
    {
        _expenses.Add(expense);
        return Task.CompletedTask;
    }

    public Task DeleteExpenseAsync(Expense expense)
    {
        _expenses.Remove(expense);
        return Task.CompletedTask;
    }
} 