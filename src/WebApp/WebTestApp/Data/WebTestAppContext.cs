using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebTestApp.Controllers;

namespace WebTestApp.Data
{
    public class WebTestAppContext : DbContext
    {
        public WebTestAppContext (DbContextOptions<WebTestAppContext> options)
            : base(options)
        {
        }

        public DbSet<WebTestApp.Controllers.User> User { get; set; } = default!;
    }
}
