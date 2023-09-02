using Microsoft.AspNetCore.Identity;

namespace Authentication.api.Models
{
    public class DWUser : IdentityUser
    {
        public string Badge { get; set; }
        public string Position { get; set; }
    }
}
