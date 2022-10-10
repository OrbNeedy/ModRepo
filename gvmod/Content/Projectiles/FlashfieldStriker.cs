using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Content.Projectiles
{
    public class FlashfieldStriker : ModProjectile
    {
        float collisionWidth => 50f * Projectile.scale;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flashfield");
        }

        public override void SetDefaults()
        {
            Projectile.damage = 1;
            Projectile.knockBack = 0;
            Projectile.Size = new Vector2(176);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.timeLeft = 4;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.ownerHitCheck = false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.Center = player.Center;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 start = Projectile.Center;
            Vector2 end = start * Projectile.width;

            float collisionPoint = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, collisionWidth, ref collisionPoint);
        }
    }
}
