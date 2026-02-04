namespace FG
{
    public enum CharacterSaveSlot           // STATIC 10 SLOTS FOR SAVES
    {
        Slot_01 = 1,
        Slot_02,
        Slot_03,
        Slot_04,
        Slot_05,
        Slot_06,
        Slot_07,
        Slot_08,
        Slot_09,
        Slot_10,
    }

    public enum CharacterWeaponSocket       // WEAPON SOCKETS (RIGHT HAND, LEFT HAND, HIPS, BACK ETC.)
    {
        LEFT_HAND = 1,
        RIGHT_HAND,
        LEFT_HAND_SHIELD,
        BACK,
    }

    public enum WeaponMeleeAttackType       // WEAPON ACTION ATTACK TYPES
    {
        LIGHT_ATTACK_01 = 1,
        LIGHT_ATTACK_02,
        HEAVY_ATTACK_01,
        HEAVY_ATTACK_02,
        CHARGED_ATTACK_01,
        CHARGED_ATTACK_02,
        RUN_ATTACK_01,
        ROLL_ATTACK_01,
        BACKSTEP_ATTACK_01,
    }

    public enum CharacterTeam               // TO DETERMINE IF CHARACTER IS FRIENDLY OR NOT ETC.
    {
        Team01 = 1, // ENEMY
        Team02,     // FRIENDLY
    }

    public enum BossID                      // TO SET BOSS'S ID
    {
        DURK = 1,
    }

    public enum DamageIntensity             // DETERMINE BLOCK REACTION FROM GETTING DAMAGE
    {
        Ping = 1,
        Light,
        Medium,
        Heavy,
        Colossal
    }

    public enum WeaponType                  // FOR INSTANTIOTION SLOTS
    {
        WEAPON,
        SHIELD
    }

    public enum WeaponClass                 // FOR "UNEQUIPPED" SLOT CORRECT PLACEMENT
    {
        STRAIGHT_SWORD = 1,
        SHIELD,
        FIST,
    }

    public enum HitDirection                // DIRECTION OF HIT (IN PERSPECTIVE OF TARGET)
    {
        LEFT = 1,
        RIGHT,
        FRONT,
        BEHIND
    }
}
