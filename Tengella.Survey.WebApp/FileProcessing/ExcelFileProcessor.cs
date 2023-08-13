using OfficeOpenXml;
using Tengella.Survey.Data.Models;

namespace Tengella.Survey.WebApp.FileProcessing
{
	public static class ExcelFileProcessor
	{
		public static List<Recipient> ProcessFile(Stream fileStream)
		{
			var recipients = new List<Recipient>();

			using (var package = new ExcelPackage(fileStream))
			{
				var worksheet = package.Workbook.Worksheets[0]; // Assuming the data is in the first worksheet

				for (int row = 2; row <= worksheet.Dimension.Rows; row++) // Assuming the first row is the header
				{
					recipients.Add(new Recipient
					{
						Email = worksheet.Cells[row, 1].Value?.ToString(),
						Name = worksheet.Cells[row, 2].Value?.ToString(),
						Type = Enum.Parse<RecipientType>(worksheet.Cells[row, 3].Value?.ToString()),
						Identifier = worksheet.Cells[row, 4].Value?.ToString(),
					});
				}
			}

			return recipients;
		}
	}
}
