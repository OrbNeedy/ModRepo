using gvmod.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Common.Players.Septimas.Abilities
{
    internal class Astrasphere : Special
    {
        private int specialDuration = 120;
        private Vector2 basePosition = new Vector2(128);

        public Astrasphere(Player player, AdeptPlayer adept) : base(player, adept)
        {
            ApUsage = 1;
            SpecialCooldownTime = 600;
            CooldownTimer = SpecialCooldownTime;
            BeingUsed = false;
            SpecialTimer = 1;
        }

        public override string Name => "Astrasphere";

        public override int UnlockLevel { get => 1; }

        public override void Effects()
        {
            if (BeingUsed)
            {
                Dust.NewDust(Player.Center, 10, 10, DustID.BlueTorch);
            }
        }

        public override void Attack()
        {
            if (BeingUsed)
            {
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + basePosition, new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), (int)(60 * Adept.specialDamageLevelMult * Adept.specialDamageEquipMult), 8, Player.whoAmI);
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + basePosition.RotatedBy(MathHelper.ToRadians(120)), new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), (int)(60 * Adept.specialDamageLevelMult * Adept.specialDamageEquipMult), 8, Player.whoAmI);
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + basePosition.RotatedBy(MathHelper.ToRadians(-120)), new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), (int)(60 * Adept.specialDamageLevelMult * Adept.specialDamageEquipMult), 8, Player.whoAmI);
                basePosition = basePosition.RotatedBy(MathHelper.ToRadians(3.5f));
            }
        }

        public override void Update()
        {
            if (!BeingUsed)
            {
                VelocityMultiplier = new Vector2(1f, 1f);
            } else
            {
                VelocityMultiplier *= 0f;
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
            if(BeingUsed)
            {
                Adept.isUsingSpecialAbility = true;
                SpecialTimer++;
                if (SpecialTimer >= specialDuration)
                {
                    BeingUsed = false;
                    Adept.isUsingSpecialAbility = false;
                }
            }
            Player.velocity *= VelocityMultiplier;
        }
    }
}
