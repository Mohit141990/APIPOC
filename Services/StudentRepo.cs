using WebAPI5.Entities;
using WebAPI5.Models;
using WebAPI5.Search;

namespace WebAPI5.Services
{

    public class StudentRepo : IStudentRepo
    {
        IRepository<Student> _repoStudent;
        public StudentRepo(IRepository<Student> repoStudent)
        {
            _repoStudent = repoStudent;
        }
        public async Task<Student> SaveStudent(Student student)
        {
            try
            {
                student.DateOfBirth = DateTime.Now;
                await _repoStudent.InsertAsync(student);
            }
            catch (Exception ex)
            {

                throw;
            }
            return student;
        }
        public async Task<Student> UpdateStudent(Student student)
        {
            student.DateOfBirth = DateTime.Now;
            await _repoStudent.UpdateAsync(student);
            return student;
        }
        public IEnumerable<Student> GetAllStudent()
        {
            return _repoStudent.Query().AsTracking().Get().ToList();
        }

        public Student GetStudentById(long Id)
        {
            return _repoStudent.Query().Filter(s => s.StudentID.Equals(Id)).Get().FirstOrDefault();
        }

        public PagedListResult<Student> GetPagedItemsNew(SearchQuery<Student> query, out int totalItems)
        {
            return _repoStudent.Search(query, out totalItems);
        }
    }
}
