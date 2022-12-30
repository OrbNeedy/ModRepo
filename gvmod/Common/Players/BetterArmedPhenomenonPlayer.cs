using gvmod.Content;
using Terraria.ModLoader;

namespace gvmod.Common.Players
{
    internal class BetterArmedPhenomenonPlayer : ModPlayer
    {
        public bool edenType;
        public bool sumeragiType;
        public bool improved;

        public override void ResetEffects()
        {
            edenType = false;
            sumeragiType = false;
            improved = false;
        }

        public override void PostUpdateBuffs()
        {
            AdeptPlayer adept = Player.GetModPlayer<AdeptPlayer>();
            if (improved)
            {
                Player.statDefense += 16;
                Player.endurance += 0.1f;
                Player.GetDamage<SeptimaDamageClass>() += 0.2f;

                adept.MaxSeptimalPower += 200;
                adept.PrimaryDamageEquipMult += 0.2f;
                adept.SecondaryDamageEquipMult += 0.2f;
                adept.SpecialDamageEquipMult += 0.2f;

                adept.SPRegenOverheatModifier += 1f;
                adept.SPRegenModifier += 0.5f;
                adept.APRegenModifier += 0.12f;
            } else if (sumeragiType || edenType)
            {
                Player.statDefense += 12;
                Player.GetDamage<SeptimaDamageClass>() += 0.15f;
                adept.MaxSeptimalPower += 140;
                adept.PrimaryDamageEquipMult += 0.12f;
                adept.SecondaryDamageEquipMult += 0.12f;
                adept.SpecialDamageEquipMult += 0.12f;

                if (sumeragiType)
                {
                    adept.SPRegenOverheatModifier += 0.5f;
                }
            }
        }

        public override void NaturalLifeRegen(ref float regen)
        {
            if (improved)
            {
                regen *= 1.2f;
            } else if (edenType)
            {
                regen *= 1.15f;
            }
        }
    }
}
