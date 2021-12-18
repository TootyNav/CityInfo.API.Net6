using Microsoft.AspNetCore.Mvc.Formatters;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddRazorPages();
builder.Services.AddMvc()
    .AddMvcOptions(o =>
    {
        o.EnableEndpointRouting = false;
        o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseMvc();

app.Run();
