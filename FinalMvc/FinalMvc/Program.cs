using FinalMvc.Data;
using Microsoft.EntityFrameworkCore;
using FinalMvc.Services.Interfaces;
using FinalMvc.Services;
using Microsoft.AspNetCore.Identity;
using FinalMvc.Models;
using FinalMvc.Helpers;
using MVC_Mini_project.Services;
using FinalMvc.FluentValidation.TestimonialsValidation;

using FluentValidation.AspNetCore;
using FluentValidation;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("Smtp"));



builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();



builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;


    options.User.RequireUniqueEmail = true;


    options.SignIn.RequireConfirmedEmail = true;








});
//// FluentValidation
//builder.Services.AddFluentValidationAutoValidation(options => options.DisableDataAnnotationsValidation = true);
//builder.Services.AddValidatorsFromAssemblyContaining<TestimonialAddValidation>();

// Enable both FluentValidation and DataAnnotations validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<TestimonialAddValidation>();


//builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<ILayoutService, LayoutService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
builder.Services.AddScoped<ITestimonialService, TestimonialService>();




var app = builder.Build();

app.UseStaticFiles();



app.UseRouting();


app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=SliderImage}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapDefaultControllerRoute();
});

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//        name: "areas",
//        pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
//    );
//    endpoints.MapDefaultControllerRoute();
//});



app.Run();