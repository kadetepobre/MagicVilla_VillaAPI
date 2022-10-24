using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_VillaAPI.Models
{
    public class VillaNumber
    {
        [Key, DatabaseGenerated (DatabaseGeneratedOption.None)]
        public int VillaNo { get; set; }

        

        /// When we define a FK in EF, we need to add a navigation property
        /// We also need to specify the name of the navigation property
        [ForeignKey("Villa")] //Means VillaId here is a foreign key that points to the "Villa" table.
        public int VillaId { get; set; }

        // This is the navigation property for our FK. This is a reference to
        // the table which the Foreign key above points to.
        public Villa Villa { get; set; }



        public string SpecialDetails {  get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    
    }
}
