using Microsoft.AspNetCore.Mvc.Formatters;

using NLog;
using NLog.Web;

//use inbuilt logger to log errors using Nlogger. This will log files in folders mentioned in nlog config file 
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");


try
{
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

}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}