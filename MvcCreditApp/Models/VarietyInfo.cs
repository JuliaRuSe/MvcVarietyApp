using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcCreditApp.Models
{
    public class VarietyInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Сорт")]
        public int VarietyId { get; set; }

        [Required(ErrorMessage = "Введите год")]
        [Range(2000, 2026, ErrorMessage = "Год должен быть от 2000 до 2026")]
        [Display(Name = "Год")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Выберите всхожесть")]
        [Display(Name = "Всхожесть")]
        [Range(1, 3, ErrorMessage = "Значение от 1 до 3")]
        public int Germination { get; set; }

        [Required(ErrorMessage = "Выберите урожайность")]
        [Display(Name = "Урожайность")]
        [Range(1, 3, ErrorMessage = "Значение от 1 до 3")]
        public int Productivity { get; set; }

        [Display(Name = "Примечания")]
        public string Notes { get; set; }

        [ForeignKey("VarietyId")]
        public virtual Variety Variety { get; set; }

        [Display(Name = "Общая сумма баллов")]
        public int TotalScore => Germination + Productivity;

        [Display(Name = "Оценка всхожести")]
        public string GerminationText
        {
            get
            {
                return Germination switch
                {
                    3 => "Высокая",
                    2 => "Средняя",
                    1 => "Низкая",
                    _ => "Не указано"
                };
            }
        }

        [Display(Name = "Оценка урожайности")]
        public string ProductivityText
        {
            get
            {
                return Productivity switch
                {
                    3 => "Высокая",
                    2 => "Средняя",
                    1 => "Низкая",
                    _ => "Не указано"
                };
            }
        }
    }
}

