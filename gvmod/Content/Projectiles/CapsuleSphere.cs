using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace gvmod.Content.Projectiles
{
    public class CapsuleSphere : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.light = 1f;
            Projectile.ignoreWater = true;
            Projectile.damage = 100;
            Projectile.knockBack = 10;
            Projectile.Size = new Vector2(70, 116);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.timeLeft = 90;
            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.ownerHitCheck = false;
        }
    }
}
