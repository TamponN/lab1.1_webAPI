using FluentAssertions;
using System.Text;
using API.Services;
using Data.Model;

namespace Tests
{
    public class DictionaryXmlReaderTests
    {
        private readonly DictionaryXmlReader<V008Entity> _xmlReader;

        public DictionaryXmlReaderTests()
        {
            _xmlReader = new DictionaryXmlReader<V008Entity>();
        }

        [Fact]
        public async Task ReadFromXmlStreamAsync_ValidXml_ReturnsListOfEntities() // Тестим чтение валидного xml (ожидаем: ок)
        {
            // Arrange
            var xmlContent = @"
                <root>
                    <zap>
                        <IDVMP>001</IDVMP>
                        <DATEBEG>2023-01-01</DATEBEG>
                        <DATEEND>2023-12-31</DATEEND>
                        <VMPNAME>Сосал?</VMPNAME>
                    </zap>
                    <zap>
                        <IDVMP>002</IDVMP>
                        <DATEBEG>2023-01-01</DATEBEG>
                        <VMPNAME>Я устал</VMPNAME>
                    </zap>
                </root>";

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // Регистрация кодировок
            var encoding = Encoding.GetEncoding("windows-1251");
            using var stream = new MemoryStream(encoding.GetBytes(xmlContent));

            // Act
            var result = await _xmlReader.ReadFromXmlStreamAsync(stream);

            // Assert
            result.Should().HaveCount(2);

            result[0].Code.Should().Be("001");
            result[0].BeginDate.Should().Be(DateTime.SpecifyKind(DateTime.Parse("2023-01-01"), DateTimeKind.Utc));
            result[0].EndDate.Should().Be(DateTime.SpecifyKind(DateTime.Parse("2023-12-31"), DateTimeKind.Utc));
            result[0].Name.Should().Be("Сосал?");

            result[1].Code.Should().Be("002");
            result[1].BeginDate.Should().Be(DateTime.SpecifyKind(DateTime.Parse("2023-01-01"), DateTimeKind.Utc));
            result[1].EndDate.Should().Be(DateTime.SpecifyKind(new DateTime(2099, 12, 31, 23, 59, 59), DateTimeKind.Utc));
            result[1].Name.Should().Be("Я устал");
        }

        [Fact]
        public async Task ReadFromXmlStreamAsync_InvalidEncoding_ThrowsException() // Тестим чтение с кодировкой UTF-8 (ожидаем исключение)
        {
            // Arrange
            var xmlContent = @"
                <root>
                    <zap>
                        <VMPNAME>КИРИЛЛИЦА</VMPNAME>
                    </zap>
                </root>";
            var encoding = Encoding.UTF8; // Используем неправильную кодировку
            using var stream = new MemoryStream(encoding.GetBytes(xmlContent));

            // Act
            var result = await _xmlReader.ReadFromXmlStreamAsync(stream);

            // Assert
            result.Should().HaveCount(1);
            result[0].Name.Should().NotBe("КИРИЛЛИЦА"); // По идее, значение должно быть некорректным из-за другой кодировки
        }
    }
}
