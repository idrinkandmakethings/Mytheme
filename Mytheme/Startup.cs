using System.Threading.Tasks;
using Blazor.FileReader;
using BlazorStyled;
using BlazorTypography;
using ElectronNET.API;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mytheme.Dal;
using Mytheme.Data;
using Mytheme.Modal;


namespace Mytheme
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            using var db = new DataStorage();
            db.Database.Migrate();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBlazorStyled();
            services.AddTypography();
            services.AddServerSideBlazor().AddHubOptions(o =>
            {
                o.MaximumReceiveMessageSize = 10 * 1024 * 1024; // 10MB
            });
            services.AddFileReaderService(options => options.InitializeOnFirstCall = true);

            services.AddMythemeModal();

            services.AddSingleton<BreadcrumbService>();
            services.AddSingleton<SvgHelperService>();
            services.AddSingleton<RandomTableService>();
            services.AddSingleton<TemplateService>();
            services.AddSingleton<FileHandlerService>();
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
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            Task.Run(function: async () => await Electron.WindowManager.CreateWindowAsync());
        }
    }
}
