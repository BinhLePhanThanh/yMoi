using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using yMoi.Service;
using yMoi.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// 🧩 1. Swagger configuration
// =====================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Nhập token vào đây theo định dạng: Bearer {your token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// =====================================================
// 🧩 2. Controller + JSON + Global Auth Policy
// =====================================================
builder.Services.AddControllers(options =>
{
    // Mặc định yêu cầu xác thực với mọi controller
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
})
.AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    x.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

// =====================================================
// 🧩 3. Database (SQLite)
// =====================================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// =====================================================
// 🧩 4. JWT configuration
// =====================================================
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// =====================================================
// 🧩 5. Dependency Injection
// =====================================================
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserBehavior, UserAdminBehavior>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserRoleService>();
builder.Services.AddScoped<IUserRoleBehavior, UserRoleAdminBehavior>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IUnitService, UnitService>();
builder.Services.AddScoped<IUploadFileService, UploadFileService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IMedicineService, MedicineService>();
builder.Services.AddScoped<ICustomerGroupService, CustomerGroupService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "FrontEnd";
});
// =====================================================
// 🧩 6. CORS
// =====================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// =====================================================
// 🧩 7. Database migration + seed data
// =====================================================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();

    // Tạo role Admin nếu chưa có
    var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
    if (adminRole == null)
    {
        adminRole = new Role
        {
            Name = "Admin",
            Description = "Administrator role with full permissions.",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };
        context.Roles.Add(adminRole);
        await context.SaveChangesAsync();
        Console.WriteLine("✅ Seeded default Admin role.");
    }

    // Tạo user "h" nếu chưa có
    var adminUser = await context.Users
        .Include(u => u.UserRoles)
        .FirstOrDefaultAsync(u => u.Username == "h");

    if (adminUser == null)
    {
        adminUser = new User
        {
            Username = "h",
            Password = "1", // ⚠️ cần mã hoá thật khi production
            Name = "System Admin",
            Email = "admin@gmail.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true,
            JobTitle = "Administrator",
            Department = "IT",
            PhoneNumber = "0000000000",
            IdentificationNumber = "N/A",
            DateOfBirth = DateTime.UtcNow,
            Gender = "Other",
            MarriageStatus = "Single",
            Language = "English",
            EducationLevel = "N/A",
            Regilion = "N/A",
            Country = "N/A",
            Notes = "Seeded admin account"
        };

        context.Users.Add(adminUser);
        await context.SaveChangesAsync();
        Console.WriteLine("✅ Seeded default admin user.");
    }

    // Gán role Admin cho user h nếu chưa có
    bool hasAdminRole = await context.UserRoles
        .AnyAsync(ur => ur.UserId == adminUser.Id && ur.RoleId == adminRole.Id);

    if (!hasAdminRole)
    {
        context.UserRoles.Add(new UserRole
        {
            UserId = adminUser.Id,
            RoleId = adminRole.Id
        });
        await context.SaveChangesAsync();
        Console.WriteLine("✅ Assigned Admin role to user 'h'.");
    }
}

// =====================================================
// 🧩 8. Middleware pipeline
// =====================================================
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "StaticFiles")),
    RequestPath = "/static-files"
});
if (builder.Environment.EnvironmentName != "Development")
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "FrontEnd")),
        RequestPath = ""
    });

}
app.MapControllers();

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "FrontEnd";
});

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

        var result = new
        {
            statusCode = context.Response.StatusCode,
            message = exceptionHandlerFeature?.Error.Message // lấy message gốc
        };

        await context.Response.WriteAsJsonAsync(result);
    });
});

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// =====================================================
// 🧩 9. Run
// =====================================================
app.Run();
