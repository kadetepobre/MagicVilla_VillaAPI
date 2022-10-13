using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_VillaAPI.Models
{
    public class Villa
    {
        [Key] // set as primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-create / increment ID
        public int Id { get; set; }


        public string Name { get; set; }

        // we dont want this property to be exposed in the API
        // this is only for DB record purpose
        // This is the reason why we will expose only the DTO version and NOT the actual Model 
        //
        

        public int Occupancy { get; set; }

        public string Details { get; set; }

        public double Rate { get; set; }

        public string ImageURL { get; set; }

        public string Amenity { get; set; }

        public int AreaInSqFt { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
