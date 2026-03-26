using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResearchProjectManagementSystem.Models
{
    [Table("Budgets")]
    public class BudgetModel
    {
        [Key]
        [Column("Id")]
        public int IdBudget { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public string Currency { get; set; }
    }
}
