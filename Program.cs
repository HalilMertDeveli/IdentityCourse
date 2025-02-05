using IdentityVersion2.Context;
using IdentityVersion2.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace IdentityVersion2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<UdemyContext>(opt =>
            {
                opt.UseSqlServer("server=localhost; database=IdentityDatabase; integrated security=true;TrustServerCertificate=true");

            });
            builder.Services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredLength = 1;
                opt.Password.RequireNonAlphanumeric = false;
                opt.SignIn.RequireConfirmedEmail = false;

            }).AddEntityFrameworkStores<UdemyContext>();//1

            builder.Services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.HttpOnly = true;//java script ile alamasın 
                opt.Cookie.SameSite = SameSiteMode.Strict;//sadece site kullanabilsin 
                //opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;//sadece https 'de çalışdı
                opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;//hemp https hem de http 'de çalışsın
                opt.Cookie.Name = "HMDCookie";
                opt.ExpireTimeSpan=TimeSpan.FromDays(25);//25 gün sonra tarayıcıdan silinsin
                opt.LoginPath = new PathString("/Home/SingIn");//Hata durumunda kullanıcının nereye gideceğini veriyoruz/Home/AdminPanel, 


            });

            var app = builder.Build();

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/node_modules",
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"node_modules")),

            });
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthorization();
            

            app.MapControllerRoute(  
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
           
            app.Run();
        }
    }
}
