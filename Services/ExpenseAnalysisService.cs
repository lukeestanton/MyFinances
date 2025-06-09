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

        public (double[] Data, string[] Labels) GetMonthlySpendingTrend(List<Expense> expenses)
        {
            if (!expenses.Any())
                return (Array.Empty<double>(), Array.Empty<string>());

            var mostRecentDate = expenses.Max(e => e.Date);
            var sixMonthsAgo = mostRecentDate.AddMonths(-5); // -5 to include current month
            var monthlyTotals = new Dictionary<DateTime, decimal>();

            // Initialize the last 6 months with zero values
            for (int i = 0; i < 6; i++)
            {
                var date = mostRecentDate.AddMonths(-i);
                monthlyTotals[new DateTime(date.Year, date.Month, 1)] = 0;
            }

            // Sum up expenses for each month
            foreach (var expense in expenses.Where(e => e.Date >= sixMonthsAgo))
            {
                var monthStart = new DateTime(expense.Date.Year, expense.Date.Month, 1);
                if (monthlyTotals.ContainsKey(monthStart))
                {
                    monthlyTotals[monthStart] += expense.Amount;
                }
            }

            // Sort by date ascending
            var sortedMonths = monthlyTotals.OrderBy(x => x.Key).ToList();

            // Convert to arrays for the chart
            var data = new double[sortedMonths.Count];
            var labels = new string[sortedMonths.Count];

            for (int i = 0; i < sortedMonths.Count; i++)
            {
                data[i] = (double)sortedMonths[i].Value;
                labels[i] = sortedMonths[i].Key.ToString("MMM yyyy");
            }

            return (data, labels);
        }

        public (double[] Data, string[] Labels) GetProductivityDistributionData(List<Expense> expenses)
        {
            var productiveCategories = new HashSet<ExpenseCategory>
            {
                ExpenseCategory.Tuition,
                ExpenseCategory.Books,
                ExpenseCategory.Courses,
                ExpenseCategory.Healthcare,
                ExpenseCategory.MedicalInsurance,
                ExpenseCategory.Dental,
                ExpenseCategory.Vision,
                ExpenseCategory.Prescriptions,
                ExpenseCategory.Investments,
                ExpenseCategory.Savings,
                ExpenseCategory.BusinessExpenses,
                ExpenseCategory.OfficeSupplies,
                ExpenseCategory.ProfessionalServices,
                ExpenseCategory.Software,
                ExpenseCategory.Electronics,
                ExpenseCategory.HomeImprovement
            };

            decimal productiveTotal = 0;
            decimal unproductiveTotal = 0;

            foreach (var expense in expenses)
            {
                if (productiveCategories.Contains(expense.Category))
                {
                    productiveTotal += expense.Amount;
                }
                else
                {
                    unproductiveTotal += expense.Amount;
                }
            }

            var data = new double[] { (double)productiveTotal, (double)unproductiveTotal };
            var labels = new string[] 
            { 
                $"Productive (${productiveTotal:N2})", 
                $"Unproductive (${unproductiveTotal:N2})" 
            };

            return (data, labels);
        }

        public (double[] Data, string[] Labels) GetLargestExpensesData(List<Expense> expenses)
        {
            if (!expenses.Any())
                return (Array.Empty<double>(), Array.Empty<string>());

            var mostRecentDate = expenses.Max(e => e.Date);
            var sixMonthsAgo = mostRecentDate.AddMonths(-5);

            var largestExpenses = expenses
                .Where(e => e.Date >= sixMonthsAgo)
                .OrderByDescending(e => e.Amount)
                .Take(5)
                .ToList();

            var data = largestExpenses.Select(e => (double)e.Amount).ToArray();
            var labels = largestExpenses.Select(e => 
                $"{e.Description} ({e.Date:MMM d})").ToArray();

            return (data, labels);
        }
    }
} 