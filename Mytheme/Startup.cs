using System;
using System.IO;
using System.Threading.Tasks;
using Blazor.FileReader;
using BlazorStyled;
using BlazorTypography;
using ElectronNET.API;
using Ganss.XSS;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Mytheme.Dal;
using Mytheme.Modal;
using Mytheme.Services;
using Mytheme.Services.Interfaces;


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

            services.AddScoped<IHtmlSanitizer, HtmlSanitizer>(x =>
            {
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedAttributes.Add("class");
                return sanitizer;
            });

            services.AddSingleton<BreadcrumbService>();
            services.AddSingleton<SvgHelperService>();

            services.AddSingleton<DataStorage>();

            services.AddSingleton<IRandomTableService>(x => new RandomTableService(x.GetRequiredService<DataStorage>()));
            services.AddSingleton<ITemplateService>(x => new TemplateService(x.GetRequiredService<DataStorage>()));
            services.AddSingleton<IFileHandlerService>(x => new FileHandlerService(x.GetRequiredService<DataStorage>()));
            services.AddSingleton<ICampaignService>(x => new CampaignService(x.GetRequiredService<DataStorage>()));

            services.AddServerSideBlazor().AddCircuitOptions(o =>
            {
                o.DetailedErrors = true;
            });
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

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Mytheme")),
                RequestPath = "/mythemelocal"
            });

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
