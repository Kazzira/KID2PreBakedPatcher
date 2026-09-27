using System.Text.RegularExpressions;


namespace KID2PreBakedPatcher.Core.KID.LineColumns;


////////////////////////////////////////////////////////////////////////////
/// TRAIT RECORD
////////////////////////////////////////////////////////////////////////////
/// <summary>
/// Represents a trait that can be applied to an item, such as armor or weapon traits.
/// </summary>
public abstract record Trait;



////////////////////////////////////////////////////////////////////////////
/// ARMOR TRAITS
////////////////////////////////////////////////////////////////////////////
public abstract record ArmorTrait : Trait;
public abstract record ArmorTrait_Single : ArmorTrait;
public abstract record ArmorTrait_NonNegate : ArmorTrait_Single;
public record ArmorTrait_Negate(ArmorTrait_NonNegate Trait) : ArmorTrait_Single;

public record ArmorTrait_Multiple(ArmorTrait_Single[] Traits) : ArmorTrait;

public record ArmorTrait_Enchanted : ArmorTrait_NonNegate;
public record ArmorTrait_Template : ArmorTrait_NonNegate;

public record ArmorTrait_ArmorRating(decimal Min, decimal Max) : ArmorTrait_Single;
public record ArmorTrait_Weight(float Min, float Max) : ArmorTrait_Single;
public record ArmorTrait_BodySlot(int BodySlot) : ArmorTrait_Single;

public record ArmorTrait_Heavy : ArmorTrait_NonNegate;
public record ArmorTrait_Light : ArmorTrait_NonNegate;
public record ArmorTrait_Clothing : ArmorTrait_NonNegate;


////////////////////////////////////////////////////////////////////////////
/// WEAPON TRAITS
////////////////////////////////////////////////////////////////////////////
public abstract record WeaponTrait : Trait;
public abstract record WeaponTrait_Single : WeaponTrait;
public abstract record WeaponTrait_NonNegate : WeaponTrait_Single;
public record WeaponTrait_Negate(WeaponTrait_NonNegate Trait) : WeaponTrait_Single;
public record WeaponTrait_Multiple(WeaponTrait_Single[] Traits) : WeaponTrait;

public record WeaponTrait_Enchanted : WeaponTrait_NonNegate;
public record WeaponTrait_Template : WeaponTrait_NonNegate;
public record WeaponTrait_Weight(float Min, float Max) : WeaponTrait_Single;
public record WeaponTrait_Damage(decimal Min, decimal Max) : WeaponTrait_Single;

public record WeaponTrait_HandToHandMelee : WeaponTrait_NonNegate;
public record WeaponTrait_OneHandedSword : WeaponTrait_NonNegate;
public record WeaponTrait_OneHandedDagger : WeaponTrait_NonNegate;
public record WeaponTrait_OneHandedAxe : WeaponTrait_NonNegate;
public record WeaponTrait_OneHandedMace : WeaponTrait_NonNegate;
public record WeaponTrait_TwoHandedSword : WeaponTrait_NonNegate;
public record WeaponTrait_TwoHandedAxe : WeaponTrait_NonNegate;
public record WeaponTrait_TwoHandedMace : WeaponTrait_NonNegate;
public record WeaponTrait_Bow : WeaponTrait_NonNegate;
public record WeaponTrait_Crossbow : WeaponTrait_NonNegate;
public record WeaponTrait_Staff : WeaponTrait_NonNegate;


////////////////////////////////////////////////////////////////////////////
/// AMMO TRAITS
////////////////////////////////////////////////////////////////////////////
public abstract record AmmoTrait : Trait;
public abstract record AmmoTrait_Single : AmmoTrait;
public abstract record AmmoTrait_NonNegate : AmmoTrait_Single;
public record AmmoTrait_Negate(AmmoTrait_NonNegate Trait) : AmmoTrait_Single;
public record AmmoTrait_Multiple(AmmoTrait_Single[] Traits) : AmmoTrait;

public record AmmoTrait_Bolt : AmmoTrait_NonNegate;
public record AmmoTrait_Damage(decimal Min, decimal Max) : AmmoTrait_Single;


////////////////////////////////////////////////////////////////////////////
/// MAGIC EFFECT TRAITS
////////////////////////////////////////////////////////////////////////////
public abstract record MagicEffectTrait : Trait;

public abstract record MagicEffectTrait_Single : MagicEffectTrait;
public abstract record MagicEffectTrait_NonNegate : MagicEffectTrait_Single;
public record MagicEffectTrait_Negate(MagicEffectTrait_NonNegate Trait) : MagicEffectTrait_Single;

public record MagicEffectTrait_Multiple(MagicEffectTrait_Single[] Traits) : MagicEffectTrait;
public record MagicEffectTrait_Hostile : MagicEffectTrait_NonNegate;
public record MagicEffectTrait_Delivery : MagicEffectTrait_NonNegate;
public record MagicEffectTrait_CastingType : MagicEffectTrait_NonNegate;
public record MagicEffectTrait_Resistance(int Value) : MagicEffectTrait_Single;
public record MagicEffectTrait_Dispel : MagicEffectTrait_NonNegate;
public record MagicEffectTrait_School(Enums.MagicSchool School, int Min, int Max) : MagicEffectTrait_Single;


public static partial class TraitExtensions
{
    [GeneratedRegex(@"^AR\((\d+)\/(\d+)\)$")]
    private static partial Regex ArmorRatingRegex();

