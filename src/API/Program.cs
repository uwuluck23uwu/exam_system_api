using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using EXAM_SYSTEM_API.API.Shared;
using EXAM_SYSTEM_API.Domain.Interfaces;
using EXAM_SYSTEM_API.Application.Interfaces;
using EXAM_SYSTEM_API.Application.Services;
using EXAM_SYSTEM_API.Infrastructure.Persistence;
using EXAM_SYSTEM_API.Infrastructure.Persistence.Repositories;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Load config by environment
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Add services to the container.
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API",
        Description = "",

        Contact = new OpenApiContact
        {
            Name = "API",
            //Url = new Uri("https://localhost:7183/swagger/index.html")
        },
    });
    c.TagActionsBy(api =>
    {
        if (api.GroupName != null)
        {
            return new[] { api.GroupName };
        }
        var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
        if (controllerActionDescriptor != null)
        {
            return new[] { controllerActionDescriptor.ControllerName };
        }

        throw new InvalidOperationException("Unable to determine tag for endpoint.");
    });
    c.DocInclusionPredicate((name, api) => true);
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


// DbContext
builder.Services.AddDbContext<ExamSystemDbContext>(options =>
     options.UseSqlServer(builder.Configuration.GetConnectionString("ExamSystemConnection")));

// DI สำหรับ Application และ Infrastructure
builder.Services.AddScoped<ControllerHelper>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IAuthenService, AuthenService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IUserSelectService, UserSelectService>();
builder.Services.AddScoped<IExamTakenService, ExamTakenService>();
builder.Services.AddScoped<IServiceFactory, ServiceFactory>();
builder.Services.AddScoped<IExamScheduleService, ExamScheduleService>();
builder.Services.AddScoped<IExamInstructionService, ExamInstructionService>();
builder.Services.AddScoped<IExamSetService, ExamSetService>();
builder.Services.AddScoped<IMultipleChoiceQuestionService, MultipleChoiceQuestionService>();
builder.Services.AddScoped<IGcioCandidateService, GcioCandidateService>();

// Services
builder.Services.AddScoped(typeof(IGenericCrudService<>), typeof(GenericCrudService<>));

const string CORSPOLICY_NAME = "_CORSPOLICY";

builder.Services.AddCors(options =>
{
    options.AddPolicy(CORSPOLICY_NAME, builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    );
});
builder.Services.AddControllers();

var app = builder.Build();

var cultureInfo = new CultureInfo("en-US");
cultureInfo.DateTimeFormat.Calendar = new GregorianCalendar();

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Middleware
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}
//else
//{
//    app.UseExceptionHandler("/error");
//    app.UseHsts();
//}

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseCors(CORSPOLICY_NAME);
app.MapControllers();
app.Run();