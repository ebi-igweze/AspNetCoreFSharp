namespace AspNetFSharp

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging

type Startup() =

    member this.ConfigureServices(services: IServiceCollection) =
        do services.AddMvc() |> ignore

    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment, loggerFactory: ILoggerFactory) =
        do loggerFactory.AddConsole() |> ignore 

        if env.IsDevelopment() 
        then do app.UseDeveloperExceptionPage() |> ignore
        else do app.UseExceptionHandler() |> ignore

        do app.UseMvc() |> ignore

        // do app.Run(fun context -> raise (System.Exception("Example exception")))
        // do app.Run(fun context -> context.Response.WriteAsync("Hello World!"))