namespace AspNetCoreFSharp.API.Controllers

open Microsoft.AspNetCore.Mvc

[<Route("api/cities")>]
type CitiesController() =
    inherit Controller()

    [<HttpGet()>]
    member this.getCities(): IActionResult =
        let items = CitiesStore.getCities()
        this.Ok items :> _

    [<HttpGet("{id}")>]
    member this.getCity(id): IActionResult =
        match CitiesStore.getCity(id) with
        | Some city -> this.Ok city :> _
        | None -> this.NotFound () :> _
        
        

 [<Route("api/cities")>]
type PointsOfInterestControler() =
    inherit Controller()

    [<HttpGet("{cityId}/pointsOfInterest")>]
    member this.getPointsOfInterest (cityId: int): IActionResult =
        match CitiesStore.getCity(cityId) with
        | Some city -> this.Ok city.pointsOfInterest :> _
        | None -> this.NotFound() :> _
        
    [<HttpGet("{cityId}/pointsOfInterest/{id}")>]
    member this.getPointOfInterest (cityId: int, id: int): IActionResult =
        match CitiesStore.getCity(cityId) with
        | None -> this.NotFound() :> _
        | Some city -> 
            let result = 
                query {
                    for poi in city.pointsOfInterest do
                        where (poi.id = id)
                        select poi
                    } |> Seq.tryHead
            match result with
            | Some poi -> this.Ok poi :> _
            | None -> this.NotFound() :> _


     