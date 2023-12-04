using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "AddBlazorInAspNetCore API",
        Description = "Web API for managing AddBlazorInAspNetCore",
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.<br>" +
                      "Enter 'Bearer' [space] and then your token in the text input below.<br><br>" +
                      "Example: 'Bearer 12345abcdef'<br><br>",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
    {
      new OpenApiSecurityScheme
      {
        Reference = new OpenApiReference
          {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
          },
        Scheme = "oauth2",
        Name = "Bearer",
        In = ParameterLocation.Header,
      },
      new List<string>()
    }
    });

    // Set XML code docs for swagger.json and Swagger UI
    var pathToXmlDocumentsToLoad = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory)
        .Where(x => Path.GetExtension(x).Equals(".xml", StringComparison.OrdinalIgnoreCase))
        .ToList();

    pathToXmlDocumentsToLoad.ForEach(x => options.IncludeXmlComments(x));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//Blazor
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseBlazorFrameworkFiles();
app.MapFallbackToFile("/index.html");

app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/clientapp"), clientApp =>
{
    clientApp.UseBlazorFrameworkFiles("/clientapp");
    clientApp.UseRouting();
    clientApp.UseEndpoints(endpoints =>
    {       
        endpoints.MapFallbackToFile("/clientapp/{*path:nonfile}", "/clientapp/index.html");
    });    
});

app.UseRouting();

app.MapGet("/", () => "Hello World!");

app.Run();
