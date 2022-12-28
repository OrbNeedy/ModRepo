using gvmod.Content;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Common.Players
{
    internal class AnthemPoweredPlayer: ModPlayer
    {
        public bool anthemBuff;

        public override void Initialize()
        {
            anthemBuff = false;
        }

        public override void ResetEffects()
        {
            anthemBuff = false;
            Player.GetModPlayer<AdeptPlayer>().AnthemLevel = 0;
        }

        public override void PostUpdateBuffs()
        {
            AdeptPlayer adept = Player.GetModPlayer<AdeptPlayer>();
            if (anthemBuff)
            {
                if (adept.HasDjinnLamp)
                {
                    adept.AnthemLevel = 5;
                }
                else if (adept.HasWholeMirror)
                {
                    adept.AnthemLevel = 3;
                }
                else
                {
                    adept.AnthemLevel = 1;
                }
                if (adept.AnthemLevel > 1)
                {
                    adept.SPUsageModifier *= 0;
                }
                adept.SPRegenModifier += (0.33f * adept.AnthemLevel);
                adept.SPRegenOverheatModifier += 2;
                adept.SpecialDamageEquipMult += (0.01f * adept.AnthemLevel);
                adept.SecondaryDamageEquipMult += (0.01f * adept.AnthemLevel);
                Player.GetDamage<SeptimaDamageClass>() += (0.01f * adept.AnthemLevel);
            }
            base.PostUpdateBuffs();
        }
    }
}
