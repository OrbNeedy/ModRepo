using gvmod.Common.Players;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace gvmod.Content.Projectiles
{
    public class ElectricSword : ModProjectile
    {
        private int phase;
        private int counter;
        private Vector2 rotationCenter;
        private Vector2 offset;
        private Vector2 target;

        public override void SetDefaults()
        {
            Projectile.light = 1f;
            Projectile.damage = 150;
            Projectile.knockBack = 12;
            Projectile.Size = new Vector2(180, 90);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.timeLeft = 3;
            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.ownerHitCheck = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            phase = 1;
            counter = 0;
            switch (Projectile.ai[1])
            {
                case 3:
                    Projectile.timeLeft = 100;
                    rotationCenter = Projectile.Center;
                    offset = rotationCenter.DirectionTo(Main.MouseWorld) * 64;
                    offset = offset.RotatedBy(-1.727876 * Projectile.ai[2]);
                    break;
                case 2:
                    Projectile.timeLeft = 600;
                    break;
                case 4:
                    Projectile.timeLeft = 540;
                    rotationCenter = Projectile.Center;
                    offset = new Vector2(1, 0).RotatedBy(1.570796 * Projectile.ai[2]) * 96;
                    break;
            }

            switch (Projectile.ai[0])
            {
                case -1:
                    Projectile.penetrate = 2;
                    break;
                default:
                    Projectile.penetrate = -1;
                    break;
            }
            base.OnSpawn(source);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();

            return base.PreDraw(ref lightColor);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            switch (Projectile.ai[0])
            {
                case -1:
                    if (adept.IsUsingSpecialAbility)
                    {
                        Projectile.penetrate = 2;
                    }
                    break;
                default:
                    Projectile.penetrate = -1;
                    break;
            }

            switch (Projectile.ai[1])
            {
                case 1:
                    if (adept.IsUsingSpecialAbility)
                    {
                        Projectile.timeLeft = 2;
                        Projectile.velocity *= 0.8f;
                    }
                    break;
                case 2:
                    switch (phase)
                    {
                        case 1 or 2:
                            if (counter == 30)
                            {
                                Projectile.damage = (int)(Projectile.damage * 1.5f);
                            }
                            if (counter == 60)
                            {
                                Projectile.damage = (int)(Projectile.damage * 0.6667f);
                            }
                            break;
                        case 3:
                            if (counter == 30)
                            {
                                Projectile.damage = (int)(Projectile.damage * 2f);
                            }
                            break;
                    }
                    MovementAI(player);
                    break;
                case 3:
                    GvMove(player);
                    break;
                case 4:
                    Projectile.velocity *= 0.86f;
                    if (counter >= 60)
                    {
                        Projectile.Center = rotationCenter + offset;
                        offset = offset.RotatedBy(0.09162979);
                        Projectile.velocity = rotationCenter.DirectionTo(Projectile.Center)*0.01f;
                        if (counter == 180)
                        {
                            target = rotationCenter.DirectionTo(Main.MouseWorld);
                        }
                        if (counter > 180)
                        {
                            rotationCenter += target * 12;
                        }
                    } else
                    {
                        offset = Projectile.Center - rotationCenter;
                    }
                    break;
                default:
                    if (adept.IsUsingPrimaryAbility && adept.CanUsePrimary)
                    {
                        Projectile.timeLeft = 2;
                    }
                    break;
            }
            counter++;
        }

        private void GvMove(Player player)
        {
            Projectile.Center = rotationCenter + offset;
            Projectile.velocity = rotationCenter.DirectionTo(Projectile.Center);
            switch (phase)
            {
                case 1:
                    if (counter >= 20)
                    {
                        phase++;
                        counter = 0;
                    }
                    break;
                case 2:
                    offset = offset.RotatedBy(0.1919862f * Projectile.ai[2]);
                    if (counter == 10)
                    {
                        Projectile.NewProjectile(player.GetSource_FromThis(), Projectile.Center, 
                            Projectile.velocity * 16, ModContent.ProjectileType<SwordWave>(), 
                            (int)(Projectile.damage * 1.25f), 9, player.whoAmI);
                    }
                    if (counter >= 21)
                    {
                        phase++;
                        counter = 0;
                    }
                    break;
                case 3:
                    Projectile.alpha -= 12;
                    if (counter >= 21)
                    {
                        Projectile.timeLeft = -2;
                    }
                    break;
            }
        }

        private void MovementAI(Player player)
        {
            float value = Map(counter, 30, 45, 0, (float)Math.PI, true);
            switch (phase)
            {
                case 1:
                    Projectile.Center = player.Center + new Vector2(-160, -30);
                    Projectile.velocity.X = (float)Math.Sin(value) * 0.1f;
                    Projectile.velocity.Y = -(float)Math.Cos(value) * 0.1f;
                    if (counter >= 60)
                    {
                        phase++;
                        counter = 0;
                        Projectile.velocity.X = 0;
                        Projectile.velocity.Y = 0.01f;
                    }
                    break;
                case 2:
                    Projectile.Center = player.Center + new Vector2(160, -30);
                    Projectile.velocity.X = -(float)Math.Sin(value) * 0.1f;
                    Projectile.velocity.Y = -(float)Math.Cos(value) * 0.1f;
                    if (counter >= 60)
                    {
                        phase++;
                        counter = 0;
                    }
                    break;
                case 3:
                    if (counter <= 30)
                    {
                        Projectile.Center = player.Center + new Vector2(0, -280);
                    }
                    Projectile.velocity.X = 0;
                    Projectile.velocity.Y = 20;
                    if (counter >= 60)
                    {
                        Projectile.timeLeft = 0;
                    }
                    break;
            }
        }

        private float Map(float x, float inMin, float inMax, float outMin, float outMax, bool clamp=false)
        {
            float newVal = (outMax - outMin) * (100 * (x - inMin) / (inMax - inMin)) / 100;
            if (clamp)
            {
                newVal = newVal < outMin ? outMin : newVal > outMax ? outMax : newVal;
            }
            return newVal;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float widthMultiplier = 45f;
            float collisionPoint = 0f;

            Rectangle swordHitboxBounds = new Rectangle(0, 0, 400, 400);

            swordHitboxBounds.X = (int)Projectile.position.X - swordHitboxBounds.Width / 2;
            swordHitboxBounds.Y = (int)Projectile.position.Y - swordHitboxBounds.Height / 2;

            Vector2 tip = Projectile.Right.RotatedBy(Projectile.rotation, Projectile.Center);
            Vector2 root = Projectile.TopLeft.RotatedBy(Projectile.rotation, Projectile.Center);
            Vector2 root2 = Projectile.BottomLeft.RotatedBy(Projectile.rotation, Projectile.Center);

            if (swordHitboxBounds.Intersects(targetHitbox)
                && (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, tip, widthMultiplier * Projectile.scale, ref collisionPoint)
                || Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, root, widthMultiplier * Projectile.scale, ref collisionPoint)
                || Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, root2, widthMultiplier * Projectile.scale, ref collisionPoint)
                || Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), root, root2, widthMultiplier * Projectile.scale, ref collisionPoint)))
            {
                return true;
            }
            return false;
        }
    }
}
