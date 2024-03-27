using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ReLogic.Content;
using Terraria.DataStructures;

namespace gvmod.Content.Projectiles
{
    public class ElectricPilar : ModProjectile
    {
        private static float rotation = 0.06298932639f;//MathHelper.TwoPi / 99;
        private int step = 0;
        private int phase = 1;
        private Vector2 startingPosition = new(0, 0);
        private Vector2 truePosition = new(0, 0);

        public override void SetDefaults()
        {
            Projectile.light = 1f;
            Projectile.damage = 180;
            Projectile.knockBack = 8;
            Projectile.Size = new Vector2(20, 20);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.timeLeft = 900;
            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.ownerHitCheck = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            startingPosition = Projectile.Center;
            Projectile.velocity.SafeNormalize(Vector2.One);
            truePosition = Projectile.velocity * 700;
            Projectile.Center = startingPosition + truePosition;
            Projectile.velocity = Vector2.Zero;
        }

        public override void AI()
        {
            switch (phase)
            {
                case 1:
                    if (step >= 40)
                    {
                        phase++;
                        step = 0;
                    }
                    break;
                case 2:
                    Projectile.Center = truePosition + startingPosition;
                    Projectile.velocity = startingPosition.DirectionTo(Projectile.Center).RotatedBy(MathHelper.PiOver2) * 0.0001f;
                    truePosition = truePosition.RotatedBy(rotation);
                    if (step >= 400)
                    {
                        phase++;
                        step = 0;
                    }
                    break;
                case 3:
                    if (step >= 40)
                    {
                        Projectile.timeLeft = 0;
                    }
                    break;
                default:
                    break;
            }
            step++;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[Projectile.owner];
            float widthMultiplier = 20f;
            float collisionPoint = 0f;
            
            Rectangle chainHitboxBounds = new(0, 0, 800, 800);

            chainHitboxBounds.X = (int)player.Center.X - chainHitboxBounds.Width / 2;
            chainHitboxBounds.Y = (int)player.Center.Y - chainHitboxBounds.Height / 2;

            Vector2 tip = Projectile.Right.RotatedBy(Projectile.velocity.ToRotation(), Projectile.Center);
            Vector2 root = startingPosition;

            if (chainHitboxBounds.Intersects(targetHitbox)
                && Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), tip, root,
                widthMultiplier * Projectile.scale, ref collisionPoint))
            {
                return true;
            }
            return base.Colliding(projHitbox, targetHitbox);
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

        public override bool PreDrawExtras()
        {
            Asset<Texture2D> thunderTexture = ModContent.Request<Texture2D>("gvmod/Content/Projectiles/ElectricPilar");
            Vector2 origin = startingPosition;
            Vector2 center = Projectile.Center;
            Vector2 directionToOrigin = origin - Projectile.Center;
            float chainRotation = directionToOrigin.ToRotation() - MathHelper.PiOver2;
            float distanceToOrigin = directionToOrigin.Length();

            while (distanceToOrigin > 200f && !float.IsNaN(distanceToOrigin))
            {
                directionToOrigin /= distanceToOrigin;
                directionToOrigin *= thunderTexture.Height();

                center += directionToOrigin;
                directionToOrigin = origin - center;
                distanceToOrigin = directionToOrigin.Length();

                SpriteEffects rotationEffect;
                if (Projectile.spriteDirection == -1)
                {
                    rotationEffect = SpriteEffects.FlipHorizontally;
                }
                else
                {
                    rotationEffect = SpriteEffects.None;
                }

                Main.EntitySpriteDraw(thunderTexture.Value,
                    center - Main.screenPosition,
                    thunderTexture.Value.Bounds,
                    Color.White,
                    chainRotation,
                    thunderTexture.Size() * 0.5f,
                    1f,
                    rotationEffect,
                    0);
            }
            return false;
        }
    }
}
