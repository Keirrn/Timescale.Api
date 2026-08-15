using Microsoft.EntityFrameworkCore;
using Timecale.Application.Interfaces;
using Timecale.Application.Services;
using Timecale.Application.Validators;
using Timecale.Application.Calculators;
using Timecale.Infrastructure.Csv;
using Timecale.Infrastructure.Data;
using Timecale.Infrastructure.Repositories;
using Timescale.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<ICsvParser, CsvParser>();
builder.Services.AddScoped<IValueValidator, ValueValidator>();
builder.Services.AddScoped<IResultCalculator, ResultCalculator>();
builder.Services.AddScoped<IFileImportService, FileImportService>();
builder.Services.AddScoped<ITransaction, EfTransaction>();
builder.Services.AddScoped<IResultService, ResultService>();
builder.Services.AddScoped<IValueService, ValueService>();
var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();