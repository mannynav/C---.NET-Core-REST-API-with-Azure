using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAPI.Dtos
{
    public partial class UserToUpdateEmailDto : UserUpdateDto
    {
        public required string Email { get; set; }

    }
}
