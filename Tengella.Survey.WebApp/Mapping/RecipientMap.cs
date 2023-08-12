using CsvHelper.Configuration;
using Tengella.Survey.Data.Models;

namespace Tengella.Survey.WebApp.Mapping
{
	public class RecipientMap : ClassMap<Recipient>
	{
		public RecipientMap()
		{
			Map(m => m.Email).Name("Email");
			Map(m => m.Name).Name("Name");
			Map(m => m.Type).Name("Type");
			Map(m => m.Identifier).Name("Identifier");
		}
	}
}
