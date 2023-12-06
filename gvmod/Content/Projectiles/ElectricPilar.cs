using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ReLogic.Content;
using Terraria.DataStructures;

namespace gvmod.Content.Projectiles
{
    public class ElectricPilar : ModProjectile
    {
        private Asset<Texture2D> pillar = ModContent.Request<Texture2D>("gvmod/Content/Projectiles/TruePilar");
        private static float rotation = MathHelper.Pi / 50;
        private int step = 0;
        private int phase = 1;

        public override void SetDefaults()
        {
            Projectile.light = 1f;
            Projectile.damage = 180;
            Projectile.knockBack = 8;
            Projectile.Size = new Vector2(24, 800);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.timeLeft = 900;
            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.ownerHitCheck = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.ai[0] < 0)
            {
                Projectile.rotation = MathHelper.Pi;
            } else
            {
                Projectile.rotation = 0;
            }
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            switch (phase)
            {
                case 1:
                    if (step >= 40)
                    {
                        phase++;
                        step = 0;
                    }
                    break;
                case 2:
                    Projectile.rotation += rotation;
                    if (step >= 400)
                    {
                        phase++;
                        step = 0;
                    }
                    break;
                case 3:
                    if (step >= 40)
                    {
                        Projectile.timeLeft = 0;
                    }
                    break;
                default:
                    break;
            }
            step++;
        }

        /*public override bool PreDraw(ref Color lightColor)
        {
            Color color = Color.White;
            if (Projectile.ai[0] < 0) color = Color.Violet;
            else color = Color.Yellow;
            for (int i = 0; i <= 3; i++)
            {
                var position = -Main.screenPosition + new Vector2(0f, 250 * i * Projectile.ai[0]).RotatedBy(Projectile.rotation);
                if (Projectile.ai[0] < 0) position += Projectile.BottomRight;
                else position += Projectile.TopRight;
                position += ((pillar.Height()/3) * i * Projectile.Center);
                Main.EntitySpriteDraw(
                    pillar.Value,
                    position,
                    null,
                    Color.White,
                    Projectile.rotation,
                    pillar.Size()*i,
                    1f,
                    SpriteEffects.None
                );
            }
            return true;
        }*/

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float widthMultiplier = 34f;
            float collisionPoint = 0f;

            Rectangle pilarHitboxBounds = new Rectangle(0, 0, 800, 800);

            pilarHitboxBounds.X = (int)Projectile.Center.X - pilarHitboxBounds.Width / 2;
            pilarHitboxBounds.Y = (int)Projectile.Center.Y - pilarHitboxBounds.Height / 2;

            Vector2 top = Projectile.Right.RotatedBy(Projectile.rotation, Projectile.Center);
            Vector2 bottom = Projectile.TopLeft.RotatedBy(Projectile.rotation, Projectile.Center);

            if (pilarHitboxBounds.Intersects(targetHitbox)
                && (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), top, bottom, widthMultiplier * Projectile.scale, ref collisionPoint)))
            {
                return true;
            }
            return false;
        }
    }
}
