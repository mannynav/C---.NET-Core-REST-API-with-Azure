using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAPI.Dtos
{
    public partial class UserToUpdateLastNameDto : UserUpdateDto
    {
        public required string LastName { get; set; }

    }
}
