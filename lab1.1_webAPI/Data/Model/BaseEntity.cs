using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Model
{
    public class BaseEntity
    {
        // Уникальный идентификатор для сущности
        public int Id { get; set; }

        // Флаг, указывающий на удаление (логическое удаление)
        public bool IsDeleted { get; set; }

        // Дата создания сущности
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        // Дата последнего изменения сущности
        public DateTime EditDate { get; set; }

        // Дата, когда сущность была помечена на удаление
        public DateTime? DeleteDate { get; set; }

        // Идентификатор пользователя, который удалил запись
        public int? DeletedUserId { get; set; }
    }
}