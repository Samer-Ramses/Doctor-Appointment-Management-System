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
            builder.Entity<ClinicWorkingHours>().HasKey(ch => new { ch.ClinicId, ch.DayOfWeek });

            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(SeedRoles());
            builder.Entity<ApplicationUser>().HasData(SeedSuperAdmin());
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1", // RoleId for SuperAdmin
                    UserId = "1" // User Id for the default user
                }
            );
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        private List<IdentityRole> SeedRoles()
        {
            return new List<IdentityRole>
            {
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "Admin" },
                new IdentityRole { Id = "2", Name = "Doctor", NormalizedName = "Doctor" },
                new IdentityRole { Id = "3", Name = "Patient", NormalizedName = "Patient" },
            };
        }

        private ApplicationUser SeedSuperAdmin()
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            return new ApplicationUser
            {
                Id = "1",
                UserName = "TemporaryUsername",
                NormalizedUserName = "TEMPORARY-USERNAME",
                Email = "TemporaryEmail@example.com",
                NormalizedEmail = "TEMPORARYEMAIL@EXAMPLE.COM",
                Name = "Temporary first Name",
                Age = 99,
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "TemporaryPassword"),
                SecurityStamp = string.Empty

            };
        }
    }
}
