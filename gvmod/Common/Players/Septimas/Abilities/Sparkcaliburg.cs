using gvmod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Common.Players.Septimas.Abilities
{
    public class Sparkcaliburg : Special
    {
        private Vector2 lastPlayerPos = new Vector2(0);
        private int sparkcaliburgIndex;
        private bool sparkcaliburgExists = false;
        public Sparkcaliburg(Player player, AdeptPlayer adept) : base(player, adept)
        {
            ApUsage = 2;
            SpecialCooldownTime = 600;
            CooldownTimer = SpecialCooldownTime;
            BeingUsed = false;
            SpecialTimer = 1;
            lastPlayerPos = player.Center;
            SpecialDuration = 60;
        }

        public override int UnlockLevel => 13;

        public override bool IsOffensive => true;

        public override bool GivesIFrames => true;

        public override string Name => "Sparkcaliburg";

        public override void Attack()
        {
            if (BeingUsed && !sparkcaliburgExists)
            {
                sparkcaliburgIndex = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(96 * Player.direction, 0f), new Vector2(1f * Player.direction, 0f), ModContent.ProjectileType<ElectricSword>(), (int)(150 * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 10, Player.whoAmI, -1, 1);
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
                lastPlayerPos = Player.Center;
            }
            else
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
                }
            }

            ProjectileUpdate();
            
            Player.velocity *= VelocityMultiplier;
        }

        public void ProjectileUpdate()
        {
            Projectile sparkcaliburg = Main.projectile[sparkcaliburgIndex];
            if (sparkcaliburg.active && sparkcaliburg.ModProjectile is ElectricSword)
            {
                sparkcaliburgExists = true;
            }
            else
            {
                sparkcaliburgExists = false;
            }
        }
    }
}
