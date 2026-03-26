using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResearchProjectManagementSystem.Models
{
    [Table("Acquisitions")]
    public class AcquisitionModel
    {
        [Key]
        [Column("Id")]
        public int IdAcquisition { get; set; }
        [Required]
        public string AcquisitionedObjectName { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public string UnitOfMeasurement { get; set; }
        [Required]
        public double PricePerUnit { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required]
        public DateTime AcquisitionDate { get; set; }
        public int IdResearchProject { get; set; }
        public ResearchProjectModel ResearchProject { get; set; }
    }
}
