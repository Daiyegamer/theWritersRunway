﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using AdilBooks.Data; // Keep this for now

// Service interfaces and implementations
using AdilBooks.Services;
using AdilBooks.Interfaces;
//using FashionVote.Services;

var builder = WebApplication.CreateBuilder(args);

// Setup SQL Server connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(connectionString));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseSqlite(connectionString)); 

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// ADDED THIS INSTEAD
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();

// Core MVC + Razor + SignalR
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

// Book Domain Services
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddTransient<IFileService, FileService>();

// FashionVote Services
//builder.Services.AddSingleton<IEmailSender, EmailSender>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "WritersRunway API",
        Version = "v1",
        Description = "Unified API for books and fashion voting"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {token}' to authenticate."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Role/User Seeding Logic
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ApplicationDbContext>();

    // UNCOMENTED THESE LINES BELOW
    dbContext.Database.Migrate();

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = { "Admin", "Participant" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

// Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // ✅ Show detailed dev errors
    app.UseMigrationsEndPoint();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WritersRunway API v1");
        c.RoutePrefix = "swagger";
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // ✅ Required to serve images/css/js
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

// Custom Routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "books",
    pattern: "Books/{action=List}/{id?}",
    defaults: new { controller = "BooksPage" });

app.MapControllerRoute(
    name: "authors",
    pattern: "Authors/{action=List}/{id?}",
    defaults: new { controller = "AuthorsPage" });

app.MapControllerRoute(
    name: "publishers",
    pattern: "Publishers/{action=List}/{id?}",
    defaults: new { controller = "PublishersPage" });

// SignalR (for FashionVote)
app.MapHub<AdilBooks.Hubs.VoteHub>("/voteHub");

app.Run();
    