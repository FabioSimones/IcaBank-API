﻿using IcaBank.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IcaBank.Models
{
    [Table("Accounts")]
    public class Account
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
        [JsonIgnore]
        public byte[] PinHash { get; set; }
        [JsonIgnore]
        public byte[] PinSalt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }


        Random rand = new Random();

        public Account()
        {
            AccountNumberGenerated = Convert.ToString((long)Math.Floor(rand.NextDouble() * 9_000_000_000L + 1_000_000_000L));
            AccountName = $"{FirstName} {LastName}";
        }
    }    
}
