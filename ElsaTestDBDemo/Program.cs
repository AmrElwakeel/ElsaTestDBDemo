using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.SqlServer;
using ElsaTestDBDemo.DomainDatabase;

var builder = WebApplication.CreateBuilder(args);


var elsaSection = builder.Configuration.GetSection("Elsa");

var SqlServerconnectionString = builder.Configuration.GetConnectionString("Defualt");

builder.Services.AddDbContextPool<ApplicationDBContext>(opt => opt.UseSqlServer(SqlServerconnectionString, typeof(ApplicationDBContext)));

builder.Services.AddCors(cors => cors.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

// Elsa services.
builder.Services
    .AddElsa(options => options.UseEntityFrameworkPersistence(ef => ef.UseSqlServer(SqlServerconnectionString))
        .AddConsoleActivities()
        .AddJavaScriptActivities()
        .AddHttpActivities(elsaSection.GetSection("Server").Bind)
        //.AddEmailActivities(elsaSection.GetSection("Smtp").Bind)
        //.AddQuartzTemporalActivities()
        //.AddWorkflowsFrom<Program>()
    );

// Elsa API endpoints.
builder.Services.AddElsaApiEndpoints();


// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles()
    .UseHttpActivities()
    .UseRouting()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();

        // For Dashboard.
        endpoints.MapFallbackToPage("/_Host");
    });

//app.UseRouting();

//app.UseAuthorization();

//app.MapRazorPages();

app.Run();
