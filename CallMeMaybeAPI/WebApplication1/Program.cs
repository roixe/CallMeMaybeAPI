using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);
//Ajout du DbContext
builder.Services.AddDbContext<CallMeMaybeDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21))));

// ajout des controlleurs
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.WriteIndented = true; // Active l'indentation
});



//Permet la génération automatique de documentation des routes
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("V0.1", new OpenApiInfo { Title = "CallMeMaybe API", Description = "Donne accès à la base de données gérant pour le client lourd et le site web", Version = "v0.1" }));


builder.Services.AddCors(options =>
 
     options.AddPolicy("AllowAnyOrigin", builder =>
     {
         builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
     }));

var app = builder.Build();

// Activer Swagger pour la documentation API en mode développement
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/V0.1/swagger.json", "CallMeMaybe API");
    });
}

// Activer CORS avant le routage
app.UseCors("AllowAnyOrigin");

app.UseRouting();

// Définir les points de terminaison (endpoints)
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); 
});

// Map des contrôleurs
app.MapControllers();


app.Run();