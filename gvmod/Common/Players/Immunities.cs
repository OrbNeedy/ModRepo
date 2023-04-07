using gvmod.Common.Players.Septimas;
using gvmod.Content.Buffs;
using gvmod.Content.Projectiles;
using IL.Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Common.Players
{
    public class Immunities : ModPlayer
    {
        public override void PreUpdateBuffs()
        {
            AdeptPlayer adept = Player.GetModPlayer<AdeptPlayer>();
            switch (adept.Septima)
            {
                case AzureStriker or AzureThunderclap:
                    if (Player.HasBuff(BuffID.Electrified))
                    {
                        int time = Player.buffTime[Player.FindBuffIndex(BuffID.Electrified)];
                        Player.AddBuff(ModContent.BuffType<GoodElectrified>(), time);
                        Player.ClearBuff(BuffID.Electrified);
                    }
                    break;
            }
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            AdeptPlayer adept = Player.GetModPlayer<AdeptPlayer>();
            foreach (int projectileType in adept.Septima.RegularProjectileVulnerabilites)
            {
                if (damageSource.SourceProjectileType == projectileType)
                {
                    damage /= 2;
                    break;
                }
            }
            switch (adept.Septima)
            {
                case AzureStriker or AzureThunderclap:
                    if (damageSource.SourceProjectileType == ModContent.ProjectileType<ElectricWhipProjectile>())
                    {
                        adept.SeptimalPower += damage;
                        damage = 0;
                    }
                    break;
            }
            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, ref cooldownCounter);
        }
    }
}
