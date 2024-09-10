using WebAPI5.Entities;
using WebAPI5.Search;

namespace WebAPI5.Services
{
    public interface IStudentRepo
    {
        IEnumerable<Student> GetAllStudent();
        Student GetStudentById(long Id);
        Task<Student> SaveStudent(Student student);
        Task<Student> UpdateStudent(Student student);
        PagedListResult<Student> GetPagedItemsNew(SearchQuery<Student> query, out int totalItems);
    }
}
