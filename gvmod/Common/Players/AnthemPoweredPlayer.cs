using gvmod.Content;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Common.Players
{
    internal class AnthemPoweredPlayer: ModPlayer
    {
        public bool AnthemBuff { get; set; }

        public override void Initialize()
        {
            AnthemBuff = false;
        }

        public override void ResetEffects()
        {
            AnthemBuff = false;
            Player.GetModPlayer<AdeptMuse>().AnthemLevel = 0;
        }

        public override void PostUpdateBuffs()
        {
            AdeptMuse muse = Player.GetModPlayer<AdeptMuse>();
            AdeptPlayer adept = Player.GetModPlayer<AdeptPlayer>();
            if (AnthemBuff)
            {
                if (muse.HasDjinnLamp)
                {
                    muse.AnthemLevel = 5;
                }
                else if (muse.HasWholeMirror)
                {
                    muse.AnthemLevel = 3;
                }
                else
                {
                    muse.AnthemLevel = 1;
                }
                if (muse.AnthemLevel > 1)
                {
                    adept.SPUsageModifier *= 0;
                }
                adept.SPRegenModifier += (0.33f * muse.AnthemLevel);
                adept.SPRegenOverheatModifier += 2;
                adept.SpecialDamageEquipMult += (0.01f * muse.AnthemLevel);
                adept.SecondaryDamageEquipMult += (0.01f * muse.AnthemLevel);
                Player.GetDamage<SeptimaDamageClass>() += (0.01f * muse.AnthemLevel);
            }
            base.PostUpdateBuffs();
        }
    }
}
