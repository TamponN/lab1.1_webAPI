using Data.Model;

namespace API.Services.Interfaces
{
    public interface IDictionaryXmlReader<T> where T : V008Entity, new()
    {
        Task<List<T>> ReadFromXmlStreamAsync(Stream stream);
    }
}
