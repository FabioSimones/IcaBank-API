using IcaBank.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System;

namespace IcaBank.Models.DTOs
{
    public class TransactionRequestDTO
    {
        public decimal TransactionAmount { get; set; }
        public string TransactionSourceAccount { get; set; }
        public string TransactionDestinationAccount { get; set; }
        public ETransType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
