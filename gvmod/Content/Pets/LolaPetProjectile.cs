using gvmod.Common.Players;
using gvmod.Content.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace gvmod.Content.Pets
{
    public class LolaPetProjectile :ModProjectile
    {
        public static Asset<Texture2D> ballEyesGlowmask;
        public static Asset<Texture2D> ballWingHologram;
        public static Asset<Texture2D> ballColorDraw;

        public static Asset<Texture2D> ultraBody;
        public static Asset<Texture2D> ultraWings;
        public static Asset<Texture2D> ultraWingsGlowmask;

        public bool ultraMode = false;
        public int wouldBeFrame = 0;
        public int wouldBeMaxFrames = 3;
        public int frameTimer = 0;

        public Vector2 wingPosition;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                ballEyesGlowmask = ModContent.Request<Texture2D>("gvmod/Content/Pets/LolaPetProjectileEyesGlowmask");
                ballWingHologram = ModContent.Request<Texture2D>("gvmod/Content/Pets/LolaPetProjectileHologramColorGlowmask");
                ballColorDraw = ModContent.Request<Texture2D>("gvmod/Content/Pets/LolaPetProjectileBodyColor");
                
                ultraBody = ModContent.Request<Texture2D>("gvmod/Content/Pets/LolaPet_Ultra");
                ultraWings = ModContent.Request<Texture2D>("gvmod/Content/Pets/LolaPet_Wings");
                ultraWingsGlowmask = ModContent.Request<Texture2D>("gvmod/Content/Pets/LolaPet_Wings_Glowmask");
            }
        }

        public override void Unload()
        {
            ballEyesGlowmask = null;
            ballWingHologram = null;
            ballColorDraw = null;

            ultraBody = null;
            ultraWings = null;
            ultraWingsGlowmask = null;
        }

        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 30;
            Projectile.scale = 1f;
            Projectile.light = 0f;

            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.damage = 0;
            Projectile.knockBack = 0;

            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 2;
        }

        public override void OnSpawn(IEntitySource source)
        {
            wingPosition = Projectile.position;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Only return false if it's in Ultra mode, draw normally otherwise
            if (!ultraMode) return true;
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            int dir = Main.player[Projectile.owner].direction;
            SpriteEffects flipping = SpriteEffects.None;
            if (dir == -1)
            {
                flipping = SpriteEffects.FlipHorizontally;
            }

            float alpha = 1-(Projectile.alpha / 255f); // The projectile's alpha maped from 0 to 1 to multiply by color

            // If ultra mode is active, do not draw sphere glowmasks, draw wings instead
            if (ultraMode)
            {
                // Wings draw
                Texture2D wing = ultraWings.Value;
                Texture2D glowmask = ultraWingsGlowmask.Value;

                int wingHeight = wing.Height / wouldBeMaxFrames;
                int glowmaskHeight = glowmask.Height / wouldBeMaxFrames;

                Rectangle sourceRectangle = new Rectangle(0, wingHeight * wouldBeFrame, wing.Width, wingHeight);

                Vector2 wingDrawPos = new Vector2
                (
                    wingPosition.X - Main.screenPosition.X + (wing.Width * 0.5f) * dir,
                    wingPosition.Y - Main.screenPosition.Y + (wing.Height * 0.5f)
                );

                Vector2 secondWingDrawPos = new Vector2
                (
                    wingPosition.X - Main.screenPosition.X + (wing.Width * 0.5f) * dir,
                    wingPosition.Y - Main.screenPosition.Y + (wing.Height * 0.5f)
                );

                if (dir == 1)
                {
                    secondWingDrawPos.X += 30;
                } else
                {
                    wingDrawPos.X += 15;
                    secondWingDrawPos.X -= 15;
                }

                // Main Body draw
                Texture2D texture = ultraBody.Value;

                int frameHeight = texture.Height;
                int startY = 0;

                Rectangle sourceRectangle2 = new Rectangle(0, startY, texture.Width, frameHeight);

                Vector2 origin2 = sourceRectangle.Size() / 2f;

                Color drawColor = Projectile.GetAlpha(lightColor);

                Main.EntitySpriteDraw(
                    wing,
                    secondWingDrawPos,
                    sourceRectangle,
                    Lighting.GetColor(Projectile.Center.ToTileCoordinates(), Color.White) * alpha,
                    Projectile.rotation,
                    wing.Size() * 0.5f,
                    1f,
                    flipping,
                    0
                );

                Main.EntitySpriteDraw(
                    glowmask,
                    secondWingDrawPos,
                    new Rectangle(0, glowmaskHeight * wouldBeFrame, glowmask.Width, glowmaskHeight),
                    Color.White * alpha,
                    Projectile.rotation,
                    glowmask.Size() * 0.5f,
                    1f,
                    flipping,
                    0
                );

                Main.EntitySpriteDraw(texture,
                    Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                    sourceRectangle2, drawColor, Projectile.rotation, origin2, Projectile.scale, flipping, 0);

                Main.EntitySpriteDraw(
                    wing,
                    wingDrawPos,
                    sourceRectangle,
                    Lighting.GetColor(Projectile.Center.ToTileCoordinates(), Color.White) * alpha,
                    Projectile.rotation,
                    wing.Size() * 0.5f,
                    1f,
                    flipping,
                    0
                );

                Main.EntitySpriteDraw(
                    glowmask,
                    wingDrawPos,
                    new Rectangle(0, glowmaskHeight * wouldBeFrame, glowmask.Width, glowmaskHeight),
                    Color.White * alpha,
                    Projectile.rotation,
                    glowmask.Size() * 0.5f,
                    1f,
                    flipping,
                    0
                );

                return;
            }

            // Get the player's septima color to change Lola's body color
            AdeptPlayer adept = Main.player[Projectile.owner].GetModPlayer<AdeptPlayer>();

            Texture2D eyes = ballEyesGlowmask.Value;
            Texture2D hologram = ballWingHologram.Value;
            Texture2D body = ballColorDraw.Value;

            Vector2 drawPos = new Vector2
                (
                    Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f,
                    Projectile.position.Y - Main.screenPosition.Y + Projectile.height - hologram.Height * 0.5f
                );

            Main.EntitySpriteDraw(
                eyes, 
                drawPos,
                new Rectangle(0, 0, eyes.Width, eyes.Height), 
                Color.White*alpha,
                Projectile.rotation,
                eyes.Size() * 0.5f, 
                1f, 
                flipping, 
                0
            );

            Main.EntitySpriteDraw(
                hologram,
                drawPos,
                new Rectangle(0, 0, hologram.Width, hologram.Height),
                adept.Septima.ClearColor * alpha,
                Projectile.rotation,
                hologram.Size() * 0.5f,
                1f,
                flipping,
                0
            );

            // Since the body's color depends on the enviroment light, we use Lighting.GetColor to modify it
            Main.EntitySpriteDraw(
                body,
                drawPos,
                new Rectangle(0, 0, body.Width, body.Height),
                Lighting.GetColor(Projectile.Center.ToTileCoordinates(), adept.Septima.MainColor) * alpha,
                Projectile.rotation,
                body.Size() * 0.5f,
                1f,
                flipping,
                0
            );
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            CheckActive(player);

            CheckUltra(player);

            UltraMovement(player);

            Movement(player);
        }

        private void UltraMovement(Player player)
        {
            if (!ultraMode)
            {
                wingPosition = Projectile.position;
                return;
            }
            float velDistanceChange = 2f;

            int dir = player.direction;
            
            Vector2 desiredCenterRelative = new Vector2(dir * -50, -50f);

            desiredCenterRelative.Y += (float)Math.Sin(Main.GameUpdateCount / 150f * MathHelper.TwoPi) * 3f;
            desiredCenterRelative.X += (float)Math.Sin(Main.GameUpdateCount / 300f * MathHelper.TwoPi) * 6f;

            Vector2 desiredCenter = Projectile.position + desiredCenterRelative;
            Vector2 betweenDirection = desiredCenter - wingPosition;
            float betweenSQ = betweenDirection.LengthSquared();

            // If too far, set the position
            if (betweenSQ > 1000f * 1000f || betweenSQ < velDistanceChange * velDistanceChange)
            {
                wingPosition = desiredCenter;
            }

            if (betweenDirection != Vector2.Zero)
            {
                wingPosition += betweenDirection * 0.5f;
            }
        }

        private void CheckUltra(Player player)
        {
            int kudos = player.GetModPlayer<AdeptPlayer>().Kudos;
            frameTimer++;
            if (frameTimer >= 45)
            {
                wouldBeFrame++;
                if (wouldBeFrame >= wouldBeMaxFrames) wouldBeFrame = 0;
                frameTimer = 0;
            }

            if (player.HasBuff(ModContent.BuffType<AnthemBuff>()) || kudos >= 1000)
            {
                // If completely transparent, activate Ultra mode and change the used texture
                if (Projectile.alpha >= 255)
                {
                    Main.NewText("Activating ultra mode", new Color(255, 131, 64));
                    ultraMode = true;
                }
                
                // Decrease transparency while not in Ultra mode, increase it while in Ultra mode
                if (!ultraMode)
                {
                    Projectile.alpha += 6;
                } else
                {
                    Projectile.alpha -= 6;
                }
            } else
            {
                // If completely transparent, deactivate Ultra mode
                if (Projectile.alpha >= 255)
                {
                    Main.NewText("Deactivating ultra mode", new Color(255, 131, 64));
                    ultraMode = false;
                }

                // Decrease transparency while in Ultra mode, increase it while not in Ultra mode
                if (!ultraMode)
                {
                    Projectile.alpha -= 6;
                }
                else
                {
                    Projectile.alpha += 6;
                }
            }

            // Clamp Projectile.alpha so it won't cause trouble when fading in and out of Ultra Mode
            Projectile.alpha = (int)MathHelper.Clamp(Projectile.alpha, 0, 255);
        }

        private void CheckActive(Player player)
        {
            if (!player.dead && player.HasBuff(ModContent.BuffType<LolaPetBuff>()))
            {
                Projectile.timeLeft = 60;
            } else
            {
                Projectile.alpha += 5;
            }
        }

        private void Movement(Player player)
        {
            float velDistanceChange = 2f;

            int dir = player.direction;
            Projectile.direction = Projectile.spriteDirection = dir;

            Vector2 desiredCenterRelative = new Vector2(dir * -45, -60f);

            desiredCenterRelative.Y += (float)Math.Sin(Main.GameUpdateCount / 150f * MathHelper.TwoPi) * 15f;
            desiredCenterRelative.X += (float)Math.Sin(Main.GameUpdateCount / 300f * MathHelper.TwoPi) * 18f;

            Vector2 desiredCenter = player.MountedCenter + desiredCenterRelative;
            Vector2 betweenDirection = desiredCenter - Projectile.Center;
            float betweenSQ = betweenDirection.LengthSquared();


            if (betweenSQ > 1000f * 1000f || betweenSQ < velDistanceChange * velDistanceChange)
            {
                Projectile.Center = desiredCenter;
                Projectile.velocity = Vector2.Zero;
            }

            if (betweenDirection != Vector2.Zero)
            {
                Projectile.velocity = betweenDirection * 0.1f;
            }

            // Do not rotate if it's in Ultra Mode
            if (ultraMode) return;
            if (Projectile.velocity.LengthSquared() > 6f * 6f)
            {
                float rotationVel = Projectile.velocity.X * 0.01f + Projectile.velocity.Y * Projectile.spriteDirection * 0.01f;
                if (Math.Abs(Projectile.rotation - rotationVel) >= MathHelper.Pi)
                {
                    if (rotationVel < Projectile.rotation)
                    {
                        Projectile.rotation -= MathHelper.TwoPi;
                    } else
                    {
                        Projectile.rotation += MathHelper.TwoPi;
                    }
                }

                float rotationInertia = 12f;
                Projectile.rotation = (Projectile.rotation * (rotationInertia - 1f) + rotationVel) / rotationInertia;
            }
            else
            {
                if (Projectile.rotation > MathHelper.Pi)
                {
                    Projectile.rotation -= MathHelper.TwoPi;
                }

                if (Projectile.rotation > -0.005f && Projectile.rotation < 0.005f)
                {
                    Projectile.rotation = 0f;
                } else
                {
                    Projectile.rotation *= 0.96f;
                }
            }
        }
    }
}
