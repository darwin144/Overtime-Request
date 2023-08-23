using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APIs.Model
{
    [Table("tb_tr_payrolls")]
    public class Payroll
    {
        [Key]
        public Guid Id { get; set; }
        [Column("payDate")]
        public DateTime PayDate { get; set; }
        [Column("payrollCut")]
        public int PayrollCuts { get; set; }
        [Column("totalSalary")]
        public int TotalSalary { get; set; }
        [Column("employee_id")]
        public Guid Employee_id { get; set; }

        // kardinalitas
        [JsonIgnore]
        public Employee? Employee { get; set; }
    }
}
