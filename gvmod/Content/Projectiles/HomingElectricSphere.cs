using gvmod.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace gvmod.Content.Projectiles
{
    public class HomingElectricSphere : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(42);
            Projectile.light = 1f;
            Projectile.scale = 1f;

            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.damage = 185;
            Projectile.knockBack = 12;
            Projectile.penetrate = -1;

            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 300;
            Projectile.ownerHitCheck = false;
        }

        public override void AI()
        {
            NPC target = FindClosestNPC(200);

            if (Projectile.timeLeft == 270)
            {
                if (target != null)
                {
                    Projectile.velocity = Projectile.Center.DirectionTo(target.Center) * 12;
                } else
                {
                    Projectile.velocity = Projectile.Center.DirectionTo(Main.MouseWorld) * 12;
                }
            }
        }

        public NPC FindClosestNPC(float range)
        {
            NPC closestNPC = null;
            float squaredRange = range * range;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC target = Main.npc[i];
                if (target.CanBeChasedBy())
                {
                    float distance = Vector2.DistanceSquared(target.Center, Projectile.Center);
                    if (distance < squaredRange)
                    {
                        squaredRange = distance;
                        closestNPC = target;
                    }
                }
            }
            return closestNPC;
        }
    }
}
