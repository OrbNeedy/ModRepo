using Terraria;
using Terraria.ModLoader;

namespace gvmod.Common.Players
{
    public class SeptimaBuffPlayer : ModPlayer
    {
        public bool InfiniteSurge { get; set; }
        public float lastSPValue { get; set; }
        public bool SeptimalShield { get; set; }
        public bool AlchemicalField { get; set; }
        public bool SeptimalBurst { get; set; }

        public override void Initialize()
        {
            InfiniteSurge = false;
            lastSPValue = 1;
            SeptimalShield = false;
            AlchemicalField = false;
        }

        public override void ResetEffects()
        {
            SeptimalShield = false;
            AlchemicalField = false;
            InfiniteSurge = false;
        }

        public override void PreUpdate()
        {
            AdeptPlayer adept = Player.GetModPlayer<AdeptPlayer>();
            if (InfiniteSurge)
            {
                if (adept.SeptimalPower < lastSPValue && !adept.IsOverheated)
                {
                    adept.SeptimalPower = lastSPValue;
                }
                else
                {
                    lastSPValue = adept.SeptimalPower;
                }
            }
            else
            {
                lastSPValue = adept.SeptimalPower;
            }
            base.PreUpdate();
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (SeptimalShield)
            {
                modifiers.FinalDamage *= 0.5f;
            }
            base.ModifyHurt(ref modifiers);
        }
    }
}
