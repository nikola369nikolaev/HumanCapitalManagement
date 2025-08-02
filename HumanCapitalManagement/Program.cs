using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Data.Models;
using HumanCapitalManagement.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IAesEncryptionService, AesEncryptionService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");

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

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
    
    var countries = File.ReadAllText("countries.json");
    var json = JsonSerializer.Deserialize<CountriesSeed>(countries, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    if (json != null && !dbContext.Countries.Any())
    {
        dbContext.AddRange(json.Seed);
        await dbContext.SaveChangesAsync();
    }

    string[] roles = { nameof(RoleType.EMPLOYEE), $"{nameof(RoleType.MANAGER)}", $"{nameof(RoleType.HR_ADMIN)}" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    if (!dbContext.Departments.Any(x => x.Name == "IT"))
    {
        dbContext.Departments.Add(new Department() { Name = "IT" });
    }

    if (!dbContext.Departments.Any(x => x.Name == "Finance"))
    {
        dbContext.Departments.Add(new Department() { Name = "Finance" });
    }

    if (!dbContext.Departments.Any(x => x.Name == "HR"))
    {
        dbContext.Departments.Add(new Department() { Name = "HR" });
    }

    await dbContext.SaveChangesAsync();

    var adminEmail = "hr.admin@example.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser != null && !(await userManager.IsInRoleAsync(adminUser, $"{nameof(RoleType.HR_ADMIN)}")))
    {
        await userManager.AddToRoleAsync(adminUser, $"{nameof(RoleType.HR_ADMIN)}");
    }
    else
    {
        var result = await userManager.CreateAsync(new IdentityUser
        {
            UserName = adminEmail,
            NormalizedUserName = "admin",
            Email = adminEmail,
            NormalizedEmail = adminEmail,
            EmailConfirmed = true
        }, "Hr.admin123!");
        if (result.Succeeded)
        {
            adminUser = await userManager.FindByEmailAsync(adminEmail);
            await userManager.AddToRoleAsync(adminUser, $"{nameof(RoleType.HR_ADMIN)}");

            var department = await dbContext.Departments.FirstOrDefaultAsync(x => x.Name == "HR");
            var country = await dbContext.Countries.FirstOrDefaultAsync(x => x.Code == "US");
            dbContext.Employees.Add(new Employee
            {
                FirstName = "admin",
                LastName = "admin",
                Email = adminEmail,
                JobTitle = "admin",
                Salary = 0,
                DepartmentId = department.Id,
                CountryId = country.Id,
                IBAN = ""
            });
            await dbContext.SaveChangesAsync();
        }
        else
        {
            Console.WriteLine("Error creating admin user");
            Console.WriteLine(result.ToString());
        }
    }

    var managerEmail = "manager@example.com";
    var managerUser = await userManager.FindByEmailAsync(managerEmail);

    if (managerUser != null && !(await userManager.IsInRoleAsync(managerUser, $"{nameof(RoleType.MANAGER)}")))
    {
        await userManager.AddToRoleAsync(managerUser, $"{nameof(RoleType.MANAGER)}");
    }
    else
    {
        var result = await userManager.CreateAsync(new IdentityUser
        {
            UserName = managerEmail,
            NormalizedUserName = "manager",
            Email = managerEmail,
            NormalizedEmail = managerEmail,
            EmailConfirmed = true
        }, "Manager123!");
        if (result.Succeeded)
        {
            managerUser = await userManager.FindByEmailAsync(managerEmail);
            await userManager.AddToRoleAsync(managerUser, $"{nameof(RoleType.MANAGER)}");
            var department = await dbContext.Departments.FirstOrDefaultAsync(x => x.Name == "IT");
            var country = await dbContext.Countries.FirstOrDefaultAsync(x => x.Code == "US");
            dbContext.Employees.Add(new Employee
            {
                FirstName = "manager",
                LastName = "manager",
                Email = managerEmail,
                JobTitle = "manager",
                Salary = 0,
                DepartmentId = department.Id,
                CountryId = country.Id,
                IBAN = ""
            });
            await dbContext.SaveChangesAsync();
        }
        else
        {
            Console.WriteLine("Error creating manager user");
            Console.WriteLine(result.ToString());
        }
    }
}

app.Run();


class CountriesSeed
{
    public List<Country> Seed { get; set; }
}