using Microsoft.EntityFrameworkCore;

namespace Course.Management.Api.Data;

    public class CourseContext:DbContext
    {
        public CourseContext(DbContextOptions<CourseContext>options)
            :base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
    }

