using gvmod.Common.Players;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Content.Projectiles
{
    public class ElectricSword : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Luxcaliburg");
        }

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
            AdeptPlayer adept = Main.player[Projectile.owner].GetModPlayer<AdeptPlayer>();
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
                default:
                    if (adept.IsUsingPrimaryAbility && adept.CanUsePrimary)
                    {
                        Projectile.timeLeft = 2;
                    }
                    break;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rotationFactor = Projectile.rotation + (float)Math.PI / 4f;
            float verticalScaleFactor = 190f;
            float widthMultiplier = 45f;
            float collisionPoint = 0f;

            Rectangle swordHitboxBounds = new Rectangle(0, 0, 400, 400);

            swordHitboxBounds.X = (int)Projectile.position.X - swordHitboxBounds.Width / 2;
            swordHitboxBounds.Y = (int)Projectile.position.Y - swordHitboxBounds.Height / 2;

            Vector2 hitLineEnd = Projectile.Center + rotationFactor.ToRotationVector2() * 180f;
            //How do I even do this?
            Vector2 verticalHitLineEnd = Projectile.Center + rotationFactor.ToRotationVector2() * verticalScaleFactor;
            Vector2 verticalHitLineEnd2 = Projectile.Center + rotationFactor.ToRotationVector2() * verticalScaleFactor;

            if (swordHitboxBounds.Intersects(targetHitbox)
                && Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd, widthMultiplier * Projectile.scale, ref collisionPoint))
            {
                return true;
            }
            return false;
        }
    }
}
