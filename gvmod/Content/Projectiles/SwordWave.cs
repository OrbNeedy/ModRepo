using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Content.Projectiles
{
    public class SwordWave : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.light = 1f;
            Projectile.damage = 350;
            Projectile.knockBack = 8;
            Projectile.Size = new Vector2(90, 180);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.timeLeft = 600;
            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.ownerHitCheck = false;
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

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float widthMultiplier = 35f;
            float collisionPoint = 0f;

            Rectangle hitboxBounds = new Rectangle(0, 0, 400, 400);

            hitboxBounds.X = (int)Projectile.position.X - hitboxBounds.Width / 2;
            hitboxBounds.Y = (int)Projectile.position.Y - hitboxBounds.Height / 2;

            Vector2 topleft = Projectile.TopLeft.RotatedBy(Projectile.velocity.ToRotation(), Projectile.Center);
            Vector2 topright = Projectile.TopRight.RotatedBy(Projectile.velocity.ToRotation(), Projectile.Center);
            Vector2 bottomleft = Projectile.BottomLeft.RotatedBy(Projectile.velocity.ToRotation(), Projectile.Center);
            Vector2 bottomright = Projectile.BottomRight.RotatedBy(Projectile.velocity.ToRotation(), Projectile.Center);

            if (hitboxBounds.Intersects(targetHitbox)
                && (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), topleft, topright, widthMultiplier * Projectile.scale, ref collisionPoint)
                || Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), topright, bottomright, widthMultiplier * Projectile.scale, ref collisionPoint)
                || Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), bottomright, bottomleft, widthMultiplier * Projectile.scale, ref collisionPoint)
                || Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), bottomleft, topleft, widthMultiplier * Projectile.scale, ref collisionPoint)))
            {
                return true;
            }
            return false;
        }
    }
}
