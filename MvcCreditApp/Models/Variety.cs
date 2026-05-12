using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MvcCreditApp.Models
{
    public class Variety
    {
        
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название сорта")]
        [Display(Name = "Название сорта")]
        public string Name { get; set; }

        [Display(Name = "Культура")]
        [Required(ErrorMessage = "Введите культуру")]
        public string Crop { get; set; }

        [Display(Name = "Селекционер/Производитель")]
        public string Breeder { get; set; }

        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public List<VarietyInfo> VarietyInfos { get; set; } = new List<VarietyInfo>();
    }
}

