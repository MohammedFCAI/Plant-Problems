var builder = WebApplication.CreateBuilder(args);

// Dependencies.
builder.Services.AddServiceRegistration(builder.Configuration).AddInfrastructureDependencies().AddServiceDependencies().AddCoreDependencies().AddDataDependencies();

builder.Services.AddTransient<IEmailService, EmailService>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

app.UseAuthorization();


app.MapControllers();

app.Run();
