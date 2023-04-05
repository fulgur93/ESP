using ESP.Data;
using Microsoft.EntityFrameworkCore;
using ESP.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ForumDBUsersConnection") ?? throw new InvalidOperationException("Connection string 'ForumDBUsersConnection' not found.");

builder.Services.AddDbContext<ForumDBUsers>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ForumUser>(options => options.SignIn.RequireConfirmedAccount = false) //Czy chce potwierdza� email
    .AddEntityFrameworkStores<ForumDBUsers>();
//baza danych
builder.Services.AddDbContext<MVCDBC>(options =>
options.UseSqlServer(builder.Configuration
.GetConnectionString("MVCDBCString")));
// Add services to the container.
builder.Services.AddControllersWithViews();

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
    pattern: "{controller=Question}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
