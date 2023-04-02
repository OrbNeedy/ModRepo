using gvmod.Common.Configs.CustomDataTypes;
using gvmod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Common.Players.Septimas.Skills
{
    public class VoltaicChainsMeteor : Special
    {
        private int meteorIndex;
        public VoltaicChainsMeteor(Player player, AdeptPlayer adept, string type) : base(player, adept, type)
        {
            ApUsage = 2;
            SpecialCooldownTime = 1200;
            CooldownTimer = SpecialCooldownTime;
            BeingUsed = false;
            SpecialTimer = 1;
            SpecialDuration = 300;
        }

        public override int UnlockLevel => 30;

        public override bool IsOffensive => true;

        public override bool GivesIFrames => false;

        public override string Name => "Voltaic Chains Meteor";

        public override void Attack()
        {
            if (BeingUsed && SpecialTimer == 1)
            {
                meteorIndex = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<ChainMeteor>(), 0, 15, Player.whoAmI, 0); Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<ChainMeteor>(), 0, 15, Player.whoAmI, 0);
            }
        }

        public override void Effects()
        {
        }

        public override void Update()
        {
            Projectile meteor = Main.projectile[meteorIndex];
            if (!BeingUsed)
            {
                VelocityMultiplier = new Vector2(1f, 1f);
            }
            else
            {
                if (!meteor.active || meteor.ModProjectile is not ChainMeteor || meteor.owner != Player.whoAmI)
                {
                    SpecialTimer = SpecialDuration - 45;
                }
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
