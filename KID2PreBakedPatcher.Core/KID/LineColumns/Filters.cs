using System.Text.RegularExpressions;
using KID2PreBakedPatcher.Core.KID.Enums;
using Mutagen.Bethesda.Plugins;

namespace KID2PreBakedPatcher.Core.KID.LineColumns;

public enum StringToColumnOption
{
    None,
    KeywordOnly,
}

public abstract record Filter;


public record Filter_StringFull(string Value) : Filter;
public record Filter_StringPartial(string Value) : Filter;


public record Filter_KeywordFull(string Value) : Filter_StringFull(Value);
public record Filter_NifPathFull(string NifPath) : Filter_StringFull(NifPath);
public record Filter_EditorIDOrFullNameOrKeywordFull(string Value) : Filter_StringFull(Value);


public record Filter_NifPathPartial(string NifPath) : Filter_StringPartial(NifPath);
public record Filter_EditorIDOrFullNameOrKeywordPartial(string Value) : Filter_StringPartial(Value);

public record Filter_PluginName(string PluginName) : Filter;
public record Filter_FormID(FormID FormID) : Filter;

public record Filter_ActorValue(Enums.ActorValue ActorValue) : Filter;
public record Filter_Archetype(Enums.Archetype Archetype) : Filter;

public record Filter_None : Filter;

public record Filter_And(List<Filter> Filters) : Filter;
public record Filter_Or(List<Filter> Filters) : Filter;

public record Filter_Exclude(Filter Filters) : Filter;


public  static partial class FilterExtensions
{
    [GeneratedRegex(@"^NONE$")]
    private static partial Regex NoneRegex();

    [GeneratedRegex(@"^.*?(\.esp|\.esm|\.esl)$")]
    private static partial Regex PluginNameRegex();

    [GeneratedRegex(@"^0x([0-9A-Fa-f]+)\~(.*?)$")]
    private static partial Regex FormIDRegex();

    [GeneratedRegex(@"^.*?\.nif$")]
    private static partial Regex NifPathRegex();


    private static readonly Enums.Type[] TypesWithActorValueFilters = [
        Enums.Type.Book,
        Enums.Type.MagicEffect,
        Enums.Type.Spell,
        Enums.Type.Enchantment,
        Enums.Type.Scroll,
        Enums.Type.Potion,
        Enums.Type.Weapon,
    ];

    public static readonly Enums.Type[] TypesWithArchetypeFilters = [
        Enums.Type.MagicEffect,
        Enums.Type.Spell,
        Enums.Type.Enchantment,
        Enums.Type.Scroll,
        Enums.Type.Potion,
    ];

    extension(string Filter)
    {
        public Filter ToColumnFilter(Enums.Type Type, StringToColumnOption Option = StringToColumnOption.None)
        {
            if (Filter.Contains(','))
            {
                var subFilters = Filter.Split(',').Select(f => f.Trim()).Where(f => !string.IsNullOrEmpty(f)).ToList();
                var filterObjects = subFilters.Select(f => f.ToColumnFilter(Type)).ToList();

                return new Filter_Or(filterObjects);
            }

            if (Filter.Contains('+'))
            {
                var subFilters = Filter.Split('+').Select(f => f.Trim()).Where(f => !string.IsNullOrEmpty(f)).ToList();
                var filterObjects = subFilters.Select(f => f.ToColumnFilter(Type, StringToColumnOption.KeywordOnly)).ToList();

                return new Filter_And(filterObjects);
            }
            if (TypesWithActorValueFilters.Contains(Type))
            {
                var actorValue = Filter.ToActorValueEnum();

                if (actorValue is not null)
                {
                    return new Filter_ActorValue(actorValue.Value);
                }
            }

            if  (TypesWithArchetypeFilters.Contains(Type))
            {
                var archetype = Filter.ToArchetypeEnum();

                if (archetype is not null)
                {
                    return new Filter_Archetype(archetype.Value);
                }
            }

            if (PluginNameRegex().IsMatch(Filter))
            {
                return new Filter_PluginName(Filter);
            }

            if (FormIDRegex().IsMatch(Filter))
            {
                return new Filter_FormID(Filter.ToFormID());
            }

            if (NifPathRegex().IsMatch(Filter))
            {
                if (Filter[0] == '*')
                {
                    return new Filter_NifPathPartial(Filter[1..]);
                }

                return new Filter_NifPathFull(Filter);
            }

            if (NoneRegex().IsMatch(Filter))
            {
                return new Filter_None();
            }

            if (Option == StringToColumnOption.KeywordOnly)
            {
                return new Filter_KeywordFull(Filter);
            }

            // If none of the above, treat it as an EditorID, FullName, or Keyword filter
            if (Filter[0] == '*')
            {
                return new Filter_EditorIDOrFullNameOrKeywordPartial(Filter[1..]);
            }

            return new Filter_EditorIDOrFullNameOrKeywordFull(Filter);
        }

    }
}