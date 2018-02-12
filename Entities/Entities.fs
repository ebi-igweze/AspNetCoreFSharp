namespace AspNetFSharp.Entities
open System.Collections.Generic
open Microsoft.FSharp.Core.Operators.Unchecked
open System.ComponentModel.DataAnnotations
open System.ComponentModel.DataAnnotations.Schema
open Microsoft.EntityFrameworkCore.Design

type City() = 
    member val Id = defaultof<int> with get, set
    member val Name = defaultof<string> with get, set
    member val Description = defaultof<string> with get, set
    member val PointsOfInterest: ICollection<PointOfInterest> = List<PointOfInterest>() :> _  with get, set

and PointOfInterest() =    
    [<Key; DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    member val Id = defaultof<int> with get, set
    member val Name = defaultof<string> with get, set
    member val Description = defaultof<string> with get, set
    [<ForeignKey("City")>]
    member val CityId = defaultof<int> with get, set
    member val City = defaultof<City> with get, set





   