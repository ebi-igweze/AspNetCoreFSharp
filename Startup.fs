namespace AspNetFSharp

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection

type Startup() =

    member this.ConfigureServices(services: IServiceCollection) =
        do services.AddMvc() |> ignore

    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =
        if env.IsDevelopment() 
        then do app.UseDeveloperExceptionPage() |> ignore
        else do app.UseExceptionHandler() |> ignore

        do app.UseMvc() |> ignore

        // do app.Run(fun context -> raise (System.Exception("Example exception")))
        // do app.Run(fun context -> context.Response.WriteAsync("Hello World!"))