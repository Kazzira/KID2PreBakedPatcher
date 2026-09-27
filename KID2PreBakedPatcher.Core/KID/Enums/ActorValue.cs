namespace KID2PreBakedPatcher.Core.KID.Enums;


public enum ActorValue
{
    Aggression,
    Confidence,
    Energy,
    Morality,
    Mood,
    Assistance,
    OneHanded,
    TwoHanded,
    Marksman,
    Block,
    Smithing,
    HeavyArmor,
    LightArmor,
    Pickpocket,
    Lockpicking,
    Sneak,
    Alchemy,
    Speechcraft,
    Alteration,
    Conjuration,
    Destruction,
    Illusion,
    Restoration,
    Enchanting,
    Health,
    Magicka,
    Stamina,
    HealRate,
    MagickaRate,
    StaminaRate,
    SpeedMult,
    InventoryWeight,
    CarryWeight,
    CritChance,
    MeleeDamage,
    UnarmedDamage,
    Mass,
    VoicePoints,
    VoiceRate,
    DamageResist,
    PoisonResist,
    FireResist,
    ElectricResist,
    FrostResist,
    MagicResist,
    DiseaseResist,
    PerceptionCondition,
    EnduranceCondition,
    LeftAttackCondition,
    RightAttackCondition,
    LeftMobilityCondition,
    RightMobilityCondition,
    BrainCondition,
    Paralysis,
    Invisibility,
    NightEye,
    DetectLifeRange,
    WaterBreathing,
    WaterWalking,
    IgnoreCrippledLimbs,
    Fame,
    Infamy,
    JumpingBonus,
    WardPower,
    RightItemCharge,
    ArmorPerks,
    ShieldPerks,
    WardDeflection,
    Variable01,
    Variable02,
    Variable03,
    Variable04,
    Variable05,
    Variable06,
    Variable07,
    Variable08,
    Variable09,
    Variable10,
    BowSpeedBonus,
    FavorActive,
    FavorsPerDay,
    FavorsPerDayTimer,
    LeftItemCharge,
    AbsorbChance,
    Blindness,
    WeaponSpeedMult,
    ShoutRecoveryMult,
    BowStaggerBonus,
    Telekinesis,
    FavorPointsBonus,
    LastBribedIntimidated,
    LastFlattered,
    MovementNoiseMult,
    BypassVendorStolenCheck,
    BypassVendorKeywordCheck,
    WaitingForPlayer,
    OneHandedMod,
    TwoHandedMod,
    MarksmanMod,
    BlockMod,
    SmithingMod,
    HeavyArmorMod,
    LightArmorMod,
    PickPocketMod,
    LockpickingMod,
    SneakMod,
    AlchemyMod,
    SpeechcraftMod,
    AlterationMod,
    ConjurationMod,
    DestructionMod,
    IllusionMod,
    RestorationMod,
    EnchantingMod,
    OneHandedSkillAdvance,
    TwoHandedSkillAdvance,
    MarksmanSkillAdvance,
    BlockSkillAdvance,
    SmithingSkillAdvance,
    HeavyArmorSkillAdvance,
    LightArmorSkillAdvance,
    PickPocketSkillAdvance,
    LockpickingSkillAdvance,
    SneakSkillAdvance,
    AlchemySkillAdvance,
    SpeechcraftSkillAdvance,
    AlterationSkillAdvance,
    ConjurationSkillAdvance,
    DestructionSkillAdvance,
    IllusionSkillAdvance,
    RestorationSkillAdvance,
    EnchantingSkillAdvance,
    LeftWeaponSpeedMult,
    DragonSouls,
    CombatHealthRegenMult,
    OneHandedPowerMod,
    TwoHandedPowerMod,
    MarksmanPowerMod,
    BlockPowerMod,
    SmithingPowerMod,
    HeavyArmorPowerMod,
    LightArmorPowerMod,
    PickPocketPowerMod,
    LockpickingPowerMod,
    SneakPowerMod,
    AlchemyPowerMod,
    SpeechcraftPowerMod,
    AlterationPowerMod,
    ConjurationPowerMod,
    DestructionPowerMod,
    IllusionPowerMod,
    RestorationPowerMod,
    EnchantingPowerMod,
    DragonRend,
    AttackDamageMult,
    HealRateMult,
    MagickaRateMult,
    StaminaRateMult,
    WerewolfPerks,
    VampirePerks,
    GrabActorOffset,
    Grabbed,
    DEPRECATED05,
    ReflectDamage
}


public static class ActorValueExtensions
{
    extension(string ActorValueString)
    {
        public ActorValue? ToActorValueEnum()
        {
            return Enum.TryParse<ActorValue>(ActorValueString, ignoreCase: true, out var actorValue) ? actorValue : null;
        }
    }

    extension(int ActorValueIndex)
    {
        public ActorValue? ToActorValueEnum()
        {
            if (Enum.IsDefined(typeof(ActorValue), ActorValueIndex))
            {
                return (ActorValue)ActorValueIndex;
            }
            return null;
        }
    }

    extension(ActorValue ActorValueEnum)
    {
        public string ToActorValueString()
        {
            return ActorValueEnum.ToString();
        }

        public int ToActorValueIndex()
        {
            return (int)ActorValueEnum;
        }
    }
}