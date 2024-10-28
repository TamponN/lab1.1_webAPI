using System.Text;
using System.Xml.Linq;
using API.Services.Interfaces;
using Data.Model;

namespace API.Services
{
    public class DictionaryXmlReader<T> : IDictionaryXmlReader<T> where T : V008Entity, new()
    {
        public async Task<List<T>> ReadFromXmlStreamAsync(Stream stream)
        {
            List<T> entries = new List<T>();

            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // Регистрация кодировок

                Encoding encoding = Encoding.GetEncoding("windows-1251"); // Устанавливаем кодировку

                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    XDocument xdoc = await XDocument.LoadAsync(reader, LoadOptions.None, CancellationToken.None);

                    var entryElements = xdoc.Descendants("zap");

                    foreach (var elem in entryElements)
                    {
                        T entry = new T();

                        var codeElement = elem.Element("IDVMP");
                        if (codeElement != null)
                        {
                            entry.Code = codeElement.Value;
                        }

                        var beginDateElement = elem.Element("DATEBEG");
                        if (beginDateElement != null && DateTime.TryParse(beginDateElement.Value, out DateTime beginDate))
                        {
                            entry.BeginDate = DateTime.SpecifyKind(beginDate, DateTimeKind.Utc);
                        }

                        var endDateElement = elem.Element("DATEEND");
                        if (endDateElement != null && DateTime.TryParse(endDateElement.Value, out DateTime endDate))
                        {
                            entry.EndDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);
                        }
                        else
                        {
                            // Для времени используем максимально возможное значение даты и времени, которое поддерживает постгре
                            entry.EndDate = DateTime.SpecifyKind(new DateTime(2099, 12, 31, 23, 59, 59), DateTimeKind.Utc);
                        }

                        var nameElement = elem.Element("VMPNAME");
                        if (nameElement != null)
                        {
                            entry.Name = nameElement.Value;
                        }

                        entries.Add(entry);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении XML файла: {ex.Message}\n{ex.StackTrace}");
                throw;
            }

            return entries;
        }
    }
}
