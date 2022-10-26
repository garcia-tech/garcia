using System.ComponentModel;
using Garcia.Domain;

namespace Garcia.Infrastructure.Localization.Local.Sample.Models
{
    public class TestModel : Entity<long>
    {
        public TestModel(long id, string localizableText, string nonLocalizableText)
        {
            Id = id;
            LocalizableText = localizableText;
            NonLocalizableText = nonLocalizableText;
        }

        [Localizable(true)]
        public string LocalizableText { get; set; }
        public string NonLocalizableText { get; set; }
    }
}
