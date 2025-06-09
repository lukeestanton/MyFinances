using System;

namespace MyFinances.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public ExpenseCategory Category { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Location { get; set; }
    }
} 