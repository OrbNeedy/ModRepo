using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Projectiles
{
    internal class LightningCreeper : ModProjectile
    {
        public Texture2D big = (Texture2D)ModContent.Request<Texture2D>("gvmod/Content/Projectiles/LightningCreeper_Big",
            ReLogic.Content.AssetRequestMode.ImmediateLoad);
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning creeper");
        }

        public override void SetDefaults()
        {
            Projectile.light = 1f;
            Projectile.damage = 35;
            Projectile.knockBack = 10;
            Projectile.Size = new Vector2(64, 12);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.timeLeft = 300;
            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.ownerHitCheck = false;
        }

        public override void AI()
        {
            for (int i = 0; i < 7; i++)
            {
                Dust.NewDust(Projectile.Center, 10, 10, DustID.MartianSaucerSpark, 0);
            }
            if (Projectile.timeLeft == 150)
            {
                Projectile.velocity *= -1f;
                Projectile.Size = new Vector2(64, 24);
                Projectile.damage *= 2;
                Projectile.position += new Vector2(0, -12);
            }
        }

        public override void PostDraw(Color lightColor)
        {
            if (Projectile.timeLeft == 150)
            {
                Main.EntitySpriteDraw(big, Projectile.Center - Main.screenPosition, Projectile.getRect(), Color.White, 0, Projectile.Center - Main.screenPosition, 1f, SpriteEffects.None, 0);
            }
        }
    }
}
