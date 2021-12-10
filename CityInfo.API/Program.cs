using Microsoft.AspNetCore.Mvc.Formatters;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddRazorPages();

builder.Services.AddControllers()
    .AddMvcOptions(x => 
    {
        x.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
//app.UseStaticFiles();
//app.UseRouting();
//app.UseAuthorization();
//app.MapRazorPages();

app.UseStatusCodePages();
app.MapControllers();

app.Run();
