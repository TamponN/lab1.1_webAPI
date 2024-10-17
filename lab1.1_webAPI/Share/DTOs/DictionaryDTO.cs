using System.ComponentModel.DataAnnotations;

namespace Share.DTOs
{
    public class DictionaryDTO : BaseDTO
    {
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "Начало")]
        public DateTime BeginDate { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "Окончание")]
        public DateTime EndDate { get; set; }
        
        [Display(Name = "Код")]
        public string Code { get; set; } = string.Empty;
        
        [Display(Name = "Наименование")]
        public string Name { get; set; } = string.Empty;
        
        // Нахуа они пользователю
        // public string Comments { get; set; } = string.Empty;
        
        public DictionaryDTO()
        {
            BeginDate = DateTime.SpecifyKind(new DateTime(2099, 12, 31, 23, 59, 59), DateTimeKind.Utc);
            EndDate = DateTime.SpecifyKind(new DateTime(2099, 12, 31, 23, 59, 59), DateTimeKind.Utc);
        }

    }
}