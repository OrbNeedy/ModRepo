using gvmod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Common.Players.Septimas.Skills
{
    public class Sparkcaliburg : Special
    {
        private int baseDamage;
        private int sparkcaliburgIndex;
        private int sparkcaliburgIndex2;
        private int sparkcaliburgIndex3;
        private bool sparkcaliburgExists = false;
        private bool sparkcaliburgExists2 = false;
        private bool sparkcaliburgExists3 = false;
        public Sparkcaliburg(Player player, AdeptPlayer adept, string type) : base(player, adept, type)
        {
            ApUsage = 2;
            SpecialCooldownTime = 600;
            CooldownTimer = SpecialCooldownTime;
            BeingUsed = false;
            SpecialTimer = 1;
            SpecialDuration = 60;
            baseDamage = 150;
        }

        public override int UnlockLevel => 13;

        public override bool IsOffensive => true;

        public override bool StayInPlace => true;

        public override bool GivesIFrames => true;

        public override string Name => "Sparkcaliburg";

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

        public override void MoveOverride()
        {
            VelocityMultiplier = new Vector2(0f, 0.00001f);
            Player.slowFall = true;
        }

        public override void Update()
        {
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

            ProjectileUpdate();
        }

        public void ProjectileUpdate()
        {
            Projectile sparkcaliburg = Main.projectile[sparkcaliburgIndex];
            if (sparkcaliburg.active && sparkcaliburg.ModProjectile is ElectricSword && sparkcaliburg.owner == Player.whoAmI)
            {
                sparkcaliburgExists = true;
            }
            else
            {
                sparkcaliburgExists = false;
            }

            
            Projectile sparkcaliburg2 = Main.projectile[sparkcaliburgIndex2];
            if (sparkcaliburg2.active && sparkcaliburg2.ModProjectile is ElectricSword && sparkcaliburg2.owner == Player.whoAmI)
            {
                sparkcaliburgExists2 = true;
            }
            else
            {
                sparkcaliburgExists2 = false;
            }

            Projectile sparkcaliburg3 = Main.projectile[sparkcaliburgIndex3];
            if (sparkcaliburg3.active && sparkcaliburg3.ModProjectile is ElectricSword && sparkcaliburg3.owner == Player.whoAmI)
            {
                sparkcaliburgExists3 = true;
            }
            else
            {
                sparkcaliburgExists3 = false;
            }
        }

        private void TypeSAttack()
        {
            switch (Adept.PowerLevel)
            {
                case 1:
                    baseDamage = 150;
                    if (!sparkcaliburgExists)
                    {
                        sparkcaliburgIndex = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(96 * Player.direction, 0f), new Vector2(1f * Player.direction, 0f), ModContent.ProjectileType<ElectricSword>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 10, Player.whoAmI, -1, 1);
                    }
                    break;
                case 2:
                    baseDamage = 200;
                    if (!sparkcaliburgExists)
                    {
                        sparkcaliburgIndex = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(96 * Player.direction, 0f), new Vector2(1f * Player.direction, 0f), ModContent.ProjectileType<ElectricSword>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 10, Player.whoAmI, -1, 1);
                    }
                    if (!sparkcaliburgExists2)
                    {
                        sparkcaliburgIndex2 = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(0f, -96f), new Vector2(0f, -1f), ModContent.ProjectileType<ElectricSword>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 10, Player.whoAmI, -1, 1);
                    }
                    if (!sparkcaliburgExists3)
                    {
                        sparkcaliburgIndex3 = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(96 * -Player.direction, 0f), new Vector2(1f * -Player.direction, 0f), ModContent.ProjectileType<ElectricSword>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 10, Player.whoAmI, -1, 1);
                    }
                    break;
                case 3:
                    baseDamage = 425;
                    SpecialDuration = 90;
                    if (SpecialTimer == 1)
                    {
                        int direction;
                        if (Player.Center.DirectionTo(Main.MouseWorld).X < 0)
                        {
                            direction = -1;
                        } else
                        {
                            direction = 1;
                        }
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, 
                            Vector2.Zero, ModContent.ProjectileType<ElectricSword>(), 
                            (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 10, 
                            Player.whoAmI, 0, 3, direction);
                    }
                    break;
            }
        }

        private void TypeTAttack()
        {
            switch (Adept.PowerLevel)
            {
                case 1:
                    baseDamage = 150; 
                    if (!sparkcaliburgExists)
                    {
                        sparkcaliburgIndex = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(96 * Player.direction, 0f), new Vector2(1f * Player.direction, 0f), ModContent.ProjectileType<ElectricSword>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 10, Player.whoAmI, -1, 1);
                    }
                    break;
                case 2:
                    baseDamage = 250;
                    SpecialDuration = 180;
                    if (!sparkcaliburgExists)
                    {
                        sparkcaliburgIndex = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(0f, 0.01f), ModContent.ProjectileType<ElectricSword>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 10, Player.whoAmI, 0, 2);
                    }
                    break;
                case 3:
                    baseDamage = 250;
                    SpecialDuration = 300;
                    SpecialCooldownTime = 900;
                    if (SpecialTimer == 90)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, 
                                new Vector2(20f, 0f).RotatedBy(1.570796*i), ModContent.ProjectileType<ElectricSword>(),
                                (int)(baseDamage * (Adept.SpecialDamageLevelMult + Adept.SpecialDamageEquipMult)), 10,
                                Player.whoAmI, 1, 4, i);
                        }
                    }
                    break;
            }
        }
    }
}
