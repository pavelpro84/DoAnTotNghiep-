using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace courses_edu_be.Models
{
    public partial class CoursesEduContext : DbContext
    {
        public CoursesEduContext()
        {
        }

        public CoursesEduContext(DbContextOptions<CoursesEduContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Courses> Courses { get; set; }
        public virtual DbSet<CoursesDetail> CoursesDetail { get; set; }
        public virtual DbSet<Grade> Grade { get; set; }
        public virtual DbSet<GradeSubjectDetail> GradeSubjectDetail { get; set; }
        public virtual DbSet<Lesson> Lesson { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<SchoolSubject> SchoolSubject { get; set; }
        public virtual DbSet<SubCategory> SubCategory { get; set; }
        public virtual DbSet<SystemRole> SystemRole { get; set; }
        public virtual DbSet<SystemUser> SystemUser { get; set; }
        public virtual DbSet<UserDetail> UserDetail { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=OPEN-AI;Database=CoursesEdu;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.CategorySlug)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Courses>(entity =>
            {
                entity.Property(e => e.CoursesId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CoursesName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.CoursesNameSlug)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<CoursesDetail>(entity =>
            {
                entity.Property(e => e.CoursesDetailId).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Courses)
                    .WithMany(p => p.CoursesDetail)
                    .HasForeignKey(d => d.CoursesId)
                    .HasConstraintName("FK__CoursesDe__Cours__398D8EEE");

                entity.HasOne(d => d.Grade)
                    .WithMany(p => p.CoursesDetail)
                    .HasForeignKey(d => d.GradeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CoursesDe__Grade__3B75D760");

                entity.HasOne(d => d.SchoolSubject)
                    .WithMany(p => p.CoursesDetail)
                    .HasForeignKey(d => d.SchoolSubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CoursesDe__Schoo__3A81B327");
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.Property(e => e.GradeId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.GradeName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.GradeSlug)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<GradeSubjectDetail>(entity =>
            {
                entity.Property(e => e.GradeSubjectDetailId).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Grade)
                    .WithMany(p => p.GradeSubjectDetail)
                    .HasForeignKey(d => d.GradeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GradeSubj__Grade__31EC6D26");

                entity.HasOne(d => d.SchoolSubject)
                    .WithMany(p => p.GradeSubjectDetail)
                    .HasForeignKey(d => d.SchoolSubjectId)
                    .HasConstraintName("FK__GradeSubj__Schoo__30F848ED");
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.Property(e => e.LessonId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LessonName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.LessonSlug)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Courses)
                    .WithMany(p => p.Lesson)
                    .HasForeignKey(d => d.CoursesId)
                    .HasConstraintName("FK__Lesson__CoursesI__403A8C7D");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.QuestionId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Answer).HasMaxLength(10);

                entity.Property(e => e.OptionA).IsRequired();

                entity.Property(e => e.OptionB).IsRequired();

                entity.Property(e => e.OptionC).IsRequired();

                entity.Property(e => e.OptionD).IsRequired();

                entity.Property(e => e.QuestionContent).IsRequired();

                entity.HasOne(d => d.Lesson)
                    .WithMany(p => p.Question)
                    .HasForeignKey(d => d.LessonId)
                    .HasConstraintName("FK__Question__Lesson__440B1D61");
            });

            modelBuilder.Entity<SchoolSubject>(entity =>
            {
                entity.Property(e => e.SchoolSubjectId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.SchoolSubjectName).HasMaxLength(255);

                entity.Property(e => e.SchoolSubjectSlug).HasMaxLength(255);
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.Property(e => e.SubCategoryId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.SubCategoryName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.SubCategorySlug)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<SystemRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__SystemRo__8AFACE1A40740B1A");

                entity.Property(e => e.RoleName).HasMaxLength(255);
            });

            modelBuilder.Entity<SystemUser>(entity =>
            {
                entity.HasIndex(e => e.UserName)
                    .HasName("UQ__SystemUs__C9F28456A7BAEE7C")
                    .IsUnique();

                entity.Property(e => e.SystemUserId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.UserAvatar).HasMaxLength(255);

                entity.Property(e => e.UserDob).HasMaxLength(255);

                entity.Property(e => e.UserEmail).HasMaxLength(255);

                entity.Property(e => e.UserFullName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UserPhone).HasMaxLength(20);
            });

            modelBuilder.Entity<UserDetail>(entity =>
            {
                entity.Property(e => e.UserDetailId).HasDefaultValueSql("(newid())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
