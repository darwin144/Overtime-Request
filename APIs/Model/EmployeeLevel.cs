using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APIs.Model
{
    [Table("tb_m_employeeLevels")]
    public class EmployeeLevel
    {
        [Key]
        public Guid Id { get; set; }
        [Column("title", TypeName = "Varchar(50)")]
        public string Title { get; set; }
        [Column("level", TypeName = "Varchar(20)")]
        public string Level { get; set; }
        [Column("salary")]
        public int Salary { get; set; }
        [Column("allowence")]
        public int? Allowence { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