    [GeneratedRegex(@"^D\((\d+)\/(\d+)\)$")]
    private static partial Regex DamageRegex();

    [GeneratedRegex(@"^W\((\d+)\/(\d+)\)$")]
    private static partial Regex WeightRegex();



    extension(string str)
    {
        public Trait ToTraitColumn(Enums.Type type)
        {
            return type switch
            {
                Enums.Type.Armor       => str.ToArmorTraitColumn(),
                Enums.Type.Weapon      => str.ToWeaponTraitColumn(),
                Enums.Type.Ammo        => str.ToAmmoTraitColumn(),
                Enums.Type.MagicEffect => str.ToMagicEffectTraitColumn(),
                _ => throw new NotImplementedException($"Trait parsing not implemented for type {type}."),
            };
        }

        private ArmorTrait ToArmorTraitColumn()
        {
            var items = str.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();

            if (items.Count > 1)
            {
                return new ArmorTrait_Multiple([.. items.Select(i => i.ToArmorTraitColumn() as ArmorTrait_Single ?? throw new InvalidOperationException($"Invalid armor trait: {i}"))]);
            }

            var item = items[0];

            if (item.StartsWith('-'))
            {
                var innerTrait = item[1..].ToArmorTraitColumn() as ArmorTrait_NonNegate ?? throw new InvalidOperationException($"Invalid armor trait: {item}");
                return new ArmorTrait_Negate(innerTrait);
            }

            if (item.Length == 1)
            {
                if (item == "E") return new ArmorTrait_Enchanted();
                if (item == "T") return new ArmorTrait_Template();
                throw new InvalidOperationException($"Invalid armor trait: {item}");
            }

            if (ArmorRatingRegex().IsMatch(item))
            {
                var match = ArmorRatingRegex().Match(item);
                var min = decimal.Parse(match.Groups[1].Value);
                var max = decimal.Parse(match.Groups[2].Value);
                return new ArmorTrait_ArmorRating(min, max);
            }

            if (WeightRegex().IsMatch(item))
            {
                var match = WeightRegex().Match(item);
                var min = float.Parse(match.Groups[1].Value);
                var max = float.Parse(match.Groups[2].Value);
                return new ArmorTrait_Weight(min, max);
            }

            if (int.TryParse(item, out var intValue))
            {
                if (intValue >= 30 && intValue <= 61)
                {
                    return new ArmorTrait_BodySlot(intValue);
                }
                else
                {
                    throw new InvalidOperationException($"Invalid armor body slot value: {intValue}");
                }
            }

            if (item == "HEAVY") return new ArmorTrait_Heavy();
            if (item == "LIGHT") return new ArmorTrait_Light();
            if (item == "CLOTHING") return new ArmorTrait_Clothing();

            throw new InvalidOperationException($"Invalid armor trait: {item}");

        }

        private WeaponTrait ToWeaponTraitColumn()
        {
            var items = str.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();

            if (items.Count > 1)
            {
                return new WeaponTrait_Multiple([.. items.Select(i => i.ToWeaponTraitColumn() as WeaponTrait_Single ?? throw new InvalidOperationException($"Invalid weapon trait: {i}"))]);
            }

            var item = items[0];

            if (item.StartsWith('-'))
            {
                var innerTrait = item[1..].ToWeaponTraitColumn() as WeaponTrait_NonNegate ?? throw new InvalidOperationException($"Invalid weapon trait: {item}");
                return new WeaponTrait_Negate(innerTrait);
            }

            if (item.Length == 1)
            {
                if (item == "E") return new WeaponTrait_Enchanted();
                if (item == "T") return new WeaponTrait_Template();
                throw new InvalidOperationException($"Invalid weapon trait: {item}");
            }

            if (DamageRegex().IsMatch(item))
            {
                var match = DamageRegex().Match(item);
                var min = decimal.Parse(match.Groups[1].Value);
                var max = decimal.Parse(match.Groups[2].Value);
                return new WeaponTrait_Damage(min, max);
            }

            if (WeightRegex().IsMatch(item))
            {
                var match = WeightRegex().Match(item);
                var min = float.Parse(match.Groups[1].Value);
                var max = float.Parse(match.Groups[2].Value);
                return new WeaponTrait_Weight(min, max);
            }

            item = item.ToLowerInvariant();

            return item switch
            {
                "handtohandmelee" => new WeaponTrait_HandToHandMelee(),
                "onehandsword" => new WeaponTrait_OneHandedSword(),
                "onehanddagger" => new WeaponTrait_OneHandedDagger(),
                "onehandaxe" => new WeaponTrait_OneHandedAxe(),
                "onehandmace" => new WeaponTrait_OneHandedMace(),
                "twohandsword" => new WeaponTrait_TwoHandedSword(),
                "twohandaxe" => new WeaponTrait_TwoHandedAxe(),
                "twohandmace" => new WeaponTrait_TwoHandedMace(),
                "bow" => new WeaponTrait_Bow(),
                "crossbow" => new WeaponTrait_Crossbow(),
                "staff" => new WeaponTrait_Staff(),
                _ => throw new InvalidOperationException($"Invalid weapon trait: {item}"),
            };
        }

        private AmmoTrait ToAmmoTraitColumn()
        {
            // Implement ammo trait parsing logic here
            throw new NotImplementedException("Ammo trait parsing not implemented.");
        }

        private MagicEffectTrait ToMagicEffectTraitColumn()
        {
            // Implement magic effect trait parsing logic here
            throw new NotImplementedException("Magic effect trait parsing not implemented.");
        }
    }
}