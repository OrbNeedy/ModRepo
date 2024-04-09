using Terraria.ModLoader;

namespace gvmod.Common.Players
{
    public class GoodDebuffPlayer : ModPlayer
    {
        public bool rechargingElectricity;
        public bool rechargingBleeding;

        public override void ResetEffects()
        {
            rechargingElectricity = false;
            rechargingBleeding = false;
        }

        public override void PreUpdate()
        {
            if (rechargingElectricity)
            {
                Player.GetModPlayer<AdeptPlayer>().SPRegenModifier += 0.2f;
            }
            if (rechargingElectricity)
            {
                Player.GetModPlayer<AdeptPlayer>().SPRegenModifier += 0.1f;
            }
            base.PreUpdate();
        }
    }
}
