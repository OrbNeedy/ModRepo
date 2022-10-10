using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using gvmod.Content;

namespace gvmod.Content.Projectiles
{
    internal class Thunder : ModProjectile
    {
        public Texture2D thunder = (Texture2D)ModContent.Request<Texture2D>("gvmod/Content/Projectiles/RightThunder",
            ReLogic.Content.AssetRequestMode.ImmediateLoad);
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

        public override void AI()
        {
        }

        public override void PostDraw(Color lightColor)
        {
            for (int i = 1; i <= 4; i++)
            {
                Main.EntitySpriteDraw(thunder, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y + (250 * i)), new Rectangle((int)(Projectile.position.X - Main.screenPosition.X), (int)(Projectile.position.Y - Main.screenPosition.Y + (250 * i)), 32, 250), Color.White, 0, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y + (250 * i)), 1f, SpriteEffects.None, 0);
            }
        }
    }
}
