using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Content.Projectiles
{
    public class ElectricBullet : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(10);
            Projectile.light = 1f;
            Projectile.scale = 1f;

            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.damage = 185;
            Projectile.knockBack = 12;
            Projectile.penetrate = 1;

            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 150;
            Projectile.ownerHitCheck = false;
            Projectile.alpha = 204;
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

        public override void OnKill(int timeLeft)
        {
            Projectile.NewProjectile(Main.player[Projectile.owner].GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<HomingElectricSphere>(), Projectile.damage, 8, Projectile.owner);
        }
    }
}
