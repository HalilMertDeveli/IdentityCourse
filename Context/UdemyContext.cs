using IdentityVersion2.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityVersion2.Context
{
    public class UdemyContext:IdentityDbContext<AppUser,AppRole,int>//artık benim yazdıklarımı kullanacaksın
    {
        public UdemyContext(DbContextOptions<UdemyContext> options) :base(options)
        {
                
        }
    }
}
