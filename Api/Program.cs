

using minimals_api;

IHostBuilder CreateHosBuilder(string [] args){
    return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>{
        webBuilder.UseStartup<Startup>();

    });
}

CreateHosBuilder(args).Build().Run();



