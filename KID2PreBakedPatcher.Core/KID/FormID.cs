using Mutagen.Bethesda.Plugins;
using System.Text.RegularExpressions;


namespace KID2PreBakedPatcher.Core.KID;


public record FormID(FormKey FormKey);


public static partial class FormIDExtensions
{
    [GeneratedRegex(@"^0[Xx]([0-9A-Fa-f]+)\~(.*?)$")]
    private static partial Regex FormIDRegex();

    extension(FormID formID)
    {
        public FormKey ToFormKey()
        {
            return formID.FormKey;
        }
    }

    extension (string str)
    {
        public FormID ToFormID()
        {
            var match = FormIDRegex().Match(str);
            return new FormID(FormKey.Factory($"{match.Groups[1].Value}:{match.Groups[2].Value}"));
        }
    }
}