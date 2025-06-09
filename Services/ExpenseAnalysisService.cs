using MyFinances.Models;

namespace MyFinances.Services
{
    public class ExpenseAnalysisService
    {
        public class CategoryGroupData
        {
            public string Group { get; set; } = string.Empty;
            public decimal TotalAmount { get; set; }
        }

        public string GetCategoryGroup(ExpenseCategory category)
        {
            return category switch
            {
                ExpenseCategory.Rent or ExpenseCategory.Mortgage or ExpenseCategory.PropertyTax or
                ExpenseCategory.HomeInsurance or ExpenseCategory.Utilities or ExpenseCategory.HomeMaintenance or
                ExpenseCategory.HomeImprovement or ExpenseCategory.Furniture => "Housing",

                ExpenseCategory.CarPayment or ExpenseCategory.CarInsurance or ExpenseCategory.Gas or
                ExpenseCategory.PublicTransportation or ExpenseCategory.CarMaintenance or ExpenseCategory.Parking or
                ExpenseCategory.Tolls => "Transportation",

                ExpenseCategory.Groceries or ExpenseCategory.DiningOut or ExpenseCategory.CoffeeShops or
                ExpenseCategory.FastFood => "Food",

                ExpenseCategory.Clothing or ExpenseCategory.PersonalGrooming or ExpenseCategory.Healthcare or
                ExpenseCategory.MedicalInsurance or ExpenseCategory.Dental or ExpenseCategory.Vision or
                ExpenseCategory.Prescriptions => "Personal Care",

                ExpenseCategory.Movies or ExpenseCategory.Concerts or ExpenseCategory.Subscriptions or
                ExpenseCategory.Hobbies or ExpenseCategory.Sports or ExpenseCategory.Travel => "Entertainment",

                ExpenseCategory.Phone or ExpenseCategory.Internet or ExpenseCategory.Software or
                ExpenseCategory.Electronics => "Technology",

                ExpenseCategory.Tuition or ExpenseCategory.Books or ExpenseCategory.StudentLoans or
                ExpenseCategory.Courses => "Education",

                ExpenseCategory.CreditCardPayments or ExpenseCategory.Loans or ExpenseCategory.Investments or
                ExpenseCategory.Savings => "Financial",

                ExpenseCategory.BusinessExpenses or ExpenseCategory.OfficeSupplies or
                ExpenseCategory.ProfessionalServices => "Business",

                _ => "Miscellaneous"
            };
        }

        public (double[] Data, string[] Labels) GetCategoryDistributionData(List<Expense> expenses)
        {
            var categoryGroups = new Dictionary<string, decimal>();

            // Group expenses by category and sum their amounts
            foreach (var expense in expenses)
            {
                string group = GetCategoryGroup(expense.Category);
                if (categoryGroups.ContainsKey(group))
                {
                    categoryGroups[group] += expense.Amount;
                }
                else
                {
                    categoryGroups[group] = expense.Amount;
                }
            }

            // Sort groups by total amount in descending order
            var sortedGroups = categoryGroups.OrderByDescending(x => x.Value).ToList();

            // Convert to arrays for the chart
            var data = new double[sortedGroups.Count];
            var labels = new string[sortedGroups.Count];

            for (int i = 0; i < sortedGroups.Count; i++)
            {
                data[i] = (double)sortedGroups[i].Value;
                labels[i] = $"{sortedGroups[i].Key} (${sortedGroups[i].Value:N2})";
            }

            return (data, labels);
        }
    }
} 