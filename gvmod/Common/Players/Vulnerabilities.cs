using gvmod.Common.Players.Septimas;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Common.Players
{
    public class Vulnerabilities : ModPlayer
    {
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            AdeptPlayer adept = Player.GetModPlayer<AdeptPlayer>();
            foreach (int projectileType in adept.Septima.RegularProjectileVulnerabilites)
            {
                if (modifiers.DamageSource.SourceProjectileType == projectileType)
                {
                    modifiers.FinalDamage *= 2;
                    break;
                }
            }
            switch (adept.Septima)
            {
                case AzureStriker or AzureThunderclap:
                    if (modifiers.DamageSource.SourceProjectileType == ProjectileID.WaterBolt ||
                        modifiers.DamageSource.SourceProjectileType == ProjectileID.WaterStream ||
                        modifiers.DamageSource.SourceProjectileType == ProjectileID.WaterGun)
                    {
                        adept.SeptimalPower = 0;
                    }
                    break;
            }
        }
    }
}
