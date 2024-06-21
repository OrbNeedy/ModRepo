using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;

namespace gvmod.Content.Projectiles
{
    public class CapsuleSphere : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(70, 116);
            Projectile.light = 1f;
            Projectile.scale = 1f;
            Projectile.ignoreWater = true;
            Main.projFrames[Projectile.type] = 3;

            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.damage = 100;
            Projectile.knockBack = 10;
            Projectile.penetrate = -1;

            Projectile.tileCollide = false;
            Projectile.timeLeft = 90;
            Projectile.ownerHitCheck = false;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
        }

        public override void AI()
        {
            if (++Projectile.frameCounter >= 4)
            {
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
                Projectile.frameCounter = 0;
            }
        }
    }
}
