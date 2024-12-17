using IcaBank.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System;

namespace IcaBank.Models.DTOs
{
    public class UpdateAccountDTO
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "O Pin não pode ter mais de 4 digitos.")]
        public string Pin { get; set; }
        [Required]
        [Compare("Pin", ErrorMessage = "O confirmação do pin não é igual ao outro.")]
        public string ConfirmPin { get; set; }
        public DateTime DateLastUpdated { get; set; }
    }
}
