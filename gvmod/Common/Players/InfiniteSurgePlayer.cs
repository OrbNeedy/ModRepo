using Terraria;
using Terraria.ModLoader;

namespace gvmod.Common.Players
{
    public class InfiniteSurgePlayer : ModPlayer
    {
        public bool InfiniteSurge { get; set; }
        public float lastSPValue { get; set; }

        public override void Initialize()
        {
            InfiniteSurge = false;
            lastSPValue = 1;
        }

        public override void ResetEffects()
        {
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
    }
}
