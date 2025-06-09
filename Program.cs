using MyFinances;
using MudBlazor.Services;
using MyFinances.Services;
using MyFinances.Components;
using MyFinances.Models;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();
builder.Services.AddScoped<ExpenseCategorizationService>();
builder.Services.AddScoped<ExpenseAnalysisService>();
builder.Services.AddScoped<IExpenseClient, ExpenseClient>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Add sample expenses if the database is empty
using (var scope = app.Services.CreateScope())
{
    var expenseClient = scope.ServiceProvider.GetRequiredService<IExpenseClient>();
    var expenses = await expenseClient.GetExpensesAsync();
    
    if (!expenses.Any())
    {
        var sampleExpenses = new List<Expense>
        {
            new Expense { Description = "Sample Expense", Amount = 1500.00m, Date = DateTime.Today.AddDays(-5), Category = ExpenseCategory.Other }
        };

        foreach (var expense in sampleExpenses)
        {
            await expenseClient.AddExpenseAsync(expense);
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
