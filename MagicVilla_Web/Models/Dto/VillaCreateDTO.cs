using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.Dto
{
    public class VillaCreateDTO
    {
       

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public string Details { get; set; }

        [Required]
        public double Rate { get; set; }

        public int Occupancy { get; set; }

        public int AreaInSqFt { get; set; }

        public string? ImageURL { get; set; }

        public string? Amenity { get; set; } // New in .NET 6: MUST USE ? if field is NOT REQUIRED
                         // Not doing so will trigger Validation Error !!!



    }
}
