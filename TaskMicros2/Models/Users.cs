using System.ComponentModel.DataAnnotations;

namespace TaskMicros2.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        public List<Datas> Data { get; set; }
    }
}
