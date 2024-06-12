using Microsoft.EntityFrameworkCore;
using SchoolSchedule.Server.Models;
using System;
using static Microsoft.AspNetCore.Http.StatusCodes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//Database setup
string Schedule = string.Empty;

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<ScheduleContext>(opt => opt.UseInMemoryDatabase("InMem"));
}
else
{
    Schedule = builder.Configuration.GetConnectionString("ScheduleConnection");
    builder.Services.AddDbContext<ScheduleContext>(opts =>
        opts.UseSqlServer(Schedule));
}


if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpsRedirection(options =>
    {
        options.RedirectStatusCode = Status308PermanentRedirect;
        options.HttpsPort = 443;
    });
}

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.MapFallbackToFile("/index.html");

var scope = app.Services.CreateScope();
var ctx = scope.ServiceProvider.GetRequiredService<ScheduleContext>();

Course? course = await ctx.Courses.SingleOrDefaultAsync(obj => obj.Id == 1);
if(course == null && app.Environment.IsProduction())
{
    Course tempCourse = new Course()
    {
        Name = "No"
    };
    ctx.Database.Migrate();
    await ctx.Courses.AddAsync(tempCourse);
    await ctx.SaveChangesAsync();
}


app.Run();
