using Microsoft.AspNetCore.Identity;

namespace IdentityVersion2.Entities
{
    public class AppRole:IdentityRole<int>//<int> ile primary key int oldu , varcahr yerine 
    {
        public DateTime CreatedTime { get; set; }
    }
}
