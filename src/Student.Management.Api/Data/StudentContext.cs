using Microsoft.EntityFrameworkCore;

namespace Student.Management.Api.Data;

public class StudentContext:DbContext
{
    public StudentContext(DbContextOptions<StudentContext> options)
        :base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
}