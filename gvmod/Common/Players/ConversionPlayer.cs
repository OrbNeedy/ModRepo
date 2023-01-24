using gvmod.Content;
using gvmod.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Common.Players
{
    public class ConversionPlayer : ModPlayer
    {
        public bool Arcanum;
        public bool Forbidden;
        public bool Special;
        public bool Secret;

        public override void Initialize()
        {
            Arcanum = false;
            Forbidden = false;
            Special = false;
            Secret = false;
        }

        public override void ResetEffects()
        {
            Arcanum = false;
            Forbidden = false;
            Special = false;
            Secret = false;
        }
    }
}
