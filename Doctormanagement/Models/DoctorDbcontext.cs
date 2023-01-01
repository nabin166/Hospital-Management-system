using Microsoft.EntityFrameworkCore;

namespace Doctormanagement.Models
{
    public partial class DoctorDbcontext : DbContext
    {
        public DoctorDbcontext(DbContextOptions options):base(options)
        {

        }
        public virtual DbSet<User> Users { get; set; }
    
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<PatientAppoint> PatientAppoints { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<DoctorAppoint> DoctorAppoints { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Server= DESKTOP-SC4NGQA\\SQLEXPRESS;Database=Hospital_Management;Integrated Security=True");
            }
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



           

            modelBuilder.Entity<User>(entity =>
            {
           

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Doctor>(entity =>
            {


                entity.HasOne(d => d.User)
                    .WithMany(p => p.Doctors)
                    .HasForeignKey(d => d.userid)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Patient>(entity =>
            {


                entity.HasOne(d => d.User)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.userid)
                    .OnDelete(DeleteBehavior.Cascade);
            });



            modelBuilder.Entity<PatientAppoint>(entity =>
            {


                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientAppoints)
                    .HasForeignKey(d => d.Patient_Id)
                    
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Appointment)
                    .WithMany(p => p.PatientAppoints)
                    .HasForeignKey(d => d.Appoint_Id)
                    
                    .OnDelete(DeleteBehavior.Cascade);



            });

            modelBuilder.Entity<DoctorAppoint>(entity =>
            {


                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.DoctorAppoints)
                    .HasForeignKey(d => d.Doctor_Id)

                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Appointment)
                    .WithMany(p => p.DoctorAppoints)
                    .HasForeignKey(d => d.Appoint_Id)
                  
                    .OnDelete(DeleteBehavior.Cascade);



            });
            modelBuilder.Entity<Role>().HasData(
                new { Id = 1, Roles = "Admin" },
                new { Id = 2, Roles = "Doctor" },
                new { Id = 3, Roles = "Patient" });
       





            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    }
}
