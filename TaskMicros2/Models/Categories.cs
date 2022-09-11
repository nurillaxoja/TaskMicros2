using System.ComponentModel.DataAnnotations;

namespace TaskMicros2.Models
{
    public class Categories
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Категория")]
        public string Category { get; set; }

        public List<Datas> Data { get; set; }
    }
}
