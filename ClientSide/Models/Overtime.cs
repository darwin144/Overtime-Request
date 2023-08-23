using ClientSide.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientSide.Models
{
    public class Overtime
    {
        public Guid Id { get; set; }
        [Required]
        public DateTime StartOvertime { get; set; }
        [Required]
        public DateTime EndOvertime { get; set; }

        public DateTime SubmitDate { get; set; }
        [Required(ErrorMessage = "Deskripsi harus diisi!")]
        public string Deskripsi { get; set; }

        public int Paid { get; set; }

        public Status Status { get; set; }

        public Guid Employee_id { get; set; }

    }
}
