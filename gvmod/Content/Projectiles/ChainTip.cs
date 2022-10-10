using gvmod.Common.Players;
using gvmod.Common.Players.Septimas;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Projectiles
{
    public class ChainTip: ModProjectile
    {
        // State 1: Movement, State 2: Waiting, State 3: Set it to electrify on death, set life to a reasonable low time
        public int aiState = 1;
        public bool electrify = false;
        private Vector2 startingPosition = new Vector2();
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Voltaic chain");
        }

        public override void SetDefaults()
        {
            Projectile.light = 1f;
            Projectile.damage = 50;
            Projectile.knockBack = 0;
            Projectile.Size = new Vector2(42);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.timeLeft = 600;
            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.ownerHitCheck = false;
            startingPosition = Projectile.Center;
        }

        public override void AI()
        {
            if (aiState == 2 || Projectile.timeLeft <= 590)
            {
                Projectile.velocity *= 0;
            }
            if (aiState == 3)
            {
                electrify = true;
                Projectile.timeLeft = Main.rand.Next(20, 50);
            }
            if (Projectile.timeLeft <= 50 && electrify)
            {
                // Put attack method here
            }
            base.AI();
        }

        public override bool PreDrawExtras()
        {
            Asset<Texture2D> chainTexture = ModContent.Request<Texture2D>("gvmod/Content/Projectiles/ChainTipChain");
            Rectangle? chainSourceRectangle = null;
            float chainHeightAdjustment = 0f;
            Vector2 chainDrawPosition = Projectile.Center;
            Vector2 vector = startingPosition - Projectile.Center;
            Vector2 unitVector = vector.SafeNormalize(Vector2.Zero);
            float chainSegmentLength = (chainSourceRectangle.HasValue ? chainSourceRectangle.Value.Height : chainTexture.Height()) + chainHeightAdjustment;
            if (chainSegmentLength == 0) chainSegmentLength = 10;
            float chainRotation = unitVector.ToRotation() + MathHelper.PiOver2;
            int chains = 0;
            float chainLengthRemainingToDraw = vector.Length() + chainSegmentLength / 2f;
            while (chainLengthRemainingToDraw > 0f)
            {
                Main.spriteBatch.Draw(chainTexture.Value, chainDrawPosition - Main.screenPosition, chainSourceRectangle, Color.White, chainRotation, startingPosition, 1f, SpriteEffects.None, 0f);
                chainDrawPosition += unitVector * chainSegmentLength;
                chains++;
                chainLengthRemainingToDraw -= chainSegmentLength;
            }
            if (aiState >= 2)
            {
                Texture2D projectileTexture = TextureAssets.Projectile[Projectile.type].Value;
                Vector2 drawOrigin = new Vector2(projectileTexture.Width * 0.5f, Projectile.height * 0.5f);
                SpriteEffects spriteEffects = SpriteEffects.None;
                if (Projectile.spriteDirection == -1)
                    spriteEffects = SpriteEffects.FlipHorizontally;
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(Color.White) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                    Main.spriteBatch.Draw(projectileTexture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale - k / (float)Projectile.oldPos.Length / 3, spriteEffects, 0f);
                }
            }
            return true;
        }

        public override void Kill(int timeLeft)
        {
            // TODO: Make custom dust
            Vector2 vector = startingPosition - Projectile.Center;
            int magnitude = (int)Math.Sqrt(Math.Pow(startingPosition.X - Projectile.Center.X, 2) + Math.Pow(startingPosition.Y - Projectile.Center.Y, 2));
            for (int i = 1; i <= magnitude + 1; i++)
            {
                Vector2 unitVector = vector.SafeNormalize(Vector2.Zero);
                for (int k = 0; k <= 5; k++)
                {
                    Dust.NewDust((unitVector * magnitude) + new Vector2(Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-6, 6)), 10, 10, DustID.BlueTorch);
                }
            }
            base.Kill(timeLeft); 
        }
    }
}
