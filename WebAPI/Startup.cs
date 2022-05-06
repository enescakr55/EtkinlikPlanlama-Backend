using Business.Abstract;
using Business.Concrete;
using Business.Concrete.Helpers;
using Business.Validator;
using Core.Results;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entities.Concrete;
using Entities.Concrete.Dtos;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration,IWebHostEnvironment env)
        {
            Configuration = configuration;
        }

        
    public IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            using (var context = new AppDbContext())
            {
                context.Database.Migrate();
            }
            services.AddControllers().AddFluentValidation().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = c =>
                {
                    var errorlist = c.ModelState.Values.Where(v=>v.Errors.Count>0).SelectMany(v=>v.Errors);
                    List<string> errors = new List<string>();
                    foreach (var error in errorlist)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                    //string.Join('\n', c.ModelState.Values.Where(v => v.Errors.Count > 0)
                      //.SelectMany(v => v.Errors)
                      //.Select(v => v.ErrorMessage));

                    return new BadRequestObjectResult(new ErrorDataResult<List<String>>(errors, "Doðrulama Hatasý"));

                };
            });
            services.AddTransient<IValidator<User>, UserValidator>();
            services.AddTransient <IValidator<ChangePasswordDto>, ChangePasswordDtoValidator>();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin", builder => builder.AllowAnyOrigin());
            });

            

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddSingleton<IUserService, UserManager>();
            services.AddSingleton<IUserDao, UserDao>();
            services.AddSingleton<IEventsDao, EventsDao>();
            services.AddSingleton<IEventService, EventManager>();
            services.AddSingleton<IAccountValidationCodesDao, AccountValidationCodesDao>();
            services.AddSingleton<IAccountValidationCodeService, AccountValidationCodeManager>();
            services.AddSingleton<IInvitationsDao, InvitationsDao>();
            services.AddSingleton<IInvitationStatusesDao, InvitationStatusesDao>();
            services.AddSingleton<IInvitationService, InvitationManager>();
            services.AddSingleton<IInvitationStatusService, InvitationStatusManager>();
            services.AddScoped<IAuthService, AuthManager>();
            services.AddSingleton<ISMTPMailService, SMTPMailManager>();
            services.AddSingleton<IJoinEventService, JoinEventManager>();
            services.AddSingleton<IEventJoinsDao, JoinEventsDao>();
            services.AddSingleton<IUpcomingEventsDao, UpcomingEventsDao>();
            services.AddSingleton<IUpcomingEventService, UpcomingEventManager>();
            //services.AddDbContext<AppDbContext>(item => item.UseSqlServer(Configuration.GetConnectionString("EventDatabase")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                
                var exception = exceptionHandlerPathFeature.Error;
                var result = JsonConvert.SerializeObject(new ErrorResult(exception.Message));
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }));

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.Use(async (context, next) =>
            {
                await next();

                if (!Path.HasExtension(context.Request.Path.Value) &&
                    !context.Request.Path.Value.StartsWith("/api/"))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });

            app.UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = new List<string> { "index.html" } });
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
