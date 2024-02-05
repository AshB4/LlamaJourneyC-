using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Blazor.Data;
using MudBlazor.Services;
using MudBlazor.Extensions;
using Plotly.Blazor;
using Plotly.Blazor.Traces;
using Financr.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddMudServices();
builder.Services.AddScoped<PlotlyGrapher>();
builder.Services.AddScoped<LoanCalculator>();
builder.Services.AddScoped<LoanGrapher>();
builder.Services.AddScoped<SavingsCalculator>();
builder.Services.AddScoped<SavingsGrapher>();
builder.Services.AddScoped<PlotlySavingsGrapher>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

