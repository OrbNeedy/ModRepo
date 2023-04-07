using gvmod.Common.Players.Septimas;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Common.Players
{
    public class Vulnerabilities : ModPlayer
    {
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            AdeptPlayer adept = Player.GetModPlayer<AdeptPlayer>();
            foreach (int projectileType in adept.Septima.RegularProjectileVulnerabilites)
            {
                if (damageSource.SourceProjectileType == projectileType)
                {
                    damage *= 2;
                    break;
                }
            }
            switch (adept.Septima)
            {
                case AzureStriker or AzureThunderclap:
                    if (damageSource.SourceProjectileType == ProjectileID.WaterBolt ||
                        damageSource.SourceProjectileType == ProjectileID.WaterStream ||
                        damageSource.SourceProjectileType == ProjectileID.WaterGun)
                    {
                        adept.SeptimalPower = 0;
                    }
                    break;
            }
            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, ref cooldownCounter);
        }
    }
}
