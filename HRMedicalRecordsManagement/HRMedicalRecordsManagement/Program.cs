using Microsoft.EntityFrameworkCore;
using HRMedicalRecordsManagement.Data;
using HRMedicalRecordsManagement.Services;
using HRMedicalRecordsManagement.Repositories;
using HRMedicalRecordsManagement.Validators;
using FluentValidation;
using HRMedicalRecordsManagement.Common.DeletionData;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<IMedicalRecordTypeRepository, MedicalRecordTypeRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });
//Add validators
builder.Services.AddValidatorsFromAssemblyContaining<TMedicalRecordValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DeletionDataValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
