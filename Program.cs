using Microsoft.EntityFrameworkCore; /// to intract with database
using TodoApp.Database; // import database
using Microsoft.OpenApi.Models; // for api testing


var builder = WebApplication.CreateBuilder(args);   // create new builder  to add services like datanase or controller

builder.Services.AddControllers();    //add controller service

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    //add data base services 

// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();
// app.UseAuthorization();
app.MapControllers();
app.Run();
