using gvmod.Common.Players;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace gvmod.Content.Projectiles
{
    internal class ElectricSphere : ModProjectile
    {
        private int timer;
        private Vector2 centerOfRotation;
        private Vector2 target;
        private Vector2 positionOffset;

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(42);
            Projectile.scale = 1f;
            Projectile.light = 1f;

            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.damage = 60;
            Projectile.knockBack = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
            Projectile.penetrate = -1;

            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 2;
            Projectile.ownerHitCheck = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];
            target = centerOfRotation = player.Center;
            positionOffset = new Vector2(0, 172);
            timer = 0;

            // ai0 decides how it will act
            // 0 (GV1 Astrasphere): Regular type interaction, rotate around the center of rotation and follow it
            // 1 (GV3 Flashfield): Variation of 0, which stays while the player is using the primary ability
            // 2 (AM3 Flashfield): Same as 1, but has a separation of 0.8 times regular
            // 3 (GV2 Astrasphere): Same as 0, but after some time, separate from the center
            // 4 (AM2 Astrasphere): Same as 0, but start with less separation from the center and separate faster
            // 5 (GVEX Astrasphere): Same as 3, but has a longer time before it separates
            switch(Projectile.ai[0])
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    positionOffset *= 0.8f;
                    break;
                case 3:
                    break;
                case 4:
                    positionOffset = new Vector2(0, 22);
                    break;
                case 5:
                    break;
            }
            // ai1 decides it's initial rotation
            switch (Projectile.ai[1])
            {
                case 1:
                    positionOffset = positionOffset.RotatedBy(MathHelper.ToRadians(120));
                    break;
                case 2:
                    positionOffset = positionOffset.RotatedBy(MathHelper.ToRadians(-120));
                    break;
                default:
                    break;
            }
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();

            float rotation = 0.06108652f;

            positionOffset = positionOffset.RotatedBy(rotation);
            Projectile.Center = centerOfRotation + positionOffset;
            switch (Projectile.ai[0])
            {
                case 0:
                    if (adept.IsUsingSpecialAbility) Projectile.timeLeft = 2;
                    else Projectile.timeLeft = 0;
                    break;
                case 1:
                    if (adept.IsUsingPrimaryAbility && adept.CanUsePrimary) Projectile.timeLeft = 2;
                    else Projectile.timeLeft = 0;
                    break;
                case 2:
                    if (adept.IsUsingPrimaryAbility && adept.CanUsePrimary) Projectile.timeLeft = 2;
                    else Projectile.timeLeft = 0;
                    break;
                case 3:
                    if (adept.IsUsingSpecialAbility) Projectile.timeLeft = 2;
                    else Projectile.timeLeft = 0;
                    SimpleMovementAI(90);
                    break;
                case 4:
                    if (adept.IsUsingSpecialAbility) Projectile.timeLeft = 2;
                    else Projectile.timeLeft = 0;
                    SimpleMovementAI(150, 90);
                    break;
                case 5:
                    if (adept.IsUsingSpecialAbility) Projectile.timeLeft = 2;
                    else Projectile.timeLeft = 0;
                    SimpleMovementAI(150);
                    break;
            }

            // If ai2 is not negative, it is the index of the center projectile
            if (Projectile.ai[2] >= 0)
            {
                Projectile mainProjectile = Main.projectile[(int)Projectile.ai[2]];
                if (mainProjectile.active && mainProjectile.ModProjectile is FlashfieldStriker && mainProjectile.owner == Projectile.owner)
                {
                    centerOfRotation = mainProjectile.Center;
                } 
            }
        }

        private void SimpleMovementAI(int separationTime, int independent=-1)
        {
            if (timer == independent)
            {
                target = Main.MouseWorld;
            }

            if (timer >= separationTime) positionOffset *= 1.025f;
            
            if (target.DistanceSQ(centerOfRotation) >= 40 && timer < separationTime) centerOfRotation += centerOfRotation.DirectionTo(target) * 12;
            
            timer++;
        }
    }
}
