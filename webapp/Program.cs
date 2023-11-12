using System.Net;
using school_project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;



var builder = WebApplication.CreateBuilder(args);

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
        options.SignIn.RequireConfirmedAccount = true;
    }).AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddMvc(options => {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });

builder.Services.AddControllersWithViews();
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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();