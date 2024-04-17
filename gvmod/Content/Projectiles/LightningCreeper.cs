using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Projectiles
{
    internal class LightningCreeper : ModProjectile
    {
        private Asset<Texture2D> big = ModContent.Request<Texture2D>("gvmod/Content/Projectiles/LightningCreeper_Big",
            ReLogic.Content.AssetRequestMode.ImmediateLoad);

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(64, 12);
            Projectile.scale = 1f;
            Projectile.light = 1f;
            Main.projFrames[Projectile.type] = 2;

            Projectile.damage = 35;
            Projectile.knockBack = 10;
            Projectile.penetrate = -1;
            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();

            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
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
                Projectile.scale = 1.5f; // Size = new Vector2(64, 24);
                Projectile.Center += new Vector2(0, -12);
                Projectile.damage *= 2;
            }

            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
        }

        public override void PostDraw(Color lightColor)
        {
            
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft <= 150)
            {
                Texture2D texture = big.Value;
                int frameHeight = texture.Height / Main.projFrames[Projectile.type];

                Rectangle sourceRect = new(0, frameHeight * Projectile.frame, texture.Width, frameHeight);
                var position = Projectile.TopRight - Main.screenPosition;
                Main.EntitySpriteDraw(
                    texture,
                    position,
                    sourceRect,
                    Color.White,
                    0f,
                    big.Size() * new Vector2(0.5f, 0.25f),
                    1f,
                    SpriteEffects.None,
                    0
                );
                return false;
            }
            return true;
        }
    }
}
