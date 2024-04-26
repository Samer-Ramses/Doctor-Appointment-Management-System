using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Doctor_System.Models
{
    public class Clinic
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Doctor")]
        public string DoctorId { get; set; }

        [Phone]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [ForeignKey("ClinicWorkingHours")]
        public int WorkingHoursId { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        // Navigation property for the one-to-many relationship with ClinicWorkingHours
        public virtual ICollection<ClinicWorkingHours> WorkingHours { get; set; }

        // Navigation property for the one-to-many relationship with Appointment
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
