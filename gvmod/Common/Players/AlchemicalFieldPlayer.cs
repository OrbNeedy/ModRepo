using Terraria.ModLoader;

namespace gvmod.Common.Players
{
    public class AlchemicalFieldPlayer : ModPlayer
    {
        public bool AlchemicalField { get; set; }

        public override void Initialize()
        {
            AlchemicalField = false;
        }

        public override void ResetEffects()
        {
            AlchemicalField = false;
        }
    }
}
