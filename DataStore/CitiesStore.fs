module CitiesStore

type City = {
    id: int;
    name:string;
    description: string;
}


let private cities = 
    [|
        { id=1; name="New York City"; description="The Apple with the red color"}
        { id=2; name="Lagos"; description="The Apple of Nigeria"}
        { id=3; name="Benin"; description="Them No day carry last"} |]


let getCities (): City[] = cities

let getCity id: City option =
    query {
        for city in cities do
            where(city.id = id)
            select city
    } |> Seq.tryHead
    
    
