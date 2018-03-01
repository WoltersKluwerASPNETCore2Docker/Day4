using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EComm.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using System.Xml;
using System.IO;
using System.Reflection;

namespace EComm.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "geeklearn.com",
                        ValidAudience = "geeklearn.com",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"]))
                    };
                });

            services
                .AddDbContext<DataContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("ECommConnection")))
                .AddCors()
                //.AddMvc()
                .AddMvcCore()
                .AddFormatterMappings()
                .AddAuthorization()
                .AddJsonFormatters()
                .AddXmlDataContractSerializerFormatters()
                .AddMvcOptions(options =>
                    options.FormatterMappings.SetMediaTypeMappingForFormat("xml", "application/xml"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net();

            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.WithOrigins("http://localhost:4200"));

            app.UseMvc();
            DisplayConfiguration();
        }

        private void DisplayConfiguration()
        {
            Console.WriteLine("******************************************");
            Console.WriteLine("Configuration Example");
            Console.WriteLine("******************************************");

            Console.WriteLine("*********** appsettings.json *************");
            Console.WriteLine($"ConnectionStrings:DefaultConnection " +
                $"{(Configuration["ConnectionStrings:DefaultConnection"])}");
        }
    }
}
