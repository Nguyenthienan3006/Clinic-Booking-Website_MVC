using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Website_Mvc.Models
{
    public partial class ClinicBookingProjectContext : DbContext
    {
		public ClinicBookingProjectContext()
		{
		}

		public ClinicBookingProjectContext(DbContextOptions<ClinicBookingProjectContext> options)
			: base(options)
		{
		}

		public virtual DbSet<Account> Accounts { get; set; } = null!;
		public virtual DbSet<AccountInfo> AccountInfos { get; set; } = null!;
		public virtual DbSet<Appointment> Appointments { get; set; } = null!;
		public virtual DbSet<AppointmentStatus> AppointmentStatuses { get; set; } = null!;
		public virtual DbSet<Department> Departments { get; set; } = null!;
		public virtual DbSet<Disease> Diseases { get; set; } = null!;
		public virtual DbSet<DoctorDepartment> DoctorDepartments { get; set; } = null!;
		public virtual DbSet<DoctorFeedbacksPatient> DoctorFeedbacksPatients { get; set; } = null!;
		public virtual DbSet<PatientReviewsDoctor> PatientReviewsDoctors { get; set; } = null!;
		public virtual DbSet<Roll> Rolls { get; set; } = null!;

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
				optionsBuilder.UseSqlServer("server =(local); database = ClinicBookingProject;;uid=sa;pwd=123456;");
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Account>(entity =>
			{
				entity.HasKey(e => e.IdUser)
					.HasName("PK__Account__B607F248731392D4");

				entity.ToTable("Account");

				entity.Property(e => e.IdUser).HasColumnName("Id_user");

				entity.Property(e => e.Password)
					.HasMaxLength(255)
					.IsUnicode(false)
					.HasColumnName("password");

				entity.Property(e => e.Roll).HasColumnName("roll");

				entity.Property(e => e.AccountStatus).HasColumnName("accountStatus");

				entity.Property(e => e.Username)
					.HasMaxLength(255)
					.IsUnicode(false)
					.HasColumnName("username");

				entity.HasOne(d => d.RollNavigation)
					.WithMany(p => p.Accounts)
					.HasForeignKey(d => d.Roll)
					.HasConstraintName("FK__Account__roll__3B75D760");
			});

			modelBuilder.Entity<AccountInfo>(entity =>
			{
				entity.HasKey(e => e.IdUser)
					.HasName("PK__AccountI__B607F248BD29B165");

				entity.ToTable("AccountInfo");

				entity.Property(e => e.IdUser)
					.ValueGeneratedOnAdd()
					.HasColumnName("Id_user");

				entity.Property(e => e.Address)
					.HasMaxLength(255)
					.IsUnicode(false)
					.HasColumnName("address");

				entity.Property(e => e.BloodGroup)
					.HasMaxLength(3)
					.IsUnicode(false)
					.HasColumnName("bloodGroup");

				entity.Property(e => e.DateOfBirth)
					.HasColumnType("datetime")
					.HasColumnName("dateOfBirth");

				entity.Property(e => e.Email)
					.HasMaxLength(255)
					.IsUnicode(false)
					.HasColumnName("email");

				entity.Property(e => e.FirstName)
					.HasMaxLength(15)
					.IsUnicode(false)
					.HasColumnName("firstName");

				entity.Property(e => e.LastName)
					.HasMaxLength(15)
					.IsUnicode(false)
					.HasColumnName("lastName");

				entity.Property(e => e.Phonenumber)
					.HasMaxLength(15)
					.IsUnicode(false)
					.HasColumnName("phonenumber");

				entity.Property(e => e.User_Image)
					.HasMaxLength(255)
					.HasColumnName("user_image");

				entity.HasOne(d => d.IdUserNavigation)
					.WithOne(p => p.AccountInfo)
					.HasForeignKey<AccountInfo>(d => d.IdUser)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK__AccountIn__Id_us__3E52440B");
			});

			modelBuilder.Entity<Appointment>(entity =>
			{
				entity.HasKey(e => e.IdAppointment)
					.HasName("PK__Appointm__D0F3704D3CEDC454");

				entity.ToTable("Appointment");

				entity.Property(e => e.IdAppointment).HasColumnName("Id_appointment");

				entity.Property(e => e.AppointmentTime)
					.HasColumnType("datetime")
					.HasColumnName("appointment_time");

				entity.Property(e => e.IdDisease).HasColumnName("Id_disease");

				entity.Property(e => e.IdDoctor).HasColumnName("Id_doctor");

				entity.Property(e => e.IdPatient).HasColumnName("Id_patient");

				entity.Property(e => e.PatientConditionDescription)
					.HasColumnType("text")
					.HasColumnName("patient_condition_description");

				entity.Property(e => e.Status).HasColumnName("status");

				entity.Property(e => e.TotalPrice)
					.HasColumnType("decimal(10, 2)")
					.HasColumnName("totalPrice");

				entity.HasOne(d => d.IdDiseaseNavigation)
					.WithMany(p => p.Appointments)
					.HasForeignKey(d => d.IdDisease)
					.HasConstraintName("FK__Appointme__Id_di__49C3F6B7");

				entity.HasOne(d => d.IdDoctorNavigation)
					.WithMany(p => p.AppointmentIdDoctorNavigations)
					.HasForeignKey(d => d.IdDoctor)
					.HasConstraintName("FK__Appointme__Id_do__4AB81AF0");

				entity.HasOne(d => d.IdPatientNavigation)
					.WithMany(p => p.AppointmentIdPatientNavigations)
					.HasForeignKey(d => d.IdPatient)
					.HasConstraintName("FK__Appointme__Id_pa__4BAC3F29");

				entity.HasOne(d => d.StatusNavigation)
					.WithMany(p => p.Appointments)
					.HasForeignKey(d => d.Status)
					.HasConstraintName("FK__Appointme__statu__4CA06362");
			});

			modelBuilder.Entity<AppointmentStatus>(entity =>
			{
				entity.HasKey(e => e.IdStatus)
					.HasName("PK__Appointm__9D83DA2A743F0973");

				entity.ToTable("AppointmentStatus");

				entity.Property(e => e.IdStatus).HasColumnName("Id_status");

				entity.Property(e => e.StatusName)
					.HasMaxLength(50)
					.IsUnicode(false)
					.HasColumnName("status_name");
			});

			modelBuilder.Entity<Department>(entity =>
			{
				entity.ToTable("Department");

				entity.Property(e => e.DepartmentId)
					.HasMaxLength(10)
					.IsUnicode(false)
					.HasColumnName("department_Id");

				entity.Property(e => e.Department_Image)
					.HasMaxLength(255)
					.HasColumnName("department_image");

				entity.Property(e => e.DepartmentName)
					.HasMaxLength(255)
					.IsUnicode(false)
					.HasColumnName("department_name");
			});

			modelBuilder.Entity<Disease>(entity =>
			{
				entity.HasKey(e => e.IdDisease)
					.HasName("PK__Disease__19C3C497AB5E9BFF");

				entity.ToTable("Disease");

				entity.Property(e => e.IdDisease).HasColumnName("Id_disease");

				entity.Property(e => e.DiseaseName)
					.HasMaxLength(255)
					.IsUnicode(false)
					.HasColumnName("disease_name");

				entity.Property(e => e.IdDepartment)
					.HasMaxLength(10)
					.IsUnicode(false)
					.HasColumnName("Id_department");

				entity.Property(e => e.Price)
					.HasColumnType("decimal(10, 2)")
					.HasColumnName("price");

				entity.HasOne(d => d.IdDepartmentNavigation)
					.WithMany(p => p.Diseases)
					.HasForeignKey(d => d.IdDepartment)
					.HasConstraintName("FK__Disease__Id_depa__4316F928");
			});

			modelBuilder.Entity<DoctorDepartment>(entity =>
			{
				entity.HasKey(e => e.DepartmentId)
					.HasName("PK__DoctorDe__C222200ACB9CDDD7");

				entity.ToTable("DoctorDepartment");

				entity.Property(e => e.DepartmentId)
					.HasMaxLength(10)
					.IsUnicode(false)
					.HasColumnName("department_Id");

				entity.Property(e => e.IdUser).HasColumnName("Id_user");

				entity.HasOne(d => d.Department)
					.WithOne(p => p.DoctorDepartment)
					.HasForeignKey<DoctorDepartment>(d => d.DepartmentId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK__DoctorDep__depar__45F365D3");

				entity.HasOne(d => d.IdUserNavigation)
					.WithMany(p => p.DoctorDepartments)
					.HasForeignKey(d => d.IdUser)
					.HasConstraintName("FK__DoctorDep__Id_us__46E78A0C");
			});

			modelBuilder.Entity<DoctorFeedbacksPatient>(entity =>
			{
				entity.HasKey(e => e.IdFeedbacks)
					.HasName("PK__DoctorFe__2C8D506C573579F9");

				entity.ToTable("DoctorFeedbacksPatient");

				entity.Property(e => e.IdFeedbacks).HasColumnName("Id_feedbacks");

				entity.Property(e => e.FeedbackContent)
					.HasColumnType("text")
					.HasColumnName("feedback_content");

				entity.Property(e => e.IdAppointment).HasColumnName("Id_appointment");

				entity.Property(e => e.IdDoctor).HasColumnName("Id_doctor");

				entity.Property(e => e.IdPatient).HasColumnName("Id_patient");

				entity.HasOne(d => d.IdAppointmentNavigation)
					.WithMany(p => p.DoctorFeedbacksPatients)
					.HasForeignKey(d => d.IdAppointment)
					.HasConstraintName("FK__DoctorFee__Id_ap__5629CD9C");

				entity.HasOne(d => d.IdDoctorNavigation)
					.WithMany(p => p.DoctorFeedbacksPatientIdDoctorNavigations)
					.HasForeignKey(d => d.IdDoctor)
					.HasConstraintName("FK__DoctorFee__Id_do__5441852A");

				entity.HasOne(d => d.IdPatientNavigation)
					.WithMany(p => p.DoctorFeedbacksPatientIdPatientNavigations)
					.HasForeignKey(d => d.IdPatient)
					.HasConstraintName("FK__DoctorFee__Id_pa__5535A963");
			});

			modelBuilder.Entity<PatientReviewsDoctor>(entity =>
			{
				entity.HasKey(e => e.IdReviews)
					.HasName("PK__PatientR__3D824B49730A0FAD");

				entity.ToTable("PatientReviewsDoctor");

				entity.Property(e => e.IdReviews).HasColumnName("Id_reviews");

				entity.Property(e => e.IdAppointment).HasColumnName("Id_appointment");

				entity.Property(e => e.IdDoctor).HasColumnName("Id_doctor");

				entity.Property(e => e.IdPatient).HasColumnName("Id_patient");

				entity.Property(e => e.ReviewContent)
					.HasColumnType("text")
					.HasColumnName("review_content");

				entity.HasOne(d => d.IdAppointmentNavigation)
					.WithMany(p => p.PatientReviewsDoctors)
					.HasForeignKey(d => d.IdAppointment)
					.HasConstraintName("FK__PatientRe__Id_ap__5165187F");

				entity.HasOne(d => d.IdDoctorNavigation)
					.WithMany(p => p.PatientReviewsDoctorIdDoctorNavigations)
					.HasForeignKey(d => d.IdDoctor)
					.HasConstraintName("FK__PatientRe__Id_do__4F7CD00D");

				entity.HasOne(d => d.IdPatientNavigation)
					.WithMany(p => p.PatientReviewsDoctorIdPatientNavigations)
					.HasForeignKey(d => d.IdPatient)
					.HasConstraintName("FK__PatientRe__Id_pa__5070F446");
			});

			modelBuilder.Entity<Roll>(entity =>
			{
				entity.ToTable("Roll");

				entity.Property(e => e.RollId).HasColumnName("Roll_Id");

				entity.Property(e => e.RollName)
					.HasMaxLength(50)
					.IsUnicode(false)
					.HasColumnName("roll_name");
			});

			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}
