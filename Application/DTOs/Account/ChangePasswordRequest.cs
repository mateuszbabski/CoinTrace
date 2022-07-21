using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Account
{
    public class ChangePasswordRequest
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }
        [Required]
        [MinLength(6)]
        public string ConfirmNewPassword { get; set; }
    }
}
