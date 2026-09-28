using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.FormKeys.SkyrimSE;

namespace KID2PreBakedPatcher.App;


public class Patcher(IPatcherState<ISkyrimMod, ISkyrimModGetter> State)
{
    private Core.KID.Database Database { get; set; } = null!;
    private Core.KID.Database ReadAllKIDFiles()
    {
        //
        // Find all INI files with _KID.ini as the suffix.
        //
        var filePaths = State.DataFolderPath
            .EnumerateFiles(false, "*_KID.ini")
            .Select(filePath => filePath.ToString())
            .ToList();

        return new Core.KID.Database(new Core.KID.FileParser(new Core.KID.LineParser()), filePaths);
    }

    private void PatchActivators(List<Core.KID.Line> Lines)
    {

    }

    private void PatchAmmo(List<Core.KID.Line> Lines)
    {

    }

    private void PatchArmors(List<Core.KID.Line> Lines)
    {

    }

    private void PatchBooks(List<Core.KID.Line> Lines)
    {

    }

    private void PatchEnchantments(List<Core.KID.Line> Lines)
    {

    }

    private void PatchFlora(List<Core.KID.Line> Lines)
    {

    }

    private void PatchFurniture(List<Core.KID.Line> Lines)
    {

    }

    private void PatchIngredients(List<Core.KID.Line> Lines)
    {

    }

    private void PatchKeys(List<Core.KID.Line> Lines)
    {

    }

    private void PatchLocations(List<Core.KID.Line> Lines)
    {

    }

    private void PatchMagicEffects(List<Core.KID.Line> Lines)
    {

    }

    private void PatchMiscItems(List<Core.KID.Line> Lines)
    {

    }

    private void PatchPotions(List<Core.KID.Line> Lines)
    {

    }

    private void PatchRaces(List<Core.KID.Line> Lines)
    {

    }

    private void PatchScrolls(List<Core.KID.Line> Lines)
    {

    }

    private void PatchSpells(List<Core.KID.Line> Lines)
    {

    }

    private void PatchSoulGems(List<Core.KID.Line> Lines)
    {

    }

    private void PatchTalkingActivators(List<Core.KID.Line> Lines)
    {

    }

    private Func<IWeaponGetter, bool> GetWeaponFilterByTrait(Core.KID.LineColumns.WeaponTrait? Trait)
    {
        if (Trait is null)
        {
            return weapon => true;
        }

        return Trait switch
        {
            Core.KID.LineColumns.WeaponTrait_Multiple traits => weapon => traits.Traits.All(trait => GetWeaponFilterByTrait(trait)(weapon)),
            Core.KID.LineColumns.WeaponTrait_Negate traits => weapon => !GetWeaponFilterByTrait(traits.Trait)(weapon),
            Core.KID.LineColumns.WeaponTrait_Bow => weapon => weapon.Data?.AnimationType == WeaponAnimationType.Bow,
            Core.KID.LineColumns.WeaponTrait_Crossbow => weapon => weapon.Data?.AnimationType == WeaponAnimationType.Crossbow,
            Core.KID.LineColumns.WeaponTrait_HandToHandMelee => weapon => weapon.Data?.AnimationType == WeaponAnimationType.HandToHand,
            Core.KID.LineColumns.WeaponTrait_OneHandedAxe => weapon => weapon.Data?.AnimationType == WeaponAnimationType.OneHandAxe,
            Core.KID.LineColumns.WeaponTrait_OneHandedDagger => weapon => weapon.Data?.AnimationType == WeaponAnimationType.OneHandDagger,
            Core.KID.LineColumns.WeaponTrait_OneHandedMace => weapon => weapon.Data?.AnimationType == WeaponAnimationType.OneHandMace,
            Core.KID.LineColumns.WeaponTrait_OneHandedSword => weapon => weapon.Data?.AnimationType == WeaponAnimationType.OneHandSword,
            Core.KID.LineColumns.WeaponTrait_TwoHandedAxe => weapon => weapon.Data?.AnimationType == WeaponAnimationType.TwoHandAxe,
            Core.KID.LineColumns.WeaponTrait_TwoHandedSword => weapon => weapon.Data?.AnimationType == WeaponAnimationType.TwoHandSword,
            Core.KID.LineColumns.WeaponTrait_Damage damage => weapon => weapon.Critical?.Damage >= damage.Min && weapon.Critical?.Damage <= damage.Max,
            Core.KID.LineColumns.WeaponTrait_Weight weight => weapon => weapon.BasicStats?.Weight >= weight.Min && weapon.BasicStats?.Weight <= weight.Max,
            Core.KID.LineColumns.WeaponTrait_Enchanted => weapon => !weapon.ObjectEffect.IsNull,
            Core.KID.LineColumns.WeaponTrait_Template => weapon => !weapon.Template.IsNull,
            Core.KID.LineColumns.WeaponTrait_Staff => weapon => weapon.Data?.AnimationType == WeaponAnimationType.Staff,
            _ => throw new NotImplementedException($"Weapon trait '{Trait.GetType().Name}' is not implemented yet."),
        };
    }

