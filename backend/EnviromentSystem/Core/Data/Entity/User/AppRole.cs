using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Entity.User
{
    public class AppRole : IdentityRole<Guid>
    {
        public override Guid Id { get; set; } = Guid.NewGuid();
        public override string Name { get; set; } = string.Empty;
        public override string NormalizedName { get; set; } = string.Empty;

    }
}
