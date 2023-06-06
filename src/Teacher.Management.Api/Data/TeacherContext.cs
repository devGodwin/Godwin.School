using Microsoft.EntityFrameworkCore;

namespace Teacher.Management.Api.Data;

    public class TeacherContext:DbContext
    {
        public TeacherContext(DbContextOptions<TeacherContext>options)
            :base(options)
        {
        }

        public DbSet<Teacher> Teachers { get; set; }
    }