    private Func<IWeaponGetter, bool> GetWeaponFilterByChance(Core.KID.LineColumns.Chance? Chance)
    {
        if (Chance is null)
        {
            return weapon => true;
        }

        return weapon =>
        {
            var random = new Random();
            var roll = random.NextDouble();
            return roll <= (double)Chance.Value;
        };
    }

    enum FilterType
    {
        Or,
        And,
        Exclude
    };

    private Func<IWeaponGetter, bool> GetWeaponFilter(Core.KID.LineColumns.Filter Filter, Core.KID.LineColumns.Trait? Trait, Core.KID.LineColumns.Chance? Chance)
    {
    
        Func<IWeaponGetter, bool> traitFilter = GetWeaponFilterByTrait(Trait as Core.KID.LineColumns.WeaponTrait);
        Func<IWeaponGetter, bool> chanceFilter = GetWeaponFilterByChance(Chance);


        switch (Filter)
        {
            case Core.KID.LineColumns.Filter_Or filters:
                return weapon => filters.Filters.Any(filter => GetWeaponFilterSingle(filter)(weapon)) && traitFilter(weapon) && chanceFilter(weapon);
            case Core.KID.LineColumns.Filter_And filters:
                return weapon => filters.Filters.All(filter => GetWeaponFilterSingle(filter)(weapon)) && traitFilter(weapon) && chanceFilter(weapon);
            default:
                return weapon => GetWeaponFilterSingle(Filter)(weapon) && traitFilter(weapon) && chanceFilter(weapon);
        }
    }

    private Func<IWeaponGetter, bool> GetWeaponFilterSingle(Core.KID.LineColumns.Filter Filter)
    {
        return Filter switch
        {
            Core.KID.LineColumns.Filter_And filters => weapon => filters.Filters.All(filter => GetWeaponFilterSingle(filter)(weapon)),
            Core.KID.LineColumns.Filter_Or filters => weapon => filters.Filters.Any(filter => GetWeaponFilterSingle(filter)(weapon)),
            Core.KID.LineColumns.Filter_Exclude        filter => weapon => !GetWeaponFilterSingle(filter.Filters)(weapon),
            Core.KID.LineColumns.Filter_NifPathFull    filter => weapon => weapon.Model?.File.GivenPath?.Equals(filter.NifPath, StringComparison.OrdinalIgnoreCase) ?? false,
            Core.KID.LineColumns.Filter_NifPathPartial filter => weapon => weapon.Model?.File.GivenPath?.Contains(filter.NifPath, StringComparison.OrdinalIgnoreCase) ?? false,
            Core.KID.LineColumns.Filter_EditorIDOrFullNameOrKeywordFull filter => weapon => (weapon.EditorID?.Equals(filter.Value, StringComparison.OrdinalIgnoreCase) ?? false)
                                || (weapon.Name?.String?.Equals(filter.Value, StringComparison.OrdinalIgnoreCase) ?? false)
                                || (weapon.Keywords?.Any(keyword => State.LinkCache.Resolve(keyword)?.EditorID?.Equals(filter.Value, StringComparison.OrdinalIgnoreCase) ?? false) ?? false),
            Core.KID.LineColumns.Filter_EditorIDOrFullNameOrKeywordPartial filter => weapon => (weapon.EditorID?.Contains(filter.Value, StringComparison.OrdinalIgnoreCase) ?? false)
                                || (weapon.Name?.String?.Contains(filter.Value, StringComparison.OrdinalIgnoreCase) ?? false)
                                || (weapon.Keywords?.Any(keyword => State.LinkCache.Resolve(keyword)?.EditorID?.Contains(filter.Value, StringComparison.OrdinalIgnoreCase) ?? false) ?? false),
            Core.KID.LineColumns.Filter_None => weapon => true,
            Core.KID.LineColumns.Filter_PluginName filter => weapon => weapon.FormKey.ModKey.Name.Equals(filter.PluginName, StringComparison.OrdinalIgnoreCase),
            _ => throw new NotImplementedException($"Filter '{Filter.GetType().Name}' is not implemented yet."),
        };
    }

