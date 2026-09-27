namespace KID2PreBakedPatcher.Core.KID.Enums;


public enum Archetype
{
    ValueMod,
    Script,
    Dispel,
    CureDisease,
    Absorb,
    DualValueMod,
    Calm,
    Demoralize,
    Frenzy,
    Disarm,
    CommandSummoned,
    Invisibility,
    Light,
    Darkness,
    NightEye,
    Lock,
    Open,
    BoundWeapon,
    SummonCreature,
    DetectLife,
    Telekinesis,
    Paralysis,
    Reanimate,
    SoulTrap,
    TurnUndead,
    Guide,
    WerewolfFeed,
    CureParalysis,
    CureAddiction,
    CurePoison,
    Concussion,
    ValueAndParts,
    AccumulateMagnitude,
    Stagger,
    PeakValueMod,
    Cloak,
    Werewolf,
    SlowTime,
    Rally,
    EnhanceWeapon,
    SpawnHazard,
    Etherealize,
    Banish,
    SpawnScriptedRef,
    Disguise,
    GrabActor,
    VampireLord
}


public static class ArchetypeExtensions
{
    extension(string ArchetypeString)
    {
        public Archetype? ToArchetypeEnum()
        {
            foreach (var archetype in Enum.GetValues<Archetype>())
            {
                if (archetype.ToString().Equals(ArchetypeString, StringComparison.OrdinalIgnoreCase))
                {
                    return archetype;
                }
            }

            return null;
        }
    }
}