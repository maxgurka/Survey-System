using System.Globalization;
using Tengella.Survey.Data.Models;
using Tengella.Survey.WebApp.Mapping;

namespace Tengella.Survey.WebApp.FileProcessing
{
	public static class CsvFileProcessor
	{
		public static List<Recipient> ProcessFile(Stream fileStream)
		{
			var recipients = new List<Recipient>();

			using (var streamReader = new StreamReader(fileStream))
			using (var csvReader = new CsvHelper.CsvReader(streamReader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = true,
				HeaderValidated = null,
				MissingFieldFound = null,
			}))
			{
				csvReader.Context.RegisterClassMap<RecipientMap>();
				recipients = csvReader.GetRecords<Recipient>().ToList();
			}

			return recipients;
		}
	}
}
