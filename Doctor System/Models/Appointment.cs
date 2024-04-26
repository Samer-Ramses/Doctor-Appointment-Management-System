using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Doctor_System.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Clinic")]
        public int ClinicId { get; set; }

        [ForeignKey("Patient")]
        public string PatientId { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        // Navigation properties for the many-to-one relationships with Clinic and Patient
        public virtual Clinic Clinic { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
