namespace AspNetFSharp

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Mvc.Formatters
open NLog.Extensions.Logging
open MailService
open Microsoft.Extensions.Configuration

type Startup(env: IHostingEnvironment) =
    static let mutable _configuration: IConfigurationRoot = null
    do 
        ConfigurationBuilder()
        |> fun b -> b.SetBasePath(env.ContentRootPath)
        |> fun b -> b.AddJsonFile("appSettings.json", optional=false, reloadOnChange=true)
        |> fun b -> _configuration <- b.Build()

    static member configuration
        with get() = _configuration
        and set(value) = _configuration <- value

    member this.ConfigureServices(services: IServiceCollection) =
        // add mvc and xml contract output serializer
        do services
           |> fun s -> s.AddTransient<IMailService, LocalMailService>()
           |> fun s -> s.AddMvc()
           |> fun s -> s.AddMvcOptions(fun opt -> opt.OutputFormatters.Add(XmlDataContractSerializerOutputFormatter()))
           |> ignore

    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment, loggerFactory: ILoggerFactory) =
        do loggerFactory
           |> fun l -> l.AddConsole() 
           |> fun l -> l.AddDebug()
           |> fun l -> l.AddNLog()
           //|> fun l -> l.ConfigureNLog("nlog.config")
           |> ignore 

        if env.IsDevelopment() 
        then do app.UseDeveloperExceptionPage() |> ignore
        else do app.UseExceptionHandler() |> ignore

        do app.UseMvc() |> ignore

        // do app.Run(fun context -> raise (System.Exception("Example exception")))
        // do app.Run(fun context -> context.Response.WriteAsync("Hello World!"))

