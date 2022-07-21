using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Account
{
    public class ChangePasswordResponse
    {
        public bool IsSuccess { get; set; }
        public string Password { get; set; }
        public string Errors { get; set; }
    }
}
