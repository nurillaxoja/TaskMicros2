using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TaskMicros2.Models
{
    public class Datas
    {

        [Key]
        public int Id { get; set; }


        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата началы")]
        public DateTime StartDate { get; set; }


        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата конца")]
        public DateTime LastDate { get; set; }

        [Required]
        public double Amount { get; set; }

        [Display(Name = "Комментарий")]
        public string? Commentary { get; set; }




        #region for foregin key

        public Types Type { get; set; }

        [Display(Name = "Тип платежа")]
        public int TypeId { get; set; }


        public Categories Category { get; set; }

        [Display(Name = "Категория")]
        public int CategoryId { get; set; }

        public Users User { get; set; }


        [Display(Name = "Пользователь")]
        public int UserId { get; set; }

        #endregion for foregin key

    }
}
