namespace AspNetCoreFSharp.API.Controllers

open Microsoft.AspNetCore.Mvc

[<Route("api/cities")>]
type CitiesController() =
    inherit Controller()

    [<HttpGet()>]
    member this.GetCities(): IActionResult =
        let items = CitiesStore.getCities()
        this.Ok items :> _

    [<HttpGet("{id}")>]
    member this.GetCity(id): IActionResult =
        match CitiesStore.getCity(id) with
        | Some city -> this.Ok city :> _
        | None -> this.NotFound () :> _
        
        