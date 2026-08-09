using JobPortal.Domain.Entities;

namespace JobPortal.Application.Students;

public class StudentService
{
    private readonly IStudentRepository _studentRepository;

    public StudentService(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<Student> CreateAsync(CreateStudentRequest request)
    {
        var student = new Student
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _studentRepository.AddAsync(student);

        return student;
    }

    public Task<List<Student>> GetAllAsync()
    {
        return _studentRepository.GetAllAsync();
    }
}