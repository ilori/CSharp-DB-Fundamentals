namespace P01_StudentSystem.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {
        }

        public StudentSystemContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Homework> HomeworkSubmissions { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlServer(
                    @"Server=DESKTOP-ELJB4JK\SQLEXPRESS;Database=StudentSystem;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StudentId);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsRequired()
                    .IsUnicode();

                entity.Property(e => e.PhoneNumber)
                    .HasColumnType("char(10)")
                    .IsUnicode(false)
                    .IsRequired(false);

                entity.Property(e => e.RegisteredOn)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.Birthday)
                    .HasColumnType("DATE2")
                    .IsRequired(false);
            });

            model.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.CourseId);

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode()
                    .IsRequired();

                entity.Property(e => e.StartDate)
                    .IsRequired()
                    .HasColumnType("DATE2");

                entity.Property(e => e.EndDate)
                    .IsRequired()
                    .HasColumnType("DATE2");
            });

            model.Entity<Resource>(entity =>
            {
                entity.HasKey(e => e.ResourceId);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode()
                    .IsRequired();

                entity.Property(e => e.Url)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(e => e.Course)
                    .WithMany(e => e.Resources)
                    .HasForeignKey(e => e.CourseId);
            });

            model.Entity<Homework>(entity =>
            {
                entity.HasKey(e => e.HomeworkId);

                entity.Property(e => e.Content)
                    .HasColumnType("varchar(max)");
                entity.Property(e => e.SubmissionTime)
                    .HasColumnType("DATE2");

                entity.HasOne(e => e.Course)
                    .WithMany(e => e.HomeworkSubmissions)
                    .HasForeignKey(e => e.CourseId);

                entity.HasOne(e => e.Student)
                    .WithMany(e => e.HomeworkSubmissions)
                    .HasForeignKey(e => e.StudentId);
            });

            model.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(e => new {e.CourseId, e.StudentId});

                entity.HasOne(e => e.Student)
                    .WithMany(e => e.CourseEnrollments)
                    .HasForeignKey(e => e.StudentId);

                entity.HasOne(e => e.Course)
                    .WithMany(e => e.StudentsEnrolled)
                    .HasForeignKey(e => e.CourseId);
            });
        }
    }
}