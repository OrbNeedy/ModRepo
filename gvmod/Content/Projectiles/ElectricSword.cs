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
            Projectile.timeLeft = 2;
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
                    break;
                case 2:
                    Projectile.timeLeft = 600;
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
            counter++;
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
                    if (adept.IsUsingPrimaryAbility && adept.CanUsePrimary)
                    {
                        Projectile.penetrate = -1;
                    }
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
                default:
                    if (adept.IsUsingPrimaryAbility && adept.CanUsePrimary)
                    {
                        Projectile.timeLeft = 2;
                    }
                    break;
            }
        }

        private void GvMove(Player player)
        {
            float value = Map(counter, 30, 45, 0, (float)Math.PI);
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            Vector2 direction = (Main.MouseWorld - Projectile.Center);
            direction.Normalize();
            Projectile.Center = player.Center + new Vector2(56 * player.direction, 0);
            switch (phase)
            {
                case 1:
                    Projectile.velocity.X = 0.015f * player.direction;
                    Projectile.velocity.Y = -0.01f;
                    if (counter >= 10)
                    {
                        phase++;
                        counter = 0;
                    }
                    break;
                case 2:
                    Projectile.velocity.X = (float)Math.Sin(value*2) * 0.1f * player.direction;
                    Projectile.velocity.Y = (float)Math.Cos(value*2) * 0.1f;
                    if (counter == 20)
                    {
                        Projectile.NewProjectile(player.GetSource_FromThis(), Projectile.Center, direction * 16, ModContent.ProjectileType<SwordWave>(), (int)(Projectile.damage * adept.SpecialDamageLevelMult * adept.SpecialDamageEquipMult), 8, player.whoAmI);
                    }
                    if (counter >= 40)
                    {
                        phase++;
                        counter = 0;
                    }
                    break;
                case 3:
                    Projectile.velocity.X *= 0.1f;
                    Projectile.velocity.Y *= 0.1f;
                    if (counter >= 10)
                    {
                        Projectile.timeLeft = 0;
                    }
                    break;
            }
        }

        private void MovementAI(Player player)
        {
            float value = Map(counter, 30, 45, 0, (float)Math.PI);
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

        private float Map(float x, float in_min, float in_max, float out_min, float out_max)
        {
            if (x < in_min) x = in_min;
            if (x > in_max) x = in_max;
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float widthMultiplier = 45f;
            float collisionPoint = 0f;

            Rectangle swordHitboxBounds = new Rectangle(0, 0, 400, 400);

            swordHitboxBounds.X = (int)Projectile.position.X - swordHitboxBounds.Width / 2;
            swordHitboxBounds.Y = (int)Projectile.position.Y - swordHitboxBounds.Height / 2;

            Vector2 tip = Projectile.Right.RotatedBy(Projectile.velocity.ToRotation(), Projectile.Center);
            Vector2 root = Projectile.TopLeft.RotatedBy(Projectile.velocity.ToRotation(), Projectile.Center);
            Vector2 root2 = Projectile.BottomLeft.RotatedBy(Projectile.velocity.ToRotation(), Projectile.Center);

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
