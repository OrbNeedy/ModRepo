using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace gvmod.Content.Projectiles
{
    public class SmallRazor : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.scale = 1f;
            Projectile.light = 0.5f;
            Main.projFrames[Projectile.type] = 3;

            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.damage = 36;
            Projectile.knockBack = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 0;
            Projectile.penetrate = -1;

            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 150;
            Projectile.ownerHitCheck = false;
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
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.TwoPi;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            return base.PreDraw(ref lightColor);
        }
    }
}
