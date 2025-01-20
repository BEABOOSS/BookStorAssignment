using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace BookStore.Models
{
    public class UserIdentity : IdentityUser
    {


        public byte Permission { get; set; } = (Byte)Permissions.Admin;

    }

    enum Permissions : byte
    {
        [Description("Has access to level 1 permission")]
        Customer = 1,
        [Description("Has access to level 2 permission")]
        Employee = 2,
        [Description("Has access to level 3 permission")]
        Manager = 3,
        [Description("Has access to level 4 permission")]
        Admin = 4,

    }
}
