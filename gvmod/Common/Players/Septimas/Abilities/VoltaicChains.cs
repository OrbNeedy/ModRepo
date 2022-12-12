using gvmod.Common.Configs.CustomDataTypes;
using gvmod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace gvmod.Common.Players.Septimas.Abilities
{
    public class VoltaicChains : Special
    {
        private int firstChain;
        private bool isNotFirstChain = false;
        private Vector2 lastPlayerPos = new Vector2(0);
        public VoltaicChains(Player player, AdeptPlayer adept) : base(player, adept)
        {
            ApUsage = 3;
            SpecialCooldownTime = 600;
            CooldownTimer = SpecialCooldownTime;
            BeingUsed = false;
            SpecialTimer = 1;
            SpecialDuration = 300;
        }

        public override int UnlockLevel => 40;

        public override bool IsOffensive => true;

        public override string Name => "Voltaic Chains";

        public override void Attack()
        {
            if (BeingUsed)
            {
                if (SpecialTimer%20 == 0 && SpecialTimer <= 210)
                {
                    ChainPositions positions = new ChainPositions(Player);
                    if (isNotFirstChain)
                    {
                        // Pass the IEntitySource from the first chain as a parameter to ai0 or ai1
                        int firstChainSource = Main.projectile[firstChain].whoAmI;
                        Projectile.NewProjectile(Player.GetSource_FromThis(), positions.startingPosition, positions.GetVelocity(), ModContent.ProjectileType<ChainTip>(), (int)(50 * Adept.specialDamageLevelMult * Adept.specialDamageEquipMult), 0, Player.whoAmI, firstChainSource, 0);
                    } else
                    {
                        // Save the IEntitySource from the first chain here
                        firstChain = Projectile.NewProjectile(Player.GetSource_FromThis(), positions.startingPosition, positions.GetVelocity(), ModContent.ProjectileType<ChainTip>(), (int)(50 * Adept.specialDamageLevelMult * Adept.specialDamageEquipMult), 0, Player.whoAmI, 0, 1);
                        isNotFirstChain = true;
                    }
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
                isNotFirstChain = false;
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
                if (SpecialTimer >= SpecialDuration)
                {
                    BeingUsed = false;
                    Adept.isUsingSpecialAbility = false;
                }
            }
            Player.velocity *= VelocityMultiplier;
        }
    }
}
