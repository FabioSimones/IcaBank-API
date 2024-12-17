using IcaBank.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System;

namespace IcaBank.Models.DTOs
{
    public class GetAccountDTO
    {
        [Key]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal CurrentAccountBalance { get; set; }
        public EAccountType AccountType { get; set; }
        public string AccountNumberGenerated { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }
    }
}
