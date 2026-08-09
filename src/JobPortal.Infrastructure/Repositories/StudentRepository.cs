using Microsoft.EntityFrameworkCore;
using JobPortal.Application.Students;
using JobPortal.Domain.Entities;
using JobPortal.Infrastructure.Persistence;

namespace JobPortal.Infrastructure.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly AppDbContext _dbContext;

    public StudentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Student>> GetAllAsync()
    {
        return _dbContext.Students.ToListAsync();
    }

    public Task<Student?> GetByIdAsync(Guid id)
    {
        return _dbContext.Students.FirstOrDefaultAsync(student => student.Id == id);
    }

    public async Task AddAsync(Student student)
    {
        _dbContext.Students.Add(student);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Student student)
    {
        _dbContext.Students.Update(student);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Student student)
    {
        _dbContext.Students.Remove(student);
        await _dbContext.SaveChangesAsync();
    }
}