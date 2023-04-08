using gvmod.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Formats.Asn1.AsnWriter;

namespace gvmod.Content.Pets
{
    public class LolaPetProjectile :ModProjectile
    {
        public static Asset<Texture2D> ballEyesGlowmask;
        public static Asset<Texture2D> ballWingHologram;
        public static Asset<Texture2D> ballColorDraw;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                ballEyesGlowmask = ModContent.Request<Texture2D>("gvmod/Content/Pets/LolaPetProjectileEyesGlowmask");
                ballWingHologram = ModContent.Request<Texture2D>("gvmod/Content/Pets/LolaPetProjectileHologramColorGlowmask");
                ballColorDraw = ModContent.Request<Texture2D>("gvmod/Content/Pets/LolaPetProjectileBodyColor");
            }
        }

        public override void Unload()
        {
            ballEyesGlowmask = null;
            ballWingHologram = null;
            ballColorDraw = null;
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Lola");

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

        public override void PostDraw(Color lightColor)
        {
            AdeptPlayer adept = Main.player[Projectile.owner].GetModPlayer<AdeptPlayer>();

            Texture2D eyes = ballEyesGlowmask.Value;
            Texture2D hologram = ballWingHologram.Value;
            Texture2D body = ballColorDraw.Value;
            SpriteEffects flipping;
            if (Projectile.spriteDirection == -1)
            {
                flipping = SpriteEffects.FlipHorizontally;
            } else
            {
                flipping = SpriteEffects.None;
            }

            Vector2 drawPos = new Vector2
                (
                    Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f,
                    Projectile.position.Y - Main.screenPosition.Y + Projectile.height - hologram.Height * 0.5f
                );

            Main.EntitySpriteDraw(
                eyes, 
                drawPos,
                new Rectangle(0, 0, eyes.Width, eyes.Height), 
                Color.White,
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
                adept.Septima.ClearColor,
                Projectile.rotation,
                hologram.Size() * 0.5f,
                1f,
                flipping,
                0
            );

            Main.EntitySpriteDraw(
                body,
                drawPos,
                new Rectangle(0, 0, body.Width, body.Height),
                Lighting.GetColor(Projectile.Center.ToTileCoordinates(), adept.Septima.MainColor),
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

            Movement(player);
        }

        private void CheckActive(Player player)
        {
            if (!player.dead && player.HasBuff(ModContent.BuffType<LolaPetBuff>()))
            {
                Projectile.timeLeft = 2;
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
