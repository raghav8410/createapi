using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Registration;

namespace RegistrationApi.Models
{
    public class RegistrationApiContext : DbContext
    {
        public RegistrationApiContext (DbContextOptions<RegistrationApiContext> options)
            : base(options)
        {
        }

        public DbSet<Registration.RegisteredUser> RegisteredUser { get; set; }
    }
}
