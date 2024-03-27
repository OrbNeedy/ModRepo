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
            ApUsage = 3;
            SpecialCooldownTime = 1800;
            CooldownTimer = SpecialCooldownTime;
            BeingUsed = false;
            SpecialTimer = 1;
            SpecialDuration = 600;
        }

        public override int UnlockLevel => 30;

        public override bool IsOffensive => true;

        public override bool StayInPlace => true;

        public override bool GivesIFrames => false;

        public override string Name => "Voltaic Chains Meteor";

        public override void StatChangeEffects()
        {
            Player.endurance += 0.4f;
            Player.statDefense += 40;
        }

        public override void Attack()
        {
            if (BeingUsed && SpecialTimer == 1)
            {
                ChainPositions tempPositions = new(Player);
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(950, 440), 
                    tempPositions.GetVelocity(new Vector2(0, 880), new Vector2(0, 0)), 
                    ModContent.ProjectileType<ChainTip>(), 0, 15, Player.whoAmI, 1100, 2, 300);
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(-950, -440),
                    tempPositions.GetVelocity(new Vector2(0, 0), new Vector2(0, 880)), 
                    ModContent.ProjectileType<ChainTip>(), 0, 15, Player.whoAmI, 1100, 2, 300);

                meteorIndex = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, 
                    ModContent.ProjectileType<ChainMeteor>(), 0, 15, Player.whoAmI, 0); 
            }
        }

        public override void MoveOverride()
        {
            VelocityMultiplier = new Vector2(0f, 0.00001f);
            Player.slowFall = true;
        }

        public override void Update()
        {
            Projectile meteor = Main.projectile[meteorIndex];
            if (BeingUsed)
            {
                if (!meteor.active || meteor.ModProjectile is not ChainMeteor || meteor.owner != Player.whoAmI)
                {
                    SpecialTimer = SpecialDuration - 45;
                }
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
        }
    }
}
