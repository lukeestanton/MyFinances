using LiteDB;
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
    private readonly string _dbPath = "expenses.db";
    private const string CollectionName = "expenses";

    public Task<List<Expense>> GetExpensesAsync()
    {
        using var db = new LiteDatabase(_dbPath);
        var col = db.GetCollection<Expense>(CollectionName);
        return Task.FromResult(col.FindAll().ToList());
    }

    public Task AddExpenseAsync(Expense expense)
    {
        using var db = new LiteDatabase(_dbPath);
        var col = db.GetCollection<Expense>(CollectionName);
        col.Insert(expense);
        return Task.CompletedTask;
    }

    public Task DeleteExpenseAsync(Expense expense)
    {
        using var db = new LiteDatabase(_dbPath);
        var col = db.GetCollection<Expense>(CollectionName);
        col.DeleteMany(e => e.Description == expense.Description && e.Amount == expense.Amount && e.Date == expense.Date);
        return Task.CompletedTask;
    }
}
