using API.Mappers;
using API.Services;
using API.Services.Interfaces;
using Data.Model;
using Microsoft.AspNetCore.Mvc;
using Share.DTOs;

namespace API.Controllers
{
    [Route("api/v1/V008Entity")]
    [ApiController]
    public class V008EntityController : ControllerBase
    {
        private readonly IV008EntityService _service;

        public V008EntityController(IV008EntityService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var V008Entities = _service.GetAll();
            return Ok(V008Entities);
        }

        [HttpGet("{code}")]
        public IActionResult GetByCode([FromRoute] string code)
        {
            var V008Entity = _service.GetByCode(code);

            if (V008Entity == null)
            {
                return NotFound();
            }

            return Ok(V008Entity);
        }

        [HttpGet("GetById{id}")]
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
        public async Task<IActionResult> Post([FromBody] DictionaryDTO entity)
        {
            if (entity == null)
            {
                return BadRequest("Entity is null");
            }
            await _service.AddAsync(entity.ToV008Entity());
            return Ok();
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
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Произошла ошибка при обработке файла");
            }
        }
    }
}