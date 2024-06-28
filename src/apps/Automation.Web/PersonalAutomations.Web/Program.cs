using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using PersonalAutomations.Web.Data;
using PersonalAutomations.Web.Data.Classes;
using PersonalAutomations.Web.Interfaces;
using RabbitMQ.Client.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var hostName = builder.Configuration.GetValue<string>("hostname");
var vhostName = builder.Configuration.GetValue<string>("vhost");
var queueName = builder.Configuration.GetValue<string>("queueName");
var username = builder.Configuration.GetValue<string>("rabbitusername");
var password = builder.Configuration.GetValue<string>("password");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IMessageProcessor>(x =>
 new RabbitMQMessageProcessor(
     hostName,
     vhostName,
     queueName,
     username,
     password,
     x.GetService<ApplicationDbContext>()));

builder.Services.AddScoped<IEmailSender>(x => new AutomationEmailSender(x.GetService<ApplicationDbContext>()));
builder.Services.AddRazorPages();

builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.UseSession();

app.Run();
