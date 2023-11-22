using System;
using System.Net;
using school_project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using school_project.Security;


var builder = WebApplication.CreateBuilder(args);
// var connectionString = builder.Configuration.GetConnectionString("DiagnosisDBConnection");builder.Services.AddDbContext<school_projectIdentityDbContext>(options =>
//     options.UseSqlServer(connectionString));builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//     .AddEntityFrameworkStores<school_projectIdentityDbContext>();
// Add services to the container.
builder.Services.AddDbContext<AppDbContext> (options =>{
    var config = builder.Configuration;
    var connectionString = config.GetConnectionString("DiagnosisDBConnection");
    var classificationEndpoint = config.GetConnectionString("classificationEndpoint");

    options.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => 
    {
        options.Password.RequiredLength = 8;
        options.Password.RequiredUniqueChars = 1;   
        options.SignIn.RequireConfirmedAccount = false;
    }).AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddMvc(options => {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });

// builder.Services.AddAuthorization(options =>
//     {
//         options.AddPolicy("DeleteRolePolicy",
//             policy => policy.RequireClaim("Delete Role"));

//         options.AddPolicy("EditRolePolicy",
//             policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

//         options.InvokeHandlersAfterFailure = false;

//         options.AddPolicy("AdminRolePolicy",
//             policy => policy.RequireRole("Admin"));

//     });

builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpsRedirection(options => 
    {
        options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
        options.HttpsPort = 443;
    });
}


if (!app.Environment.IsDevelopment()){
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern:"{controller=home}/{action=Index}/{id?}");

app.Run();