namespace AspNetFSharp

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Mvc.Formatters

type Startup() =

    member this.ConfigureServices(services: IServiceCollection) =
        // add mvc and xml contract output serializer
        do services
           |> fun s -> s.AddMvc()
           |> fun s -> s.AddMvcOptions(fun opt -> opt.OutputFormatters.Add(XmlDataContractSerializerOutputFormatter()))
           |> ignore

    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment, loggerFactory: ILoggerFactory) =
        do loggerFactory.AddConsole() |> ignore 

        if env.IsDevelopment() 
        then do app.UseDeveloperExceptionPage() |> ignore
        else do app.UseExceptionHandler() |> ignore

        do app.UseMvc() |> ignore

        // do app.Run(fun context -> raise (System.Exception("Example exception")))
        // do app.Run(fun context -> context.Response.WriteAsync("Hello World!"))