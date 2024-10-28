using Data.Model;
using Share.DTOs;

namespace API.Mappers
{
    /// <summary>
    /// Класс мапера для сериализации/десирилизации объектов V008Entity в DTO и обратно
    /// </summary>
    public static class V008Mapper
    {
        public static DictionaryDTO ToDictionaryDTO(this V008Entity v008EntityModel)
        {
            return new DictionaryDTO
            {
                BeginDate = v008EntityModel.BeginDate,
                EndDate = v008EntityModel.EndDate,
                Code = v008EntityModel.Code,
                Name = v008EntityModel.Name
            };
        }

        public static V008Entity ToV008Entity(this DictionaryDTO dictionaryDTO)
        {
            return new V008Entity
            {
                // Id = dictionaryDTO.Id,
                BeginDate = dictionaryDTO.BeginDate,
                EndDate = dictionaryDTO.EndDate,
                Code = dictionaryDTO.Code,
                Name = dictionaryDTO.Name
            };

        }
    }
}