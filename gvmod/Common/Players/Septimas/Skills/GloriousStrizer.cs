﻿using gvmod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Common.Players.Septimas.Skills
{
    internal class GloriousStrizer : Special
    {
        private Vector2 lastPlayerPos = new Vector2(0);
        private float kudosMultiplier = 1;
        private int strizerIndex;
        private bool strizerExists = false;
        public GloriousStrizer(Player player, AdeptPlayer adept, string type) : base(player, adept, type)
        {
            ApUsage = 3;
            SpecialCooldownTime = 600;
            CooldownTimer = SpecialCooldownTime;
            BeingUsed = false;
            SpecialTimer = 1;
            lastPlayerPos = player.Center;
            SpecialDuration = 60;
        }

        public override int UnlockLevel => 60;

        public override bool IsOffensive => true;

        public override bool StayInPlace => true;

        public override bool GivesIFrames => true;

        public override string Name => "Glorious Strizer";

        public override void Attack()
        {
            if (BeingUsed && !strizerExists)
            {
                strizerIndex = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(96 * Player.direction, 0f), new Vector2(1f * Player.direction, 0f), ModContent.ProjectileType<GloriousSword>(), (int)(200 * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult * kudosMultiplier), 10, Player.whoAmI, -1, 1);
            }
        }

        public override void MoveOverride()
        {
            VelocityMultiplier = new Vector2(0f, 0.00001f);
            Player.slowFall = true;
        }

        public override void Update()
        {
            if (!BeingUsed)
            {
                kudosMultiplier = 1 + (Adept.Kudos * 0.0009f);
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
        }

        public void ProjectileUpdate()
        {
            Projectile strizer = Main.projectile[strizerIndex];
            if (strizer.active && strizer.ModProjectile is GloriousSword && strizer.owner == Player.whoAmI)
            {
                strizerExists = true;
            }
            else
            {
                strizerExists = false;
            }
        }
    }
}
