module CitiesStore

type PointOfInterest = {
    id: int
    name: string
    description: string
}


type City = {
    id: int;
    name:string;
    description: string;
    numberOfPointsOfInterest: int;
    pointsOfInterest: PointOfInterest[]
}

let private pointsOfInterest = [
    [| {id = 1; name = "Central Park"; description = "The most visited park in the US" }
       {id = 2; name = "Empire State Building"; description = "A 102 story skyscraper located in Midtown Mahattan."} |];
    [| {id = 3; name = "Banana Island"; description = "The most expensive place to live on the Lagos Island"}
       {id = 4; name = "IMax Studios"; description = "The cinema with the biggest screen display in Nigeria"} |]
    [| {id = 5; name = "Aso Rock"; description = "The rock on which the President of Nigeria lives"} |]
]
let private cities = 
    [|
        { id=1; name="New York City"; description="The Apple with the red color"; pointsOfInterest = pointsOfInterest.[0]; numberOfPointsOfInterest = pointsOfInterest.[0].Length}
        { id=2; name="Lagos"; description="The Apple of Nigeria"; pointsOfInterest = pointsOfInterest.[1]; numberOfPointsOfInterest = pointsOfInterest.[1].Length}
        { id=3; name="Abuja"; description="The Secretariat of Nigeria"; pointsOfInterest = pointsOfInterest.[2]; numberOfPointsOfInterest = pointsOfInterest.[2].Length} |]


let getCities (): City[] = cities

let getCity id: City option =
    query {
        for city in cities do
            where(city.id = id)
            select city
    } |> Seq.tryHead

    
    
