using gvmod.Common.Configs.CustomDataTypes;
using gvmod.Content.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace gvmod.Common.Players.Septimas.Abilities
{
    public class VoltaicChains : Special
    {
        private int specialDuration = 300;
        public VoltaicChains(Player player, AdeptPlayer adept) : base(player, adept)
        {
            ApUsage = 3;
            SpecialCooldownTime = 600;
            CooldownTimer = SpecialCooldownTime;
            BeingUsed = false;
            SpecialTimer = 1;
        }

        public override int UnlockLevel => 40;

        public override string Name => "Voltaic Chains";

        public override void Attack()
        {
            if (BeingUsed)
            {
                if (SpecialTimer%20 == 0)
                {
                    ChainPositions positions = new ChainPositions(Player);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), positions.startingPosition, positions.GetVelocity(), ModContent.ProjectileType<ChainTip>(), (int)(50 * Adept.specialDamageLevelMult * Adept.specialDamageEquipMult), 0, Player.whoAmI);
                }
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
