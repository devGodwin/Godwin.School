using Microsoft.EntityFrameworkCore;

namespace GradeAndAssessment.Api.Data;

public class StudentAssessmentContext:DbContext
{
    public StudentAssessmentContext(DbContextOptions<StudentAssessmentContext> options)
        :base(options)
    {
    }

    public DbSet<StudentAssessment> StudentAssessments { get; set; }
}