    private void PatchWeapons(List<Core.KID.Line> Lines)
    {
        Console.WriteLine($"Patching {Lines.Count} weapons lines");

        var keywordToLines = Lines.GroupBy(line => line.Keyword)
            .ToDictionary(group => group.Key, group => group.ToList());
        
        var allWeapons = State.LoadOrder
                            .PriorityOrder
                            .WinningOverrides<IWeaponGetter>()
                            .ToList();

        // Get number of cores in cpu.
        var numCores = Environment.ProcessorCount - 1; // leave one core free for other tasks
        var weaponsPerCore = (int)Math.Ceiling((double)allWeapons.Count / numCores);
        var weaponsChunks = allWeapons.Chunk(weaponsPerCore).ToList();

        foreach (var keyword in keywordToLines.Keys)
        {
            Console.WriteLine($"Patching weapons with keyword {keyword} ({keywordToLines[keyword].Count} lines)");

            var tasks = new List<Task<List<IWeaponGetter>>>();

            foreach (var chunk in weaponsChunks)
            {
                tasks.Add(Task.Run(() =>
                {
                    var weapons = chunk
                        .Where(weap => keywordToLines[keyword].Any(line => GetWeaponFilter(line.Filter, line.Trait, line.Chance)(weap)))
                        .ToList();
                    return weapons;
                }));
            }

            Task.WaitAll(tasks.ToArray());

            var weapons = tasks.SelectMany(task => task.Result).ToList();
            
            Console.WriteLine($"Found {weapons.Count} weapons to patch");

            
            // Lookup the keyword as a form. If it does not exist, then create a new keyword with the given name.
            IKeywordGetter? keywordForm = null;
    
            switch (keyword)
            {
                case Core.KID.LineColumns.KeywordEditorID editorID:
                    if (!State.LinkCache.TryResolve<IKeywordGetter>(editorID.EditorID, out keywordForm))
                    {
                        var newKeyword = State.PatchMod.Keywords.AddNew(editorID.EditorID);
                        newKeyword.EditorID = editorID.EditorID;
                        keywordForm = newKeyword;
                    }
                    break;
                case Core.KID.LineColumns.KeywordFormID formID:
                    if (!State.LinkCache.TryResolve<IKeywordGetter>(formID.FormID.FormKey, out keywordForm))
                    {
                        throw new InvalidOperationException($"Keyword with FormID {formID.FormID:X8} does not exist in the load order.");
                    }
                    break;
            }

            foreach (var (i, weapon) in weapons.Select((weapon, i) => (i, weapon)))
            {
                Console.WriteLine($"Patching weapon {i + 1}/{weapons.Count}: {weapon.EditorID} ({weapon.FormKey})");
        
                if (weapon.Keywords?.Any(kw => State.LinkCache.Resolve(kw)?.EditorID == keywordForm?.EditorID) ?? false)
                {
                    continue;
                }
                var patchWeapon = State.PatchMod.Weapons.GetOrAddAsOverride(weapon);

                if (patchWeapon.Keywords is null)
                {
                    patchWeapon.Keywords = [];
                }

                patchWeapon.Keywords.Add(keywordForm?.FormKey ?? throw new InvalidOperationException($"Keyword form is null for keyword {keyword}."));
            }
        }
    }


    public void Run()
    {
        Database = ReadAllKIDFiles();

        Console.WriteLine($"Found {Database.LinesByType.Keys.Count} types of lines in the KID files.");
        Console.WriteLine($"Found {Database.LinesByType.Values.Sum(lines => lines.Count)} total lines in the KID files.");
        Console.WriteLine($"Found {Database.LinesByType[Core.KID.Enums.Type.Weapon].Count} weapon lines in the KID files.");

        foreach (var type in Database.LinesByType.Keys)
        {
            var lines = Database.LinesByType[type];

            switch (type)
            {
                case Core.KID.Enums.Type.Armor:
                    PatchArmors(lines);
                    break;
                case Core.KID.Enums.Type.Weapon:
                    PatchWeapons(lines);
                    break;
                case Core.KID.Enums.Type.MagicEffect:
                    PatchMagicEffects(lines);
                    break;
                case Core.KID.Enums.Type.Potion:
                    PatchPotions(lines);
                    break;
                case Core.KID.Enums.Type.Ammo:
                    PatchAmmo(lines);
                    break;
                case Core.KID.Enums.Type.Book:
                    PatchBooks(lines);
                    break;
                case Core.KID.Enums.Type.Enchantment:
                    PatchEnchantments(lines);
                    break;
                case Core.KID.Enums.Type.Scroll:
                    PatchScrolls(lines);
                    break;
                case Core.KID.Enums.Type.Location:
                    PatchLocations(lines);
                    break;
                case Core.KID.Enums.Type.Ingredient:
                    PatchIngredients(lines);
                    break;
                case Core.KID.Enums.Type.MiscItem:
                    PatchMiscItems(lines);
                    break;
                case Core.KID.Enums.Type.Key:
                    PatchKeys(lines);
                    break;
                case Core.KID.Enums.Type.SoulGem:
                    PatchSoulGems(lines);
                    break;
                case Core.KID.Enums.Type.Spell:
                    PatchSpells(lines);
                    break;
                case Core.KID.Enums.Type.Activator:
                    PatchActivators(lines);
                    break;
                case Core.KID.Enums.Type.Flora:
                    PatchFlora(lines);
                    break;
                case Core.KID.Enums.Type.Furniture:
                    PatchFurniture(lines);  
                    break;
                case Core.KID.Enums.Type.Race:
                    PatchRaces(lines);
                    break;
                case Core.KID.Enums.Type.TalkingActivator:
                    PatchTalkingActivators(lines);
                    break;
                default:
                    throw new NotImplementedException($"Patching for type '{type}' is not implemented yet.");
            }
        }
    }
}