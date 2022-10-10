using Terraria;
using Terraria.ModLoader;

namespace gvmod.Common.Players
{
    public class StrikerElectrifiedPlayer : ModPlayer
    {
        public bool strikerElectricityDebuff;

        public override void ResetEffects()
        {
            strikerElectricityDebuff = false;
        }

        public override void UpdateBadLifeRegen()
        {
            if (strikerElectricityDebuff)
            {
                Player.lifeRegen -= 8;
            }
        }
    }
}
