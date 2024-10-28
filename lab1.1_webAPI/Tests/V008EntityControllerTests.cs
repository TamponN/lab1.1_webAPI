using Data.Model;
using API.Controllers;
using API.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;
using Share.DTOs;
using FluentAssertions;

namespace Tests;

public class V008EntityControllerTests
{

    private readonly Mock<IV008EntityService> _serviceMock;
    private readonly V008EntityController _controller;

    public V008EntityControllerTests()
    {
        _serviceMock = new Mock<IV008EntityService>();
        _controller = new V008EntityController(_serviceMock.Object);
    }

    #region Тесты GetAll
    [Fact]
    public void GetAll_ReturnsOkResult_WithListOfEntities() // Тест для проврки метода GetAll (ожидаем: 200)
    {
        // Arrange
        var entities = new List<V008Entity>
        {
            new V008Entity { Id = 1, Code = "001", Name = "Test1" },
            new V008Entity { Id = 2, Code = "002", Name = "test2" }
        };
        _serviceMock.Setup(s => s.GetAll()).Returns(entities);

        // Act
        var result = _controller.GetAll();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(entities);
    }
    #endregion

    #region Тесты GetByCode
    [Fact]
    public void GetByCode_ExistingCode_ReturnsOkResult_WithDictionaryDTO() // Тест для проверки метода GetByCode (ожидаем: 200)
    {
        // Arrange
        var code = "001";
        var entity = new DictionaryDTO { Code = code, Name = "test1" };
        _serviceMock.Setup(s => s.GetByCode(code)).Returns(entity);

        // Act
        var result = _controller.GetByCode(code);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(entity);
    }

    [Fact]
    public void GetByCode_NonExistingCode_ReturnsNotFoundResult() // Тест для проверки метода GetByCode (ожидаем: 404)
    {
        // Arrange
        var code = "999";
        _serviceMock.Setup(s => s.GetByCode(code)).Returns((DictionaryDTO)null);

        // Act
        var result = _controller.GetByCode(code);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
    #endregion

    #region Тесты GetById
    [Fact]
    public async Task GetById_ExistingId_ReturnsOkResult_WithEntity() // Тест метода GetByIdAsync (ожидаем: 200)
    {
        // Arrange
        var id = 1;
        var entity = new V008Entity { Id = id, Code = "001", Name = "еууу" };
        _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(entity); // Запускаем ассинхронно

        // Act
        var result = await _controller.GetById(id);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(entity);
    }

    [Fact]
    public async Task GetById_NonExistingId_ReturnsNotFoundResult() // Тест метода GetByIdAsync (ожидаем: 404)
    {
        // Arrange
        var id = 999;
        _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((V008Entity)null);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
    #endregion

    #region Тесты POST
    [Fact]
    public async Task Post_NullEntity_ReturnsBadRequest() // Тестируем POST-метод с пустой полезной нагрузкой (ожидаем: "Entity is null")
    {
        // Arrange
        DictionaryDTO dto = null;

        // Act
        var result = await _controller.Post(dto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>().Which.Value.Should().Be("Entity is null");
    }
    #endregion

    #region Тесты PUT
    [Fact]
    public async Task Put_ValidEntity_ReturnsNoContent() // Тестим метод PUT с валидными данными  (ожидаем: 200 (но по факту NoContent))
    {
        // Arrange
        var id = 1;
        var entity = new V008Entity { Id = id, Code = "001", Name = "ОбновлённаяСущность" };

        _serviceMock.Setup(s => s.UpdateAsync(entity)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Put(id, entity);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _serviceMock.Verify(s => s.UpdateAsync(entity), Times.Once);
    }

    [Fact]
    public async Task Put_IdMismatch_ReturnsBadRequest() // Тестим метод PUT с ложным id (ожидаем: 400)
    {
        // Arrange
        var id = 1;
        var entity = new V008Entity { Id = 2, Code = "001", Name = "ОбновлённаяСущность" };

        // Act
        var result = await _controller.Put(id, entity);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }
    #endregion

    #region Тесты DELETE
    [Fact]
    public async Task Delete_ExistingId_ReturnsNoContent() // Тестим метод DELETE с существующим id (ожидаем: 200 (но по факту NoContent)
    {
        // Arrange
        var id = 1;
        _serviceMock.Setup(s => s.DeleteAsync(id)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _serviceMock.Verify(s => s.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task Delete_NonExistingId_ReturnsNotFound() // Тест для метода DELETE с несуществующим id (ожидаем: 404) 
    {
        // Arrange
        var id = 999;
        _serviceMock.Setup(s => s.DeleteAsync(id)).ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await _controller.Delete(id);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        _serviceMock.Verify(s => s.DeleteAsync(id), Times.Once);
    }
    #endregion

    #region Тесты Загрузки файла
    [Fact]
    public async Task UploadFromFile_ValidFile_ReturnsOkResult() // Тестим загрузку валидного файла(.xml)(Ожидаем ответ: "Данные успешно загружены и обновлены")
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(1024);
        fileMock.Setup(f => f.FileName).Returns("testfile.xml");
        _serviceMock.Setup(s => s.UploadFromFileAsync(fileMock.Object)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UploadFromFile(fileMock.Object);

        // Assert
        result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be("Данные успешно загружены и обновлены");
        _serviceMock.Verify(s => s.UploadFromFileAsync(fileMock.Object), Times.Once);
    }

    [Fact]
    public async Task UploadFromFile_NullFile_ReturnsBadRequest() // Тестим загрузку пустого файла или его отсутсвие (ожидаем: "Файл не выбран")
    {
        // Arrange
        IFormFile file = null;

        // Act
        var result = await _controller.UploadFromFile(file);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>().Which.Value.Should().Be("Файл не выбран");
    }

    [Fact]
    public async Task UploadFromFile_ExceptionThrown_ReturnsInternalServerError() // Тестим метод, который обрабатывает любые исключения (ожидаем: 500)
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(1024);
        fileMock.Setup(f => f.FileName).Returns("testfile.xml");
        _serviceMock.Setup(s => s.UploadFromFileAsync(fileMock.Object)).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.UploadFromFile(fileMock.Object);

        // Assert
        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        objectResult.Value.Should().Be("Произошла ошибка при обработке файла");
    }
    #endregion
}