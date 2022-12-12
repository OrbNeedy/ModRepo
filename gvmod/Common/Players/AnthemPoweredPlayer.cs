using gvmod.Content;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Common.Players
{
    internal class AnthemPoweredPlayer: ModPlayer
    {
        public bool anthemBuff;
        public int anthemBuffLevel;

        public override void Initialize()
        {
            anthemBuff = false;
            anthemBuffLevel = 1;
        }

        public override void ResetEffects()
        {
            anthemBuff = false;
            Player.GetModPlayer<AdeptPlayer>().anthemState = false;
        }

        public override void PostUpdateBuffs()
        {
            AdeptPlayer adept = Player.GetModPlayer<AdeptPlayer>();
            if (adept.hasMusesPendant)
            {
                anthemBuffLevel = 5;
            } else if (adept.hasBattlePod)
            {
                anthemBuffLevel = 3;
            } else
            {
                anthemBuffLevel = 1;
            }
            if (anthemBuff)
            {
                adept.anthemState = true;
                if (anthemBuffLevel > 1)
                {
                    adept.SPUsageModifier *= 0;
                }
                adept.SPRegenModifier *= (float)(5.5 * anthemBuffLevel);
                adept.SPRegenOverheatModifier *= 2;
                adept.specialDamageLevelMult *= (float)(1.01 * anthemBuffLevel);
                adept.secondaryDamageLevelMult *= (float)(1.01 * anthemBuffLevel);
                Player.GetDamage<SeptimaDamageClass>() *= (float)(1.01 * anthemBuffLevel);
            }
            base.PostUpdateBuffs();
        }
    }
}
