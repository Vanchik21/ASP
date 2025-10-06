using MiddlewareSandbox.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var app = builder.Build();

app.UseDeveloperExceptionPage(); 
app.UseStaticFiles();            
app.UseRequestLogging();         
app.UseApiKeyValidation();       
app.UseCustomQuery();            
app.UseRequestCounter();         

app.Run();
