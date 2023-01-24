using gvmod.Content;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Common.Players
{
    public class WarPlayer : ModPlayer
    {
        public bool HaveBonus { get; set; }
        private int regenBonusTimer;
        private int damageBonusTimer;

        public override void Initialize()
        {
            regenBonusTimer = -1;
            damageBonusTimer = -1;
        }

        public override void PostUpdate()
        {
            AdeptPlayer adept = Player.GetModPlayer<AdeptPlayer>();
            if (regenBonusTimer >= 0) regenBonusTimer--;
            if (damageBonusTimer >= 0) damageBonusTimer--;

            if (regenBonusTimer >= 0)
            {
                adept.APRegenModifier *= 1.5f;
                adept.SPRegenModifier *= 1.8f;
                adept.SPRegenOverheatModifier *= 1.2f;
            }
            if (damageBonusTimer >= 0)
            {
                adept.PrimaryDamageEquipMult += 0.22f;
                adept.SecondaryDamageEquipMult += 0.22f;
                Player.GetDamage<SeptimaDamageClass>() += 0.22f;
            }

            if (HaveBonus)
            {
                if (adept.IsUsingSecondaryAbility) regenBonusTimer = 300;
                if (adept.IsUsingSpecialAbility) damageBonusTimer = 600;
            }
        }
    }
}
