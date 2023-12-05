var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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

app.UseAuthorization();

app.MapControllers();

app.Run();
