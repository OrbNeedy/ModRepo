using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace gvmod.Content.Projectiles
{
    public class Cyclone : ModProjectile
    {
        private Asset<Texture2D> cyclone = ModContent.Request<Texture2D>("gvmod/Content/Projectiles/RightThunder");

        public override void SetDefaults()
        {
            Projectile.light = 1f;
            Projectile.damage = 50;
            Projectile.knockBack = 18;
            Projectile.Size = new Vector2(96, 1000);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.timeLeft = 45;
            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.ownerHitCheck = false;
        }

        public override void AI()
        {
            Projectile.velocity = new(-0.001f, 0);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            for (int i = 0; i <= 4; i++)
            {
                var position = Projectile.TopRight - Main.screenPosition + new Vector2(0f, 250 * -i);
                var scale = Vector2.One;
                Main.EntitySpriteDraw(
                    cyclone.Value,
                    position,
                    null,
                    Color.White*0.3f,
                    0f,
                    cyclone.Size(),
                    scale,
                    SpriteEffects.None,
                    0
                );
            }
            for (int i = 0; i <= 4; i++)
            {
                var position = Projectile.TopRight - Main.screenPosition + new Vector2(0f, 250 * -i);
                var scale = Vector2.One;
                Main.EntitySpriteDraw(
                    cyclone.Value,
                    position,
                    null,
                    Color.White*0.3f,
                    0f,
                    cyclone.Size(),
                    scale,
                    SpriteEffects.FlipHorizontally,
                    0
                );
            }
            return false;
        }
    }
}
