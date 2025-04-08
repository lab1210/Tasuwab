using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMFApplication.DTOs.Transaction
{
    public class CreateTransactionChargeDto
    {
        [Required]
        public string TransactionType { get; set; } // "Deposit", "Withdrawal"

        [Required]
        public string ChargeType { get; set; } // "Fixed", "Percentage"

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Value { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal MinAmount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MaxAmount { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
    }

    public class UpdateTransactionChargeDto
    {
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Value { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }

    public class TransactionChargeDto
    {
        public int Id { get; set; }
        public string TransactionType { get; set; }
        public string ChargeType { get; set; }
        public decimal Value { get; set; }
        public decimal MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
    public class CreateTransactionDto
    {
        public string AccountCode { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public string PerformedBy { get; set; } = "system";
    }
    public class TransactionDto
    {
        public int Id { get; set; }
        public string AccountCode { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public decimal Charges { get; set; }
        public decimal FinalAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Reference { get; set; }
    }

}
