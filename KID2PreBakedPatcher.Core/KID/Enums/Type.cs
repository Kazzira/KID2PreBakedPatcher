namespace KID2PreBakedPatcher.Core.KID.Enums;


public enum Type
{
    [System.ComponentModel.Description("Weapon")]
    Weapon,
    [System.ComponentModel.Description("Armor")]
    Armor,
    [System.ComponentModel.Description("Ammo")]
    Ammo,
    [System.ComponentModel.Description("Magic Effect")]
    MagicEffect,
    [System.ComponentModel.Description("Potion")]
    Potion,
    [System.ComponentModel.Description("Scroll")]
    Scroll,
    [System.ComponentModel.Description("Location")]
    Location,
    [System.ComponentModel.Description("Ingredient")]
    Ingredient,
    [System.ComponentModel.Description("Book")]
    Book,
    [System.ComponentModel.Description("Misc Item")]
    MiscItem,
    [System.ComponentModel.Description("Key")]
    Key,
    [System.ComponentModel.Description("Soul Gem")]
    SoulGem,
    [System.ComponentModel.Description("Spell")]
    Spell,
    [System.ComponentModel.Description("Activator")]
    Activator,
    [System.ComponentModel.Description("Flora")]
    Flora,
    [System.ComponentModel.Description("Furniture")]
    Furniture,
    [System.ComponentModel.Description("Race")]
    Race,
    [System.ComponentModel.Description("Talking Activator")]
    TalkingActivator,
    [System.ComponentModel.Description("Enchantment")]
    Enchantment
}


public static class TypeExtensions
{
    public static string GetDescription(this Type type)
    {
        var fieldInfo = type.GetType().GetField(type.ToString());
        var descriptionAttributes = (System.ComponentModel.DescriptionAttribute[])(fieldInfo?.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false) ?? Array.Empty<System.ComponentModel.DescriptionAttribute>());
        return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : type.ToString();
    }
}

public static class StringExtensions
{
    public static Type? ToTypeColumn(this string str)
    {
        foreach (Type type in Enum.GetValues<Type>())
        {
            if (type.GetDescription().Equals(str, StringComparison.OrdinalIgnoreCase))
            {
                return type;
            }
        }

        return null;
    }
}