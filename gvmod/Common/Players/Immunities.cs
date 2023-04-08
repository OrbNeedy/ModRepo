using gvmod.Common.Players.Septimas;
using gvmod.Content.Buffs;
using gvmod.Content.Projectiles;
using Terraria;
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

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            AdeptPlayer adept = Player.GetModPlayer<AdeptPlayer>();
            foreach (int projectileType in adept.Septima.RegularProjectileVulnerabilites)
            {
                if (modifiers.DamageSource.SourceProjectileType == projectileType)
                {
                    modifiers.FinalDamage /= 2;
                    break;
                }
            }
            switch (adept.Septima)
            {
                case AzureStriker or AzureThunderclap:
                    if (modifiers.DamageSource.SourceProjectileType == ModContent.ProjectileType<ElectricWhipProjectile>())
                    {
                        adept.SeptimalPower += modifiers.FinalDamage.Flat;
                        modifiers.FinalDamage *= 0;
                    }
                    break;
            }
        }
    }
}
