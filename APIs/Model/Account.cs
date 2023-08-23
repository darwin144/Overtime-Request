using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APIs.Model
{
    [Table("tb_m_accounts")]
    public class Account
    {
        [Key]
        public Guid Id { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("employee_id")]
        public Guid Employee_id { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsUsed { get; set; }
        public int OTP { get; set; }
        public DateTime? ExpiredTime { get; set; }

        // kardinalitas
        public ICollection<AccountRole>? AccountRoles { get; set; }
        public Employee? Employee { get; set; }
    }
}