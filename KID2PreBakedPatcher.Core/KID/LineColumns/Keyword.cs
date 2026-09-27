using Mutagen.Bethesda.Plugins;
using System.Text.RegularExpressions;

namespace KID2PreBakedPatcher.Core.KID.LineColumns;


public abstract record Keyword;
public record KeywordEditorID(string EditorID) : Keyword;
public record KeywordFormID(FormID FormID) : Keyword;


public static partial class KeywordExtensions
{
    [GeneratedRegex(@"^0x([0-9A-Fa-f]+)\~(.*?)$")]
    private static partial Regex FormIDRegex();

    extension(string str)
    {
        public Keyword ToKeywordColumn()
        {
            if (FormIDRegex().IsMatch(str))
            {
                return new KeywordFormID(str.ToFormID());
            }
            else
            {
                return new KeywordEditorID(str);
            }
        }
    }
}
