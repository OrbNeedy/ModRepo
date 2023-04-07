using gvmod.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Common.Players.Septimas.Skills
{
    internal class Astrasphere : Special
    {
        private Vector2 basePosition;
        private Vector2 lastPlayerPos;
        private Vector2 centerPosition;
        private Vector2 targetPosition;
        private int phase;
        private int baseDamage;
        private int flashfieldIndex;
        private bool flashfieldExists = false;
        private int sphere1Index;
        private bool sphere1Exists = false;
        private int sphere2Index;
        private bool sphere2Exists = false;
        private int sphere3Index;
        private bool sphere3Exists = false;

        public Astrasphere(Player player, AdeptPlayer adept, string type) : base(player, adept, type)
        {
            ApUsage = 1;
            SpecialCooldownTime = 600;
            CooldownTimer = SpecialCooldownTime;
            BeingUsed = false;
            SpecialTimer = 1;
            basePosition = new Vector2(128);
            lastPlayerPos = player.Center;
            SpecialDuration = 120;
            centerPosition = Player.Center;
            phase = 1;
            baseDamage = 80;
            targetPosition = Vector2.Zero;
        }

        public override int UnlockLevel => 1;

        public override bool IsOffensive => true;

        public override bool GivesIFrames => true;

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
                if (Type == "S")
                {
                    TypeSAttack();
                }
                if (Type == "T")
                {
                    TypeTAttack();
                }
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
                Adept.IsUsingSpecialAbility = true;
                SpecialTimer++;
                if (SpecialTimer >= SpecialDuration)
                {
                    BeingUsed = false;
                    Adept.IsUsingSpecialAbility = false;
                    phase = 1;
                    centerPosition = Player.Center;
                    basePosition = new Vector2(128);
                }
            }
            Player.velocity *= VelocityMultiplier;
        }

        public void ProjectileUpdate()
        {
            Projectile flashfield = Main.projectile[flashfieldIndex];
            if (flashfield.active && flashfield.ModProjectile is FlashfieldStriker && flashfield.owner == Player.whoAmI)
            {
                flashfieldExists = true;
            }
            else
            {
                flashfieldExists = false;
            }

            Projectile sphere1 = Main.projectile[sphere1Index];
            if (sphere1.active && sphere1.ModProjectile is ElectricSphere && sphere1.owner == Player.whoAmI)
            {
                sphere1Exists = true;
            }
            else
            {
                sphere1Exists = false;
            }

            Projectile sphere2 = Main.projectile[sphere2Index];
            if (sphere2.active && sphere2.ModProjectile is ElectricSphere && sphere2.owner == Player.whoAmI)
            {
                sphere2Exists = true;
            }
            else
            {
                sphere2Exists = false;
            }

            Projectile sphere3 = Main.projectile[sphere3Index];
            if (sphere3.active && sphere3.ModProjectile is ElectricSphere && sphere3.owner == Player.whoAmI)
            {
                sphere3Exists = true;
            }
            else
            {
                sphere3Exists = false;
            }
        }

        private void TypeSAttack()
        {
            switch(Adept.PowerLevel)
            {
                case 1:
                    Main.NewText("Type S Astrasphere (Level 1)");
                    baseDamage = 80;
                    if (!flashfieldExists) flashfieldIndex = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(0f, 0f), ModContent.ProjectileType<FlashfieldStriker>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 8, Player.whoAmI, -1, 1);
                    if (!sphere1Exists) sphere1Index = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + basePosition, new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 8, Player.whoAmI, -1, 1);
                    if (!sphere2Exists) sphere2Index = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + basePosition.RotatedBy(MathHelper.ToRadians(120)), new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 8, Player.whoAmI, -1, 1);
                    if (!sphere3Exists) sphere3Index = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + basePosition.RotatedBy(MathHelper.ToRadians(-120)), new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 8, Player.whoAmI, -1, 1);
                    break;
                case 2:
                    Main.NewText("Type S Astrasphere (Level 2)");
                    baseDamage = 100;
                    if (phase <= 1)
                    {
                        targetPosition = Main.MouseWorld;
                    }
                    if (phase == 2 && centerPosition.DistanceSQ(targetPosition) <= 10)
                    {
                        centerPosition += centerPosition.DirectionTo(targetPosition) * 10;
                    } else
                    {
                        centerPosition = Player.Center;
                    }
                    if (phase == 3)
                    {
                        basePosition *= 1.15f;
                    }
                    if (phase == 1)
                    {
                        if (!flashfieldExists)
                        {
                            flashfieldIndex = Projectile.NewProjectile(Player.GetSource_FromThis(), centerPosition, new Vector2(0f, 0f), ModContent.ProjectileType<FlashfieldStriker>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 8, Player.whoAmI, 0, 2);
                        }
                    }
                    if (phase == 1 && SpecialTimer <= 1)
                    {
                        sphere1Index = Projectile.NewProjectile(Player.GetSource_FromThis(), centerPosition + basePosition, new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 8, Player.whoAmI, 0, 2);
                        sphere2Index = Projectile.NewProjectile(Player.GetSource_FromThis(), centerPosition + basePosition.RotatedBy(MathHelper.ToRadians(120)), new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 8, Player.whoAmI, 0, 2);
                        sphere3Index = Projectile.NewProjectile(Player.GetSource_FromThis(), centerPosition + basePosition.RotatedBy(MathHelper.ToRadians(-120)), new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 8, Player.whoAmI, 0, 2);
                    }
                    if (phase < 3 && SpecialTimer == (SpecialDuration - 1))
                    {
                        phase++;
                        if (phase == 1)
                        {
                            SpecialTimer = SpecialDuration / 2;
                        } else
                        {
                            SpecialTimer = 0;
                        }
                    }
                    break;
            }
            basePosition = basePosition.RotatedBy(MathHelper.ToRadians(3.5f));
        }

        private void TypeTAttack()
        {
            switch(Adept.PowerLevel)
            {
                case 1:
                    Main.NewText("Type T Astrasphere (Level 1)");
                    baseDamage = 80;
                    if (!flashfieldExists) flashfieldIndex = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(0f, 0f), ModContent.ProjectileType<FlashfieldStriker>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 8, Player.whoAmI, -1, 1);
                    if (!sphere1Exists) sphere1Index = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + basePosition, new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 8, Player.whoAmI, -1, 1);
                    if (!sphere2Exists) sphere2Index = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + basePosition.RotatedBy(MathHelper.ToRadians(120)), new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 8, Player.whoAmI, -1, 1);
                    if (!sphere3Exists) sphere3Index = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + basePosition.RotatedBy(MathHelper.ToRadians(-120)), new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 8, Player.whoAmI, -1, 1);
                    break;
                case 2:
                    Main.NewText("Type T Astrasphere (Level 2)");
                    baseDamage = 100;
                    if (phase <= 1)
                    {
                        targetPosition = Main.MouseWorld;
                        if (SpecialTimer <= 1)
                        {
                            basePosition *= 0.1f;
                        }
                    }
                    if (phase == 2 && centerPosition.Distance(targetPosition) <= 10)
                    {
                        centerPosition += centerPosition.DirectionTo(targetPosition) * 10;
                    }
                    else
                    {
                        centerPosition = Player.Center;
                    }
                    if (phase == 3)
                    {
                        basePosition *= 1.1f;
                    }
                    if (phase == 1 && SpecialTimer <= 1)
                    {
                        sphere1Index = Projectile.NewProjectile(Player.GetSource_FromThis(), centerPosition + basePosition, new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 8, Player.whoAmI, -1, 2);
                        sphere2Index = Projectile.NewProjectile(Player.GetSource_FromThis(), centerPosition + basePosition.RotatedBy(MathHelper.ToRadians(120)), new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 8, Player.whoAmI, -1, 2);
                        sphere3Index = Projectile.NewProjectile(Player.GetSource_FromThis(), centerPosition + basePosition.RotatedBy(MathHelper.ToRadians(-120)), new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 8, Player.whoAmI, -1, 2);
                    }
                    if (phase < 3 && SpecialTimer == (SpecialDuration - 1))
                    {
                        phase++;
                        if (phase == 1)
                        {
                            SpecialTimer = SpecialDuration / 2;
                        }
                        else
                        {
                            SpecialTimer = 0;
                        }
                    }
                    break;
            }
            basePosition = basePosition.RotatedBy(MathHelper.ToRadians(3.5f));
        }
    }
}
