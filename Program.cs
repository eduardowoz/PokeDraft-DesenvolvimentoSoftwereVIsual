using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Pokedraft.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Pokedraft.Data.AppDbContext>(options =>
    options.UseSqlite("Data Source=pokedraft.db"));

builder.Services.AddOpenApi();
builder.Services.AddHttpClient();

builder.Services.AddScoped<PublicService>();
builder.Services.AddScoped<DraftService>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<ResultadoService>();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();