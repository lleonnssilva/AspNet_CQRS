using AspNet_CQRS.Api.Filters;
using AspNet_CQRS.CrossCutting.AppDepencencies;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructre(builder.Configuration);

builder.Services.AddMvc(options =>
{
    options.Filters.Add(new CustomExceptionFilter());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
   
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
