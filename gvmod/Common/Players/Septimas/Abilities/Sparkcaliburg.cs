using gvmod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Common.Players.Septimas.Abilities
{
    public class Sparkcaliburg : Special
    {
        private int specialDuration = 60;
        private Vector2 lastPlayerPos = new Vector2(0);
        public Sparkcaliburg(Player player, AdeptPlayer adept) : base(player, adept)
        {
            ApUsage = 2;
            SpecialCooldownTime = 600;
            CooldownTimer = SpecialCooldownTime;
            BeingUsed = false;
            SpecialTimer = 1;
            lastPlayerPos = player.Center;
        }

        public override int UnlockLevel => 13;

        public override string Name => "Sparkcaliburg";

        public override void Attack()
        {
            if (BeingUsed)
            {
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(96 * Player.direction, 0f), new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSword>(), (int)(150 * Adept.specialDamageLevelMult * Adept.specialDamageEquipMult), 10, Player.whoAmI);
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
