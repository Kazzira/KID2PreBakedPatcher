namespace KID2PreBakedPatcher.Tests.Core.KID.LineColumns;


using KID2PreBakedPatcher.Core.KID.LineColumns;
using KID2PreBakedPatcher.Core.KID.Enums;

public class TraitsArmorParseTests
{
    [Test]
    public void ParseArmorTrait_NegateEnchantment()
    {
        var input = "-E";
        var result = input.ToTraitColumn(Type.Armor);

        Assert.That(result.GetType(), Is.EqualTo(typeof(ArmorTrait_Negate)));
        Assert.That(((ArmorTrait_Negate)result).Trait.GetType(), Is.EqualTo(typeof(ArmorTrait_Enchanted)));
    }
}