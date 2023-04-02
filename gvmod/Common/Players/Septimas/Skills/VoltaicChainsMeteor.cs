using gvmod.Common.Configs.CustomDataTypes;
using gvmod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Common.Players.Septimas.Skills
{
    public class VoltaicChainsMeteor : Special
    {
        public VoltaicChainsMeteor(Player player, AdeptPlayer adept, string type) : base(player, adept, type)
        {
            ApUsage = 2;
            SpecialCooldownTime = 600;
            CooldownTimer = SpecialCooldownTime;
            BeingUsed = false;
            SpecialTimer = 1;
            SpecialDuration = 1500;
        }

        public override int UnlockLevel => 30;

        public override bool IsOffensive => true;

        public override bool GivesIFrames => false;

        public override string Name => "Voltaic Chains Meteor";

        public override void Attack()
        {
            if (BeingUsed && SpecialTimer == 1)
            {
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<ChainMeteor>(), 0, 15, Player.whoAmI, 0); Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<ChainMeteor>(), 0, 15, Player.whoAmI, 0);
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(600, 400), new Vector2(0, -10), ModContent.ProjectileType<ChainTip>(), 0, 15, Player.whoAmI, -1, 2);
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(-600, -400), new Vector2(0, 10), ModContent.ProjectileType<ChainTip>(), 0, 15, Player.whoAmI, -1, 2);
            }
        }

        public override void Effects()
        {
        }

        public override void Update()
        {
            if (!BeingUsed)
            {
                VelocityMultiplier = new Vector2(1f, 1f);
            }
            else
            {
                Player.slowFall = true;
            }
            if (CooldownTimer < SpecialCooldownTime)
            {
                CooldownTimer++;
                InCooldown = true;
            }
            if (CooldownTimer >= SpecialCooldownTime)
            {
                InCooldown = false;
            }
            if (SpecialTimer == 0 && !InCooldown)
            {
                CooldownTimer = 0;
                BeingUsed = true;
            }
            if (BeingUsed)
            {
                Adept.IsUsingSpecialAbility = true;
                SpecialTimer++;
                if (SpecialTimer >= SpecialDuration)
                {
                    BeingUsed = false;
                    Adept.IsUsingSpecialAbility = false;
                }
            }

            Player.velocity *= VelocityMultiplier;
        }
    }
}
