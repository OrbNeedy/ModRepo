using gvmod.Common.Players;
using gvmod.Common.Players.Septimas;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace gvmod.Content.Projectiles
{
    public class SeptimaBolt: ModProjectile
    {
        private NPC target;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Septima Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.damage = 36;
            Projectile.knockBack = 12;
            Projectile.Size = new Vector2(18);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.timeLeft = 900;
            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.ownerHitCheck = false;
            Projectile.light = 1f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Lighting.GetColor(Projectile.Center.ToTileCoordinates(), Main.player[Projectile.owner].GetModPlayer<AdeptPlayer>().Septima.MainColor);
            return color * Projectile.Opacity;
        }

        public override void OnSpawn(IEntitySource source)
        {
            target = null;
            Projectile.damage = 40;
        }

        public override void AI()
        {
            target = FindClosestNPC(200);

            if (target != null && Projectile.timeLeft <= 900)
            {
                if (!target.active || target.life <= 0 || target.immortal || target.friendly)
                {
                    return;
                }
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
        }

        public NPC FindClosestNPC(int range)
        {
            NPC closestNPC = null;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                var npc = Main.npc[i];
                if (!npc.active && npc.life <= 0) continue;
                if (closestNPC == null)
                {
                    closestNPC = npc;
                }
                else if (Vector2.Distance(Projectile.Center, Main.npc[i].Center) < Vector2.Distance(Projectile.Center, closestNPC.Center))
                {
                    closestNPC = npc;
                }
            }
            if (Vector2.Distance(Projectile.Center, closestNPC.Center) < range)
            {
                return closestNPC;
            }
            else
            {
                return null;
            }
        }
    }
}
