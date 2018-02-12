namespace AspNetFSharp.Migrations


open AspNetFSharp.DataStore;
open Microsoft.EntityFrameworkCore.Migrations
open Microsoft.EntityFrameworkCore.Infrastructure;
open Microsoft.EntityFrameworkCore.Metadata;
open Microsoft.EntityFrameworkCore


type BuilderOp<'T when 'T :> Operations.MigrationOperation> = Operations.Builders.OperationBuilder<'T>
type AddColumn = BuilderOp<Operations.AddColumnOperation>
type Cities = { Id: AddColumn; Description: AddColumn; Name: AddColumn }
type PointsOfInterest = { Id: AddColumn; Description: AddColumn; Name: AddColumn; CityId: AddColumn }

[<Migration("20180210182534_initialCreate")>]
[<DbContext(typeof<CityInfoContext>)>]
type InitialCreate =
    inherit Migration

    override this.Up(migrationBuilder: MigrationBuilder) =
        migrationBuilder.CreateTable(
            name="Cities",
            columns = 
                fun table ->
                    { Id = table.Column<int>(nullable= false).Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                      Description = table.Column<string>(nullable=true)
                      Name = table.Column<string>(nullable=true) }
            ,
            constraints = fun table -> table.PrimaryKey("PK_Cities", fun x -> x.Id :> _) |> ignore
        ) |> ignore

        migrationBuilder.CreateTable(
            name="PointsOfInterest",
            columns= 
                fun table -> 
                    {
                        Id = table.Column<int>(nullable = false).Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                        CityId = table.Column<int>(nullable = false)
                        Description = table.Column<string>(nullable = true)
                        Name = table.Column<string>(nullable = true)
                    }
            ,
            constraints = fun table ->
                table.PrimaryKey("PK_PointsOfInterest", fun x -> x.Id :> _) |> ignore
                table.ForeignKey (
                    name= "FK_PointsOfInterest_Cities_CityId"
                    ,column= fun x -> x.CityId :> _
                    ,principalTable= "Cities",
                    principalColumn= "Id",
                    onDelete= ReferentialAction.Cascade ) |> ignore
            ) |> ignore

        migrationBuilder.CreateIndex(
            name= "IX_PointsOfInterest_CityId",
            table= "PointsOfInterest",
            column= "CityId") |> ignore
        

    override this.Down(migrationBuilder: MigrationBuilder) =
        migrationBuilder.DropTable(name= "PointsOfInterest") |> ignore
        migrationBuilder.DropTable(name= "Cities") |> ignore

    member this.BuildTargetModel(modelBuilder: ModelBuilder) =
            modelBuilder
            |> fun mb -> mb.HasAnnotation("ProductVersion", "2.0.1-rtm-125")
            |> fun mb -> mb.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
            |> ignore

            modelBuilder.Entity(
                "AspNetFSharp.Entities.City",
                fun b ->
                    b.Property<int>("Id").ValueGeneratedOnAdd() |> ignore
                    b.Property<string>("Description") |> ignore
                    b.Property<string>("Name") |> ignore
                    b.HasKey("Id") |> ignore
                    b.ToTable("Cities") |> ignore
                ) |> ignore

            modelBuilder.Entity("AspNetFSharp.Entities.PointOfInterest",
                fun b ->
                    b.Property<int>("Id").ValueGeneratedOnAdd() |> ignore
                    b.Property<int>("CityId") |> ignore
                    b.Property<string>("Description") |> ignore
                    b.Property<string>("Name") |> ignore
                    b.HasKey("Id") |> ignore
                    b.HasIndex("CityId") |> ignore
                    b.ToTable("PointsOfInterest") |> ignore
                ) |> ignore

            modelBuilder.Entity("AspNetFSharp.Entities.PointOfInterest", fun b ->                
                    b.HasOne("AspNetFSharp.Entities.City", "City")
                        .WithMany("PointsOfInterest")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade) |> ignore
                ) |> ignore

        
