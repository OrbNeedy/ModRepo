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
        private Vector2 basePosition = new Vector2(128);
        private Vector2 lastPlayerPos = new Vector2(0);
        private int flashfieldIndex;
        private bool flashfieldExists = false;
        private int sphere1Index;
        private bool sphere1Exists = false;
        private int sphere2Index;
        private bool sphere2Exists = false;
        private int sphere3Index;
        private bool sphere3Exists = false;

        public Astrasphere(Player player, AdeptPlayer adept) : base(player, adept)
        {
            ApUsage = 1;
            SpecialCooldownTime = 600;
            CooldownTimer = SpecialCooldownTime;
            BeingUsed = false;
            SpecialTimer = 1;
            lastPlayerPos = player.Center;
            SpecialDuration = 120;
        }

        public override int UnlockLevel => 1;

        public override bool IsOffensive => true;

        public override string Name => "Astrasphere";

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
                if (!flashfieldExists) flashfieldIndex = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(0f, 0f), ModContent.ProjectileType<FlashfieldStriker>(), (int)(80 * Adept.specialDamageLevelMult * Adept.specialDamageEquipMult), 8, Player.whoAmI, -1, 1);
                if (!sphere1Exists) sphere1Index = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + basePosition, new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), (int)(80 * Adept.specialDamageLevelMult * Adept.specialDamageEquipMult), 8, Player.whoAmI, -1, 1);
                if (!sphere2Exists) sphere2Index = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + basePosition.RotatedBy(MathHelper.ToRadians(120)), new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), (int)(80 * Adept.specialDamageLevelMult * Adept.specialDamageEquipMult), 8, Player.whoAmI, -1, 1);
                if (!sphere3Exists) sphere3Index = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + basePosition.RotatedBy(MathHelper.ToRadians(-120)), new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), (int)(80 * Adept.specialDamageLevelMult * Adept.specialDamageEquipMult), 8, Player.whoAmI, -1, 1);
                basePosition = basePosition.RotatedBy(MathHelper.ToRadians(3.5f));
            }
        }

        public override void Update()
        {
            ProjectileUpdate();
            if (!BeingUsed)
            {
                lastPlayerPos = Player.Center;
                VelocityMultiplier = new Vector2(1f, 1f);
            } else
            {
                VelocityMultiplier *= 0f;
                Player.Center = lastPlayerPos;
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
                Adept.isUsingSpecialAbility = true;
                SpecialTimer++;
                if (SpecialTimer >= SpecialDuration)
                {
                    BeingUsed = false;
                    Adept.isUsingSpecialAbility = false;
                }
            }
            Player.velocity *= VelocityMultiplier;
        }

        public void ProjectileUpdate()
        {
            Projectile flashfield = Main.projectile[flashfieldIndex];
            if (flashfield.active && flashfield.ModProjectile is FlashfieldStriker)
            {
                flashfieldExists = true;
            }
            else
            {
                flashfieldExists = false;
            }

            Projectile sphere1 = Main.projectile[sphere1Index];
            if (sphere1.active && sphere1.ModProjectile is ElectricSphere)
            {
                sphere1Exists = true;
            }
            else
            {
                sphere1Exists = false;
            }

            Projectile sphere2 = Main.projectile[sphere2Index];
            if (sphere2.active && sphere2.ModProjectile is ElectricSphere)
            {
                sphere2Exists = true;
            }
            else
            {
                sphere2Exists = false;
            }

            Projectile sphere3 = Main.projectile[sphere3Index];
            if (sphere3.active && sphere3.ModProjectile is ElectricSphere)
            {
                sphere3Exists = true;
            }
            else
            {
                sphere3Exists = false;
            }
        }
    }
}
