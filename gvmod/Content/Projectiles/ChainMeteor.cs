using gvmod.Common.Players;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace gvmod.Content.Projectiles
{
    public class ChainMeteor : ModProjectile
    {
        // 1: Accumulate chains, 2: Shoot thunders, 3: Fly offscreen, 4-8: Attack pattern
        private int phase;
        private int timer;
        private List<Vector2> thunderDirections;

        public override void SetDefaults()
        {
            Projectile.light = 1f;
            Projectile.ignoreWater = true;
            Projectile.damage = 0;
            Projectile.knockBack = 15;
            Projectile.Size = new Vector2(128);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.timeLeft = 3000;
            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.ownerHitCheck = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            phase = 1;
            timer = 0;
            float rotateTo45 = 0.7071067812f;
            thunderDirections = new() { new(0, -1), new(-1, 0), new(-rotateTo45, rotateTo45), 
                new(rotateTo45, -rotateTo45), new(1, 0), new(rotateTo45), new(-rotateTo45) };
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            timer++;
            MovementAI(player);
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

        private void MovementAI(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            int xPos = 800;
            int yPos = 600;
            int velocityX = 0;
            int velocityY = 0;
            switch (phase)
            {
                case 1:
                    Projectile.Center = player.Center + new Vector2(0, -200);
                    if (timer < 240 && timer%18 == 0)
                    {
                        Vector2 velocity = new Vector2(1, 0).RotatedByRandom(MathHelper.TwoPi);
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), 
                            Projectile.Center + (velocity * -600), velocity * 8, 
                            ModContent.ProjectileType<EmptyChain>(), 10, 15, Projectile.owner, 0);
                    }
                    if (timer == 240)
                    {
                        Projectile.damage = (int)(440 * adept.SpecialDamageEquipMult * adept.SpecialDamageEquipMult);
                    }
                    if (timer >= 300)
                    {
                        phase++;
                        Projectile.Center = Main.MouseWorld + new Vector2(xPos, -yPos);
                        timer = 0;
                    }
                    break;
                case 2:
                    if (timer % 30 == 0 && timer < 240)
                    {
                        Vector2 velocity;
                        if (thunderDirections.Count <= 0)
                        {
                            velocity = Vector2.One.RotatedByRandom(MathHelper.TwoPi);
                        } else
                        {
                            velocity = thunderDirections[0];
                            thunderDirections.RemoveAt(0);
                        }
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(),
                            Projectile.Center, velocity,
                            ModContent.ProjectileType<ThunderLaser>(), (int)(Projectile.damage*0.7), 15, 
                            Projectile.owner, 0);
                    }
                    if (timer >= 240)
                    {
                        velocityY = -10;
                    }
                    else
                    {
                        Projectile.Center = player.Center + new Vector2(0, -200);
                    }
                    if (timer >= 300)
                    {
                        phase++;
                        Projectile.Center = Main.MouseWorld + new Vector2(xPos, -yPos);
                        timer = 0;
                    }
                    break;
                case 3:
                    velocityX = -16;
                    velocityY = 12;
                    if (timer >= 90)
                    {
                        phase++;
                        Projectile.Center = Main.MouseWorld + new Vector2(-xPos, -yPos);
                        timer = 0;
                    }
                    break;
                case 4:
                    velocityX = 16;
                    velocityY = 12;
                    if (timer >= 90)
                    {
                        phase++;
                        Projectile.Center = Main.MouseWorld + new Vector2(-xPos, 0);
                        timer = 0;
                    }
                    break;
                case 5:
                    velocityX = 16;
                    if (timer >= 90)
                    {
                        phase++;
                        Projectile.Center = Main.MouseWorld + new Vector2(xPos, 0);
                        timer = 0;
                    }
                    break;
                case 6:
                    velocityX = -16;
                    if (timer >= 90)
                    {
                        phase++;
                        Projectile.Center = Main.MouseWorld + new Vector2(0, -yPos);
                        timer = 0;
                    }
                    break;
                case 7:
                    velocityY = 16;
                    if (timer >= 90)
                    {
                        phase++;
                        timer = 0;
                    }
                    break;
                default:
                    Projectile.timeLeft = 0;
                    break;
            }
            Projectile.velocity = new Vector2(velocityX, velocityY);
        }

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
            return (x * x) / (a * a) + (y * y) / (b * b) <= (1 + Main.rand.NextFloat(0.01f, 0.05f));
        }
    }
}
