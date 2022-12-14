using gvmod.Common.Players.Septimas;
using gvmod.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Content.Projectiles
{
    public class FlashfieldStriker : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flashfield");
        }

        public override void SetDefaults()
        {
            Projectile.damage = 1;
            Projectile.knockBack = 0;
            Projectile.Size = new Vector2(352);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.light = 1f;
            Projectile.timeLeft = 2;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.ownerHitCheck = false;
            Projectile.alpha = 128;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            Projectile.Center = player.Center;
            if (Projectile.ai[0] == -1)
            {
                Projectile.penetrate = 2;
            }

            switch (Projectile.ai[1]) {
                case 1:
                    if (adept.IsUsingSpecialAbility)
                    {
                        Projectile.timeLeft = 2;
                    }
                    break;
                default:
                    if (adept.IsUsingPrimaryAbility && adept.CanUsePrimary)
                    {
                        Projectile.timeLeft = 2;
                    }
                    break;
            }

            if (Projectile.ai[1] == 1)
            {
                if (adept.IsUsingPrimaryAbility && adept.CanUsePrimary)
                {
                    Projectile.timeLeft = 2;
                }
            }
            if (Projectile.ai[1] == 1)
            {
                if (adept.IsUsingPrimaryAbility && adept.CanUsePrimary)
                {
                    Projectile.timeLeft = 2;
                }
            }
        }

        //Thanks for the code, Blushiemagic
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 ellipsePosition = new Vector2(projHitbox.Left, projHitbox.Top);
            Vector2 ellipseDimentions = new Vector2(projHitbox.Width, projHitbox.Height);
            Vector2 ellipseCenter = ellipsePosition + 0.5f * ellipseDimentions;
            float x = 0f;
            float y = 0f;
            if (targetHitbox.Left > ellipseCenter.X)
            {
                x = targetHitbox.Left - ellipseCenter.X;
            }
            else if (targetHitbox.Left + targetHitbox.Width < ellipseCenter.X)
            {
                x = targetHitbox.Left + targetHitbox.Width - ellipseCenter.X;
            }
            if (targetHitbox.Top > ellipseCenter.Y)
            {
                y = targetHitbox.Top - ellipseCenter.Y;
            }
            else if (targetHitbox.Top + targetHitbox.Height < ellipseCenter.Y)
            {
                y = targetHitbox.Top + targetHitbox.Height - ellipseCenter.Y;
            }
            float a = ellipseDimentions.X / 2f;
            float b = ellipseDimentions.Y / 2f;
            return (x * x) / (a * a) + (y * y) / (b * b) <= 1;
        }
    }
}
