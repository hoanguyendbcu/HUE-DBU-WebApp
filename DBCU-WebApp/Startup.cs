using DBCU_WebApp.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace DBCU_WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();           // Đăng ký dịch vụ lưu cache trong bộ nhớ (Session sẽ sử dụng nó)
            services.AddSession(cfg =>
            {                    // Đăng ký dịch vụ Session
                cfg.Cookie.Name = "DBCUWeb";             // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
                cfg.IdleTimeout = new TimeSpan(0, 30, 0);    // Thời gian tồn tại của Session
            });

            //// Truy cập IdentityOptions
            //services.Configure<IdentityOptions>(options => {
            //    // Thiết lập về Password
            //    options.Password.RequireDigit = false; // Không bắt phải có số
            //    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
            //    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
            //    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
            //    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
            //    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

            //    // Cấu hình Lockout - khóa user
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
            //    options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
            //    options.Lockout.AllowedForNewUsers = true;

            //    // Cấu hình về User.
            //    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
            //        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            //    options.User.RequireUniqueEmail = true; // Email là duy nhất

            //    // Cấu hình đăng nhập.
            //    options.SignIn.RequireConfirmedEmail = true; // Cấu hình xác thực địa chỉ email (email phải tồn tại)
            //    options.SignIn.RequireConfirmedPhoneNumber = false; // Xác thực số điện thoại

            //});

            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                // Trên 5 giây truy cập lại sẽ nạp lại thông tin User (Role)
                // SecurityStamp trong bảng User đổi -> nạp lại thông tinn Security
                options.ValidationInterval = TimeSpan.FromSeconds(5);
            });

            services.Configure<RouteOptions>(options =>
            {
                options.AppendTrailingSlash = false; // Thêm dấu / vào cuối URL
                options.LowercaseUrls = true; // url chữ thường
                options.LowercaseQueryStrings = false; // không bắt query trong url phải in thường
            });

            services.AddOptions(); // Kích hoạt Options
            var mailsettings = Configuration.GetSection("MailSettings"); // đọc config
            services.Configure<MailSettings>(mailsettings); // đăng ký để Inject

            services.AddTransient<IEmailSender, SendMailService>(); // Đăng ký dịch vụ Mail

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddSingleton<LocalizationService>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName = new AssemblyName(typeof(ApplicationResource).GetTypeInfo().Assembly.FullName);
                        return factory.Create("ApplicationResource", assemblyName.Name);
                    };
                });
            services.AddRazorPages();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("vi"),
                };

                // State what the default culture for your application is. This will be used if no specific culture
                // can be determined for a given request.
                options.DefaultRequestCulture = new RequestCulture("en");

                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting numbers, dates, etc.
                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
                options.SupportedUICultures = supportedCultures;
            });
            
            services.AddDbContextPool<DBCU_WebContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DBCUContextConnection")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                  .AddRoles<IdentityRole>()
                  .AddEntityFrameworkStores<DBCU_WebContext>();

            services.AddSingleton<IConfiguration>(Configuration);
            // Cấu hình Cookie
            services.ConfigureApplicationCookie(options =>
            {
                // options.Cookie.HttpOnly = true;  
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = $"/Identity/Account/login/";                                 // Url đến trang đăng nhập
                options.LogoutPath = $"/Identity/Account/logout/";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";   // Trang khi User bị cấm truy cập
            });

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            //services.AddDbContext<DBCU_WebContext>(options =>
            //        options.UseSqlServer(Configuration.GetConnectionString("DBCU_WebContext")));


            services.AddAuthorization(options =>
            {
                // User thỏa mãn policy khi có roleclaim: permission với giá trị manage.user
                options.AddPolicy("AdminDropdown", policy =>
                {
                    policy.RequireClaim("permission", "manage.user");
                });

            });

            services.AddAuthentication();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();         // Register Middleware Session to Pipeline   

            app.UseRouting();

            var requestlocalizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(requestlocalizationOptions.Value);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "MyArea",
                   pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                 name: "learnasproute", // đặt tên route
                 defaults: new { controller = "LearnAsp", action = "Index" },
                 pattern: "learn-asp-net/{id:int?}");

                endpoints.MapRazorPages();
            });

            app.Map("/testapi", app =>
            {
                app.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    var ob = new
                    {
                        url = context.Request.GetDisplayUrl(),
                        content = "Return from testapi"
                    };
                    string jsonString = JsonConvert.SerializeObject(ob);
                    await context.Response.WriteAsync(jsonString, Encoding.UTF8);
                });
            });

            app.Run(async (HttpContext context) =>
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("Page not found (DBCU Web)!");
            });
        }
    }
}
