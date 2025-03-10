using Microsoft.Extensions.FileProviders;

namespace AspApiBasic.Extensions;

public static class SpaExtension
{
    // Метод для добавления промежуточного ПО SPA по умолчанию в коллекцию сервисов
    public static void AddDefaultSpaMiddleware(this IServiceCollection services)
    {
        // Добавляем статические файлы SPA с указанием корневого пути
        services.AddSpaStaticFiles(c => { c.RootPath = "ClientApp"; });
    }

    // Метод для настройки промежуточного ПО SPA по умолчанию
    public static void UseDefaultSpaMiddleware(this WebApplication app)
    {
        // Определяем путь к клиентскому приложению
        var clientAppPath = Path.Combine(Directory.GetCurrentDirectory(), "ClientApp");
        
        // Проверяем, существует ли директория клиентского приложения
        if (Directory.Exists(clientAppPath))
        {
            // Используем статические файлы SPA
            app.UseSpaStaticFiles();
            
            // Настраиваем SPA
            app.UseSpa(spa =>
            {
                // Указываем путь к исходным файлам SPA
                spa.Options.SourcePath = "ClientApp";
                
                // Пример проксирования к серверу фронтенда в режиме разработки
                /* if(app.Environment.IsDevelopment())
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:5272");*/
            });
        }
    }


    public static void UseCustomSpaMiddleware(this IApplicationBuilder app)
    {
        // Определяем путь к клиентскому приложению
        var clientAppPath = Path.Combine(Directory.GetCurrentDirectory(), "ClientApp");
        
        // Проверяем, существует ли директория клиентского приложения
        if (Directory.Exists(clientAppPath))
        {
            // Используем файлы по умолчанию (например, index.html)
            app.UseDefaultFiles();
            
            // Настраиваем статические файлы для обслуживания из директории клиентского приложения
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(clientAppPath),
                RequestPath = "" // Указываем, что статические файлы будут доступны по корневому пути
            });

            // Если запрос не обработан API, перенаправляем на index.html (для SPA)
            app.Use(async (context, next) =>
            {
                // Проверяем, что запрос не начинается с /api
                if (!context.Request.Path.StartsWithSegments("/api"))
                {
                    // Устанавливаем тип содержимого как text/html
                    context.Response.ContentType = "text/html";
                    
                    // Отправляем файл index.html в ответ на запрос
                    await context.Response.SendFileAsync(Path.Combine(clientAppPath, "index.html"));
                }
                else
                {
                    // Передаем управление следующему middleware
                    await next();
                }
            });
        }
    }
}