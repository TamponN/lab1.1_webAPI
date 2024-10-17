using Data.Model;
using Share.DTOs;

namespace API.Mappers
{
    /// <summary>
    /// Класс мапера для преобразования ответа в DTO
    /// </summary>
    public static class V008Mapper
    {
        public static DictionaryDTO ToDictionaryDTO(this V008Entity v008EntityModel)
        {
            return new DictionaryDTO
            {
                Id = v008EntityModel.Id,
                BeginDate = v008EntityModel.BeginDate,
                EndDate = v008EntityModel.EndDate,
                Code = v008EntityModel.Code,
                Name = v008EntityModel.Name
            };
        }
    }
}