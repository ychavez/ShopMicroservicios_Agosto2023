using Authentication.api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.api.Context
{
    public class AccountDbContext: IdentityDbContext<DWUser>
    {
        public AccountDbContext(DbContextOptions options):base(options)
        {

        }
    }
}
