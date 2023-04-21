using Terraria;
using Terraria.ModLoader;

namespace gvmod.Common.Players
{
    public class SeptimalShieldPlayer : ModPlayer
    {
        public bool SeptimalShield { get; set; }

        public override void Initialize()
        {
            SeptimalShield = false;
        }

        public override void ResetEffects()
        {
            SeptimalShield = false;
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
