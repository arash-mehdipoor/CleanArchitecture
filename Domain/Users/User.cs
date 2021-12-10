using Domain.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    [Audit]
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
    }
}
