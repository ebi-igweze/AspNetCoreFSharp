module CitiesStore

module InputDTOs = 
    open System.ComponentModel.DataAnnotations

    [<CLIMutable>]
    type PointOfInterest = {
        [<Required(ErrorMessage = "Name is required."); MaxLength(50, ErrorMessage = "Name must be less than 50 characters")>]
        name: string
        [<Required(ErrorMessage = "Description is a required input"); MaxLength(200, ErrorMessage = "Description must be less than 200 characters")>]
        description: string
    }
    
type PointOfInterest = {
    id: int
    name: string
    description: string
}


type City = {
    id: int;
    name:string;
    description: string;
    numberOfPointsOfInterest: unit -> int;
    pointsOfInterest: PointOfInterest ResizeArray
}

let private pointsOfInterest = [
    ResizeArray([| {id = 1; name = "Central Park"; description = "The most visited park in the US" }
                   {id = 2; name = "Empire State Building"; description = "A 102 story skyscraper located in Midtown Mahattan."} |]);
    ResizeArray([| {id = 3; name = "Banana Island"; description = "The most expensive place to live on the Lagos Island"}
                   {id = 4; name = "IMax Studios"; description = "The cinema with the biggest screen display in Nigeria"} |]);
    ResizeArray([| {id = 5; name = "Aso Rock"; description = "The rock on which the President of Nigeria lives"} |])
]
let private cities = 
    [|
        { id=1; name="New York City"; description="The Apple with the red color"; pointsOfInterest = pointsOfInterest.[0]; numberOfPointsOfInterest = fun () -> pointsOfInterest.[0].Count}
        { id=2; name="Lagos"; description="The Apple of Nigeria"; pointsOfInterest = pointsOfInterest.[1]; numberOfPointsOfInterest = fun () -> pointsOfInterest.[1].Count}
        { id=3; name="Abuja"; description="The Secretariat of Nigeria"; pointsOfInterest = pointsOfInterest.[2]; numberOfPointsOfInterest = fun () -> pointsOfInterest.[2].Count} |]


let getCities (): City[] = cities

let getCity id: City option =
    query {
        for city in cities do
            where(city.id = id)
            select city
    } |> Seq.tryHead

let getPointOfInterest cityId id: PointOfInterest option =
    getCity cityId
    |> Option.bind (fun city -> 
        query {
            for poi in city.pointsOfInterest do
                where (poi.id = id)
                select poi
            } |> Seq.tryHead )

let removePointOfInterest cityId poi: bool option =
    getCity cityId
    |> Option.map (fun city -> city.pointsOfInterest.Remove(poi))
    
    
    
