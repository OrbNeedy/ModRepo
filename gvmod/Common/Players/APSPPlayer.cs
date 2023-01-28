using Terraria;
using Terraria.ModLoader;

namespace gvmod.Common.Players
{
    public class APSPPlayer : ModPlayer
    {
        private int globalTimer;
        private float apLimiter;
        private float spLimiter;
        public float ApAdded { get; set; }
        public float SpAdded { get; set; }
        public override void Initialize()
        {
            globalTimer = 0;
            apLimiter = 0;
            spLimiter = 0;
            ApAdded = 0;
            SpAdded = 0;
        }

        public override void ResetEffects()
        {
            ApAdded = 0;
            SpAdded = 0;
        }

        public override void PostUpdate()
        {
            globalTimer--;
            if (globalTimer <= 0)
            {
                globalTimer = 60;
                apLimiter = 0;
                spLimiter = 0;
            }
        }

        public override void OnHitAnything(float x, float y, Entity victim)
        {
            AdeptPlayer adept = Player.GetModPlayer<AdeptPlayer>();
            if (apLimiter < 1 / 3820f)
            {
                apLimiter += 1 / 4020f * ApAdded;
                adept.AbilityPower += 1 / 4020f * ApAdded;
            }

            if (spLimiter < 30f)
            {
                spLimiter += 1f * SpAdded;
                adept.SeptimalPower += 1f * SpAdded;
            }
        }
    }
}
