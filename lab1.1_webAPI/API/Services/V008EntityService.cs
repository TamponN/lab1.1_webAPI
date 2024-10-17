using API.Repositories;
using Data.Model;
using Share.DTOs;
using API.Mappers;
using API.Services.Interfaces;

namespace API.Services
{
    public class V008EntityService : IV008EntityService
    {
        private readonly V008EntityRepository _repository;

        public V008EntityService(V008EntityRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<DictionaryDTO> GetAll()
        {
            var entities = _repository.GetAllWithDeleted().ToList();
            return entities.Select(e => e.ToDictionaryDTO());
        }

        public async Task<DictionaryDTO?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByKeyAsync(id);
            return entity?.ToDictionaryDTO();
        }

        public async Task AddAsync(V008Entity entity)
        {
            _repository.Add(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateAsync(V008Entity entity)
        {
            _repository.Edit(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByKeyAsync(id);
            if (entity != null)
            {
                _repository.Delete(entity);
                await _repository.SaveChangesAsync();
            }
        }

        public async Task UploadFromFileAsync(IFormFile file)
        {
            // Удаляем все существующие записи
            var allEntities = _repository.GetAllWithDeleted().ToList();
            foreach (var entity in allEntities)
            {
                _repository.Delete(entity);
            }
            await _repository.SaveChangesAsync();

            // Читаем данные из файла и добавляем новые записи
            var entries = await ReadEntriesFromXmlFileAsync(file);

            foreach (var entry in entries)
            {
                _repository.Add(entry);
            }
            await _repository.SaveChangesAsync();
        }

        private async Task<List<V008Entity>> ReadEntriesFromXmlFileAsync(IFormFile file)
        {
            var entries = new List<V008Entity>();

            using (var stream = file.OpenReadStream())
            {
                var xmlReader = new DictionaryXmlReader<V008Entity>();
                entries = await xmlReader.ReadFromXmlStreamAsync(stream);
            }

            return entries;
        }
    }
}
