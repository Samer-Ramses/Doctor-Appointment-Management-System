using Doctor_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Doctor_System.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Doctor> Doctors {  get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<DoctorSpecialization> DoctorsSpecializations { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<ClinicWorkingHours> ClinicsWorkingHours { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
		public IEnumerable<object> Doctor { get; internal set; }

		protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(SeedRoles());
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        private List<IdentityRole> SeedRoles()
        {
            return new List<IdentityRole>
            {
                new IdentityRole { Id = "1", Name = "Doctor", NormalizedName = "Doctor" },
                new IdentityRole { Id = "2", Name = "Patient", NormalizedName = "Patient" },
            };
        }
    }
}
