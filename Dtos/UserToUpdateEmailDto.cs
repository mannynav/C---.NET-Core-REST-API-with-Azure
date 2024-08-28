using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAPI.Dtos
{
    public partial class UserToUpdateEmailDto
    {
        public int UserId { get; set; }
        public required string Email { get; set; }

    }
}
