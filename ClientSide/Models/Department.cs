using System.ComponentModel.DataAnnotations.Schema;

namespace ClientSide.Models
{
    public class Department
    {
        public Guid Id { get; set; }


        public string Name { get; set; }
    }
}
