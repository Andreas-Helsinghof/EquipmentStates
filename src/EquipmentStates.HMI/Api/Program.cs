using EquipmentStates.HMI.MachineStateChangeHandler;
using EquipmentStates.HMI.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
{
    options.RootDirectory = "/Api/Pages";
});
builder.Services.AddServerSideBlazor().AddCircuitOptions(options =>
{
    // No special options needed, but this ensures Blazor looks in the right place
});
builder.Services.AddSingleton<EquipmentStateService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<HttpClient>(sp =>
    new HttpClient { BaseAddress = new Uri(builder.Configuration["PUBLIC_BASE_URL"] ?? "http://localhost:5000/") });
builder.Services.AddSignalR();
builder.Services.AddMachineEvents();

var app = builder.Build();

// Enable Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapBlazorHub();
app.MapControllers();
app.MapHub<EquipmentStates.HMI.Api.Hubs.EquipmentStateHub>("/hubs/equipmentstate");
app.MapFallbackToPage("/_Host");

app.Run();
