using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Entity.User
{
    public class AppUser : IdentityUser<Guid>
    {
        public override Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public override string PasswordHash { get; set; } = string.Empty;
        public bool IsConfirmed { get; set; } = false;

    }
}
