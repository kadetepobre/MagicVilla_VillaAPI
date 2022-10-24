using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Data
{
    // 
    // add-migration AddVillaTable -> You need to RUN this in Nuget Package Manager Console
    //     to be able to create MIGRATION SCRIPTS that will create the actual DB
    //     for us, based on the codes we have written
    //  
    // NOTE+ If you are modifying the database, like adding a field, YOU CAN JUST 
    //      run ADD-MIGRATION again WITH NEW NAME. i.e. add-migration AddVillaTableWithOccupancy
    //
    //
    //
    // You can also re-create/update the DB by deleting the DB in MS SQL Server
    // only as a last resort. Then in Visual Studio, delete the 
    // auto-generated scripts in MIGRATIONS folder before running add-migration again.


    // update-database  -> This is create the actual DATABASE from the Migration Scripts generated 
    //      from above. Need to run this is Nuget Package Manager too, after add-migration.


    // NOTE: In MS SQL Server Management Studio , if you cannot expand the TABLES,
    // close Visual Studio first 
    // then try expanding TABLES again in MS Server Management Studio

    public class ApplicationDbContext : DbContext
    {

        // here "base" is DbContext
        // and we pass the options to it
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        // The name of this entity will be the name of the TABLE in the Database
        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(

                new Villa()
                {
                    Id = 1,
                    Name = "Royal Villa",
                    Details = "This is a royal villa.",
                    ImageURL = "",
                    Occupancy = 100,
                    Rate = 200,
                    AreaInSqFt = 550,
                    Amenity = "",
                    CreatedDate = DateTime.Now

                },

                new Villa()
                {
                    Id = 2,
                    Name = "Coca Cola Villa",
                    Details = "This is a soda villa.",
                    ImageURL = "",
                    Occupancy = 150,
                    Rate = 100,
                    AreaInSqFt = 50,
                    Amenity = "",
                    CreatedDate = DateTime.Now
                },

                new Villa()
                {
                    Id = 3,
                    Name = "Ice Cream Villa",
                    Details = "This is the ice cream villa.",
                    ImageURL = "",
                    Occupancy =150,
                    Rate = 150,
                    AreaInSqFt = 200,
                    Amenity = "",
                    CreatedDate = DateTime.Now
                }



                );
        }

    }
}
