using FluentAPI;
using System.Data.Entity;

namespace DataAnnotations
{
    public class PlutoContext : DbContext
    {
        public PlutoContext()
            : base("name=PlutoContext")
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //TABLE NAME CHANGING...
            modelBuilder.Entity<Course>()
                .ToTable("tbl_Course", "catalog");

            //CHANGING NAME OF THE CLOUMN, SETTIN REQUIRED AND LENGHT
            modelBuilder.Entity<Course>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("Name");

            modelBuilder.Entity<Course>()
                .Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("Desc");

            //EACH COURSE HAS ONE AUTHOR, EACH AUTHOR HAS MANY COURSES MANY TO ONE RELATION
            modelBuilder.Entity<Course>()
                .HasRequired<Author>(c => c.Author)
                .WithMany(a => a.Courses)
                .HasForeignKey(c => c.AuthorId)
                .WillCascadeOnDelete(false);

            //RENAMING THE NAME OF MANY TO MANY TABLE NAME
            modelBuilder.Entity<Course>()
                .HasMany<Tag>(c => c.Tags)
                .WithMany(t => t.Courses)
                .Map(m =>
                 {
                     m.ToTable("CourseTags");
                     m.MapLeftKey("CourseId");
                     m.MapRightKey("TagId");
                 });


            //ONE TO ONE RELATION .... Principal because there will be not any cover that does not have a course.
            modelBuilder.Entity<Course>()
                .HasRequired<Cover>(c => c.Cover)
                .WithRequiredPrincipal(c => c.Course);
                


        }
    }
}