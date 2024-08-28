using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAPI.Dtos
{
    public partial class UserToUpdateActiveDto : UserUpdateDto
    {
        public required bool Active { get; set; }

    }
}
