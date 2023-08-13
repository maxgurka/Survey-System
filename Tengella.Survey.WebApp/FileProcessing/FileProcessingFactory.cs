using Tengella.Survey.Data.Models;

namespace Tengella.Survey.WebApp.FileProcessing
{
	public static class FileProcessorFactory
	{
		public static List<Recipient> ProcessFile(Stream fileStream, string fileType)
		{
			return fileType.ToLower() switch
			{
				".csv" => CsvFileProcessor.ProcessFile(fileStream),
				".xlsx" => ExcelFileProcessor.ProcessFile(fileStream),
				_ => throw new NotSupportedException("File type not supported."),
			};
		}
	}
}
