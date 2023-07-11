using BLL.Infrastructure;
using BLL.Services;
using DAL.Context;
using DAL.Repositories.UnitOfWork;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

ConfigurationService(builder.Services);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "emailConfirmation",
    pattern: "confirmation/",
    defaults: new { controller = "EmailConfirm", action = "Confirm" });
app.Run();

void ConfigurationService(IServiceCollection serviceCollection)
{
    serviceCollection.AddDbContext<SpamerContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("connStr")));
    serviceCollection.AddIdentity<Client, IdentityRole>(op => op.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<SpamerContext>().AddDefaultTokenProviders().
        AddTokenProvider<EmailConfirmationTokenProvider<Client>>("emailConfirmationProvider");

    serviceCollection.Configure<EmailConfirmationProviderOption>(op => op.TokenLifespan = TimeSpan.FromDays(5));

    serviceCollection.Configure<SendGridSenderOptions>(op => builder.Configuration.GetSection("SendGridOptions").Bind(op));

    serviceCollection.AddTransient<IUnitOfWork, UnitOfWork>();
    serviceCollection.AddTransient<IEmailSender, EmailSenderService>();
    serviceCollection.AddTransient<SpamService>();

    BllConfiguration.ConfigurationService(serviceCollection);
}