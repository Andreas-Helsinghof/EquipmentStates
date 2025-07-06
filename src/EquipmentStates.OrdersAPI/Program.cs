var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<EquipmentStates.OrdersAPI.Services.OrdersService>();
builder.Services.AddSingleton<EquipmentStates.OrdersAPI.Services.EquipmentSSEService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
// Always enable Swagger for dev/demo
app.UseSwagger();
app.UseSwaggerUI();


// Do not use HTTPS redirection
// app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
