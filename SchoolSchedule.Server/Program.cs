using Microsoft.EntityFrameworkCore;
using SchoolSchedule.Server.Models;
using static Microsoft.AspNetCore.Http.StatusCodes;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddDbContext<ScheduleContext>(opts => 
    opts.UseSqlServer(builder.Configuration.GetConnectionString("ScheduleConnection")));

builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = Status307TemporaryRedirect;
    options.HttpsPort = 5000;
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.MapFallbackToFile("/index.html");

var scope = app.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<ScheduleContext>().Database.Migrate();

app.Run();
