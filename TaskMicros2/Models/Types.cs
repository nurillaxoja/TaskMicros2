using System.ComponentModel.DataAnnotations;

namespace TaskMicros2.Models
{
    public class Types
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        public List<Datas> Data { get; set; }
    }
}
