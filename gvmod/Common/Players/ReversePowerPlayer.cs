using Terraria.ModLoader;

namespace gvmod.Common.Players
{
    public class ReversePowerPlayer : ModPlayer
    {
        public bool PowerShortage { get; set; }

        public override void Initialize()
        {
            PowerShortage = false;
        }

        public override void ResetEffects()
        {
            PowerShortage = false;
        }

        public override void NaturalLifeRegen(ref float regen)
        {
            if (PowerShortage) regen *= -1;
        }

        public override void UpdateLifeRegen()
        {
            if (PowerShortage) Player.lifeRegen *= -1;
        }
    }
}
