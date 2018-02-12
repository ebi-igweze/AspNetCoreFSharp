namespace AspNetFSharp.DataStore

open Microsoft.EntityFrameworkCore
open Microsoft.FSharp.Core.Operators.Unchecked
open AspNetFSharp.Entities
open Microsoft.EntityFrameworkCore.Design
open Microsoft.Extensions.Configuration
open System.IO

type CityInfoContext (options: DbContextOptions) =
    inherit DbContext(options)
    do // Constructor block
        base.Database.Migrate() |> ignore

    let mutable cities = defaultof<DbSet<City>>
    let mutable pointsOfInterest = defaultof<DbSet<PointOfInterest>>

    member this.Cities with get () = cities and set (v) = cities <- v
    member this.PointsOfInterest with get () = pointsOfInterest and set (v) = pointsOfInterest <- v
    
    // override this.OnConfiguring (optionBuilder: DbContextOptionsBuilder) =
    //    optionBuilder.UseSqlServer("connectionstring") |> ignore
    //    base.OnConfiguring(optionBuilder)



type DBOptionsGuy () =
    interface IDesignTimeDbContextFactory<CityInfoContext> with
        member this.CreateDbContext(args: string[]) =
            let configuration = 
                ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
     
            let builder = new DbContextOptionsBuilder<CityInfoContext>()
                 
            //configuration.GetConnectionString("DefaultConnection") 
            @"Data Source=(local);Initial Catalog=CityStore;Integrated Security=True;User ID=admin1;Password=admin1;" |> builder.UseSqlServer |> ignore
            new CityInfoContext(builder.Options)