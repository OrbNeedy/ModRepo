using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Projectiles
{
    internal class OrochiDrone : ModProjectile
    {
        private Vector2 baseVelocity;
        private Vector2 basePosition;
        private int movementStep = 0;
        private int shootTimer;
        private bool firstRotationNegative;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orochi Drone");
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.scale = 1f;
            Projectile.light = 0.20f;

            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.damage = 0;
            Projectile.knockBack = 0;

            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.ownerHitCheck = false;

            movementStep = 0;
        }

        public override void OnSpawn(IEntitySource source)
        {
            baseVelocity = Main.rand.NextBool(1, 2) ? new Vector2(10, 0) : new Vector2(-10, 0);
            basePosition = Main.player[Projectile.owner].Center + new Vector2(48 * Main.player[Projectile.owner].direction, -80);
            shootTimer = 0;
        }

        public override void AI()
        {
            if (movementStep == 0)
            {
                if (Projectile.Center.Distance(basePosition) > 10)
                {
                    Projectile.velocity = Projectile.Center.DirectionTo(basePosition) * 8;
                } else {
                    Projectile.velocity = Vector2.Zero;
                    movementStep = 1;
                    firstRotationNegative = Main.rand.NextBool(1, 2);
                }
            }
            
            if (movementStep == 1)
            {
                if (shootTimer >= 190) movementStep = 2;
                shootTimer++;
                if (shootTimer <= 80)
                {
                    if (shootTimer >= 20 && shootTimer % 10 == 0)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, baseVelocity, ModContent.ProjectileType<HairDartProjectile>(), 2, 0, Projectile.owner);
                        SoundEngine.PlaySound(SoundID.Camera);
                        if (shootTimer < 80)
                        {
                            if (!firstRotationNegative)
                            {
                                baseVelocity = baseVelocity.RotatedBy(MathHelper.ToRadians(30));
                            }
                            else
                            {
                                baseVelocity = baseVelocity.RotatedBy(MathHelper.ToRadians(-30));
                            }
                        }
                    }
                }
                if (shootTimer >= 110)
                {
                    if (shootTimer % 10 == 0 && shootTimer <= 170)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, -baseVelocity, ModContent.ProjectileType<HairDartProjectile>(), 2, 0, Projectile.owner);
                        SoundEngine.PlaySound(SoundID.Camera);
                        if (!firstRotationNegative)
                        {
                            baseVelocity = baseVelocity.RotatedBy(MathHelper.ToRadians(-30));
                        } else
                        {
                            baseVelocity = baseVelocity.RotatedBy(MathHelper.ToRadians(30));
                        }
                    }
                }
            }

            if (movementStep == 2)
            {
                Projectile.velocity = Projectile.Center.DirectionTo(Main.player[Projectile.owner].Center) * 8;
                if (Projectile.Center.Distance(Main.player[Projectile.owner].Center) <= 16) Projectile.Kill();
            }
        }
    }
}
