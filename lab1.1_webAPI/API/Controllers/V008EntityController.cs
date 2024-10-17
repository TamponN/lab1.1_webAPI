using API.Mappers;
using API.Services;
using Data.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/v1/V008Entity")]
    [ApiController]
    public class V008EntityController : ControllerBase
    {
        private readonly V008EntityService _service;

        public V008EntityController(V008EntityService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var V008Entities = _service.GetAll();
            return Ok(V008Entities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var V008Entity = await _service.GetByIdAsync(id);

            if (V008Entity == null)
            {
                return NotFound();
            }

            return Ok(V008Entity);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] V008Entity entity)
        {
            await _service.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity.ToDictionaryDTO());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] V008Entity entity)
        {
            if (id != entity.Id)
            {
                return BadRequest();
            }

            await _service.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("UploadFromFile")]
        public async Task<IActionResult> UploadFromFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не выбран");

            try
            {
                await _service.UploadFromFileAsync(file);
                return Ok("Данные успешно загружены и обновлены");
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Ошибка при загрузке файла: {ex.Message}\n{ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Произошла ошибка при обработке файла");
            }
        }
    }
}