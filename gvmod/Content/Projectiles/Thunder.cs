using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using gvmod.Content;
using ReLogic.Content;

namespace gvmod.Content.Projectiles
{
    internal class Thunder : ModProjectile
    {
        private Asset<Texture2D> thunder = ModContent.Request<Texture2D>("gvmod/Content/Projectiles/RightThunder");
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Thunder");
        }

        public override void SetDefaults()
        {
            Projectile.light = 1f;
            Projectile.damage = 50;
            Projectile.knockBack = 12;
            Projectile.Size = new Vector2(32, 1000);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.timeLeft = 15;
            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.ownerHitCheck = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            for (int i = 0; i <= 4; i++)
            {
                var position = Projectile.TopRight - Main.screenPosition + new Vector2(0f, 250 * i);
                Main.EntitySpriteDraw(
                    thunder.Value, 
                    position, 
                    null, 
                    Color.White, 
                    0f, 
                    thunder.Size(), 
                    1f, 
                    SpriteEffects.None, 
                    0
                );
            }
            return false;
        }
    }
}
