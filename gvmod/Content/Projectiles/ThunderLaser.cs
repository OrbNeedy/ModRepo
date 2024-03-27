using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;

namespace gvmod.Content.Projectiles
{
    public class ThunderLaser : ModProjectile
    {
        private bool electrify = true;
        private Vector2 startingPosition = new Vector2(0, 0);
        private int realDamage = 0;

        public override void SetDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 24000;
            Projectile.light = 1f;
            Projectile.ignoreWater = true;
            Projectile.damage = 50;
            Projectile.knockBack = 0;
            Projectile.Size = new Vector2(20);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.timeLeft = 20;
            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.ownerHitCheck = true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            startingPosition = Projectile.Center;
            realDamage = Projectile.damage;
            Projectile.velocity.SafeNormalize(Vector2.One);
            Projectile.Center = startingPosition + (Projectile.velocity*800);
            Projectile.velocity = Vector2.Zero;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (electrify)
            {
                Player player = Main.player[Projectile.owner];
                float widthMultiplier = 10f;
                float collisionPoint = 0f;

                Rectangle chainHitboxBounds = new(0, 0, 900, 900);

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
            Asset<Texture2D> thunderTexture = ModContent.Request<Texture2D>("gvmod/Content/Projectiles/ThunderLaser");
            Vector2 origin = startingPosition;
            Vector2 center = Projectile.Center;
            Vector2 directionToOrigin = origin - Projectile.Center;
            float chainRotation = directionToOrigin.ToRotation();
            float distanceToOrigin = directionToOrigin.Length();

            while (distanceToOrigin > 20f && !float.IsNaN(distanceToOrigin))
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
