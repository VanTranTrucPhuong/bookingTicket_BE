using BookingTicketRestAPICoreDapper.Controllers;
using BookingTicketRestAPICoreDapper.Hubs;
using BookingTicketRestAPICoreDapper_Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Data.SqlClient;
using System.IO;

namespace BookingTicketRestAPICoreDapper
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string dbconfig = "Server=DESKTOP-JNKAS6I;Database=BookingTicket;User Id=sa;Password=123";

            //DI Respository
            services.AddTransient<IQuanLyPhimRespository, QuanLyPhimRespository>(f => new QuanLyPhimRespository(dbconfig));
            services.AddTransient<IQuanLyHeThongRapRespository, QuanLyHeThongRapRespository>(f => new QuanLyHeThongRapRespository(dbconfig));
            services.AddTransient<ICumRapRespository, CumRapRespository>(f => new CumRapRespository(dbconfig));
            services.AddTransient<IQuanLyThanhVienRepository, QuanLyThanhVienRespository>(f => new QuanLyThanhVienRespository(dbconfig));
            services.AddTransient<IQuanLyDatVeRespository, QuanLyDatVeRespository>(f => new QuanLyDatVeRespository(dbconfig));
            services.AddTransient<IDashboardRespository, DashboardRespository>(f => new DashboardRespository(dbconfig));
            services.AddTransient<IQuanLySettingRespository, QuanLySettingRespository>(f => new QuanLySettingRespository(dbconfig));

            //DI Service
            //services.AddTransient<IPhimService, PhimService>();
            //services.AddTransient<IHeThongRapService, HeThongRapService>();
            //services.AddTransient<ICumRapService, CumRapService>();
            //services.AddTransient<IQuanLyThanhVienService, QuanLyThanhVienService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(opt =>
            {
                opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
            }); ;

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            //cau hinh jwt
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtVTTP";
                options.DefaultChallengeScheme = "JwtVTTP";
            }).AddJwtBearer("JwtVTTP", jwtOptions => {
                jwtOptions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    IssuerSigningKey = QuanLyThanhVienController.SIGNING_KEY,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromDays(365)

                };
            });
            //SQL Dependency
            services.AddSignalR();

            //Cho phép sử dụng cross
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://crm.myclass.vn/", "http://127.0.0.1:5500", "https://localhost:5001/swagger/index.html", "http://localhost:3000", "http://localhost:3001", "http://localhost:3002", "http://localhost:3003", "http://localhost:4200", "*").AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials().Build();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //Cho phép xem hình
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            app.UseCors(MyAllowSpecificOrigins);
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            //authen token
            app.UseAuthentication();

            // allow origin
            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();

            //Khai báo sử dụng folder hình
            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "HinhAnh")),
                RequestPath = new PathString("/HinhAnh")
            });

            //SignalR
            app.UseSignalR(routes =>
            {
                routes.MapHub<DatVeHub>("/DatVeHub");
            });

            app.UseOwin();

            app.UseMvc();
        }
    }
}