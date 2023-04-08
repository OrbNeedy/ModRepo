using gvmod.Common.Players;
using gvmod.Common.Players.Septimas;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace gvmod.Content.Projectiles
{
    public class ElectricBolt : ModProjectile
    {
        private NPC target;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Electric Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.damage = 60;
            Projectile.knockBack = 10;
            Projectile.Size = new Vector2(40);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.timeLeft = 900;
            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.ownerHitCheck = false;
            Projectile.light = 1f;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.ai[0] != -1)
            {
                target = Main.npc[(int)Projectile.ai[0]];
            }
            else
            {
                target = null;
            }
            if (Projectile.ai[1] == 0)
            {
                Projectile.damage = 35;
            }
            else
            {
                Projectile.damage = 0;
            }
        }

        public override void AI()
        {
            if (target == null && Projectile.ai[1] == 0)
            {
                target = FindClosestNPC(200);
            }

            if (target == null) return;

            if (Projectile.timeLeft <= 900)
            {
                if (Projectile.timeLeft <= 875)
                {
                    Projectile.velocity = Vector2.Clamp(Projectile.Center.DirectionTo(target.Center) * 16, new Vector2(-16, -16), new Vector2(16, 16));
                } else
                {
                    if (Projectile.Center.X < target.Center.X && Projectile.velocity.X < 16)
                    {
                        Projectile.velocity.X += 1f;
                    }
                    else if (Projectile.Center.X > target.Center.X && Projectile.velocity.X > -16)
                    {
                        Projectile.velocity.X -= 1f;
                    }
                    if (Projectile.Center.Y < target.Center.Y && Projectile.velocity.Y < 16)
                    {
                        Projectile.velocity.Y += 1f;
                    }
                    else if (Projectile.Center.Y > target.Center.Y && Projectile.velocity.Y > -16)
                    {
                        Projectile.velocity.Y -= 1f;
                    }
                }

                if (Projectile.Center.Distance(target.Center) <= target.Size.Length())
                {
                    Projectile.timeLeft = 0;
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
