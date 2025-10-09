using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CRMApp.Areas.Identity.Data;
using CRMApp.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ApplicationUserIdentityContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationUserIdentityContextConnection' not found.");

builder.Services.AddDbContext<ApplicationUserIdentityContext>(options => options.UseSqlServer(connectionString));


builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ApplicationUserIdentityContext>();
builder.Services.AddRazorPages();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IActivityLogger, ActivityLogService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<IContactInquiryService, ContactInquiryService>();
builder.Services.AddHttpContextAccessor();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}
else
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationUserIdentityContext>();
    dbContext.Database.Migrate();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    if (!await roleManager.RoleExistsAsync(Roles.Admin))
    {
        await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
    }
    if (!await roleManager.RoleExistsAsync(Roles.Support))
    {
        await roleManager.CreateAsync(new IdentityRole(Roles.Support));
    }
    if (!await roleManager.RoleExistsAsync(Roles.SalesRep))
    {
        await roleManager.CreateAsync(new IdentityRole(Roles.SalesRep));
    }
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();

app.Run();
