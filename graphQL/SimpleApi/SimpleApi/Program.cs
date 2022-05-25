using Microsoft.EntityFrameworkCore;
using WebApplication1;
using WebApplication1.Store;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDbContext<AppDbContext>(context =>
{
    context.UseInMemoryDatabase("TimeGraphServer");
});
services.AddGraphQLServer()
    .AddType<ProjectType>()
    .AddType<TimeLogType>()
    .RegisterDbContext<AppDbContext>()
    .AddQueryType<Query>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);



services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());
var app = builder.Build();

app.MapGraphQL();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.Run();