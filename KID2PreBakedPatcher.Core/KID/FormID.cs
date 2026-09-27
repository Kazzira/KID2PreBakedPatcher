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
            // hex id must be 6 characters.

            var hexId = match.Groups[1].Value.PadLeft(6, '0');
            return new FormID(FormKey.Factory($"{hexId}:{match.Groups[2].Value}"));
        }
    }
}