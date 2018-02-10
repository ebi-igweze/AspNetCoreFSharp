namespace AspNetCoreFSharp.API.Controllers

open Microsoft.AspNetCore.Mvc
open CitiesStore
open Microsoft.AspNetCore.JsonPatch
open Microsoft.Extensions.Logging
open MailService

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
        
  
  
type CreatedPointOfInterestResponse = { cityId: int; id: int }

 [<Route("api/cities")>]
type PointsOfInterestControler(_logger: ILogger<PointsOfInterestControler>, _mailService: IMailService) =
    inherit Controller()

    member private this.validateInput(poiOption: InputDTOs.PointOfInterest option, action: unit -> IActionResult): IActionResult = 
        do match poiOption with 
           | None -> ()
           | Some poi -> 
              if (poi.name = poi.description) 
              then do this.ModelState.AddModelError("description", "The provided description should be different from the name")

        match this.ModelState.IsValid with
        | false -> this.BadRequest (this.ModelState) :> _
        | true -> action()
            

    [<HttpGet("{cityId}/pointsOfInterest")>]
    member this.getPointsOfInterest (cityId: int): IActionResult =
        match CitiesStore.getCity(cityId) with
        | Some city -> this.Ok city.pointsOfInterest :> _
        | None -> 
            try 
                failwith "The City was not found, or does not exist"
            with 
            | e ->
                do _logger.LogInformation (sprintf "City with id %i wasn't found when accessing points of interest." cityId)
                this.StatusCode(500, "A problem happened while handling request.") :> _
        
    [<HttpGet("{cityId}/pointsOfInterest/{id}", Name="GetPointOfInterest")>]
    member this.getPointOfInterest (cityId: int, id: int): IActionResult =
        match CitiesStore.getPointOfInterest cityId id with
        | None -> this.NotFound() :> _
        | Some poi ->  this.Ok poi :> _


    [<HttpPost("{cityId}/pointsOfInterest")>]
    member this.createPointOfInterest (cityId: int, [<FromBody>] poi: InputDTOs.PointOfInterest): IActionResult = 
        this.validateInput(Some poi, fun () -> 
            match CitiesStore.getCity(cityId) with
            | None -> this.NotFound() :> _
            | Some city -> 
                let newPoi = { id = city.numberOfPointsOfInterest() + 1; name = poi.name; description = poi.description}
                do city.pointsOfInterest.Add newPoi
                this.CreatedAtRoute("GetPointOfInterest", { cityId = cityId; id = newPoi.id }, newPoi) :> _ )

     [<HttpPut("{cityId}/pointsOfInterest/{id}")>]
     member this.updatePointsOfInterest (cityId: int, id: int, [<FromBody>] poi: InputDTOs.PointOfInterest): IActionResult = 
        this.validateInput (Some poi, fun () -> 
            match CitiesStore.getPointOfInterest cityId id with
            | None -> this.NotFound() :> _
            | Some p -> 
                let v =  { p with name = poi.name; description = poi.description }
                this.NoContent() :> _ )

     [<HttpPatch("{cityId}/pointsOfInterest/{id}")>]
     member this.patchPointOfInterest (cityId: int, id: int, [<FromBody>] poiPatchDoc: JsonPatchDocument<InputDTOs.PointOfInterest>): IActionResult = 
        this.validateInput(None, fun () -> 
            match CitiesStore.getPointOfInterest cityId id with
            | None -> this.NotFound () :> _
            | Some poi -> 
                let poiToEdit = { InputDTOs.PointOfInterest.name = poi.name; InputDTOs.PointOfInterest.description = poi.description }
                do poiPatchDoc.ApplyTo(poiToEdit, this.ModelState)
                if this.ModelState.IsValid
                then this.NoContent () :> _
                else this.BadRequest(this.ModelState) :> _ )
     
     [<HttpDelete("{cityId}/pointsOfInterest/{id}")>]
     member this.deletePointOfInterest(cityId: int, id: int): IActionResult =
        match CitiesStore.getPointOfInterest cityId id with
        | None -> this.NotFound() :> _
        | Some poi -> 
            match CitiesStore.removePointOfInterest cityId poi with
            | Some _ ->
                let message = "The Delete action for Point of Interest was called"
                let subject = (sprintf "Deleted Point of Interest %s" poi.name)
                do _mailService.sendMail (subject, message)
                this.NoContent () :> _
            | None -> this.BadRequest() :> _