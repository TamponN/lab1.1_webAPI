using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class V008Entity : BaseEntity
    {
        [Display(Name = "Код")]
        public string Code { get; set; } = String.Empty;
        
        [Display(Name = "Начало")]
        public DateTime BeginDate { get; set; }

        [Display(Name = "Окончание")]
        // Для времени используем максимально возможное значение даты и времени, которое поддерживает постгре
        public DateTime EndDate { get; set; } = DateTime.SpecifyKind(new DateTime(2099, 12, 31, 23, 59, 59), DateTimeKind.Utc);
        
        [Display(Name = "Наименование")]
        public string Name { get; set; } = string.Empty;

        public V008Entity() {}
    }
}