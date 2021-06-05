// <copyright file="Startup.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.WebApplication
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using AutoMapper;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using MyAccess.Domains.Providers;
    using MyAccess.Domains.Services;
    using MyAccess.Providers;
    using MyAccess.Services;
    using MyAccess.WebApplication.Middlewares;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                  .AddJwtBearer(options =>
                  {
                      options.TokenValidationParameters = new TokenValidationParameters
                      {
                          ValidateIssuer = true,
                          ValidateAudience = true,
                          ValidateLifetime = true,
                          ValidateIssuerSigningKey = true,
                          ValidIssuer = "Issuer",
                          ValidAudience = "Audience",
                          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SIGNING_KEY"))),
                      };
                  });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o =>
                {
                    o.LoginPath = new PathString("/Users/LogIn");
                    o.AccessDeniedPath = new PathString("/Users/AccessDenied");
                    o.SlidingExpiration = true;
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(1440);
                });

            services.AddCors(c =>
            {
                c.AddPolicy(
                    $"EnableAllCors",
                    options =>
                    options
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin());
            });

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = @"<p><strong>JWT authorization header using Bearer schema.</strong></p><p>Please enter 'Bearer' [space] and then your token in the text input below. <br /> Example: 'Bearer 12345abcde'</p>",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    },
                });

                c.SwaggerDoc($"v1", new OpenApiInfo { Title = Assembly.GetExecutingAssembly().GetName().Name, Version = $"v1" });
            });

            services.AddControllersWithViews();

            services.AddScoped<IUserDao, UserDao>();
            services.AddScoped<ISystemDao, SystemDao>();
            services.AddScoped<ITypeDao, TypeDao>();
            services.AddScoped<IItemDao, ItemDao>();
            services.AddScoped<IUserItemDao, UserItemDao>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISystemService, SystemService>();
            services.AddScoped<ITypeService, TypeService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IUserItemService, UserItemService>();

            services.AddSingleton(new MapperConfiguration(m =>
            {
                m.AddProfile(new MappingProfile());
            }).CreateMapper());
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
                app.ConfigurateExceptionHandler();

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint($"v1/swagger.json", $"API Docs"));

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();
            app.UseRequestLocalization();
            app.UseCors($"EnableAllCors");

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapGet("/api/Version", async context =>
                {
                    await context.Response.WriteAsJsonAsync($"{Assembly.GetEntryAssembly().GetName().Version}");
                });
            });
        }
    }
}
