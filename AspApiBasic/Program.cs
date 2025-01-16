using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;

namespace AspApiBasic;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        /*builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", policy =>
            {
                policy.AllowAnyOrigin() // Разрешаем запросы с любого источника
                    .AllowAnyMethod() // Разрешаем любые HTTP-методы (GET, POST, PUT, DELETE и т.д.)
                    .AllowAnyHeader(); // Разрешаем любые заголовки
            });

            // В production-окружении не рекомендуется использовать AllowAnyOrigin, 
            // так как это может привести к уязвимости CORS. Вместо этого лучше явно 
            // указать разрешенные домены
            //options.AddPolicy("AllowSpecificOrigins", policy =>
            //{
            //    policy.WithOrigins("https://example.com", "http://localhost:3000")
            //          .AllowAnyMethod()
            //          .AllowAnyHeader();
            //});
        });*/

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "Дэмо API",
                    Version = "v1",
                    Description = "Описание эндпоинтов"
                });
            options.CustomSchemaIds(x => x.FullName);
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseSwagger();
        app.UseSwaggerUI();


        app.UseHttpsRedirection();

        // Включаем CORS с политикой "AllowAllOrigins"
        /*app.UseCors("AllowAllOrigins");*/

        var clientAppPath = Path.Combine(Directory.GetCurrentDirectory(), "ClientApp");
        if (Directory.Exists(clientAppPath))
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(clientAppPath),
                RequestPath = "" // Указываем, что статические файлы будут доступны по корневому пути
            });

            // Если запрос не обработан API, перенаправляем на index.html (для SPA)
            app.Use(async (context, next) =>
            {
                if (!context.Request.Path.StartsWithSegments("/api"))
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.SendFileAsync(Path.Combine(clientAppPath, "index.html"));
                }
                else
                {
                    await next();
                }
            });
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}