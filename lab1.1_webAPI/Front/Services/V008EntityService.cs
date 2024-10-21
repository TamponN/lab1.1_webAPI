using Data.Model;

namespace Front.Services
{
    public class V008EntityService
    {
        private readonly HttpClient _httpClient;

        public V008EntityService (HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<List<V008Entity>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<V008Entity>>("api/v1/V008Entity");
        }

        public async Task<V008Entity> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<V008Entity>($"api/v1/V008Entity/{id}");
        }

        public async Task AddAsync(V008Entity entity)
        {
            await _httpClient.PostAsJsonAsync("api/v1/V008Entity", entity);
        }

        public async Task UpdateAsync(int id, V008Entity entity)
        {
            await _httpClient.PutAsJsonAsync($"api/v1/V008Entity/{id}", entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClient.DeleteAsync($"api/v1/V008Entity/{id}");
        }

        public async Task<HttpResponseMessage> UploadFileAsync(MultipartFormDataContent content)
        {
            return await _httpClient.PostAsync("api/v1/V008Entity/UploadFromFile", content);
        }

    }
}