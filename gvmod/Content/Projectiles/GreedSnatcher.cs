using gvmod.Common.Players;
using gvmod.Content.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Content.Projectiles
{
    public class GreedSnatcher : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.scale = 1f;
            Projectile.light = 0.7f;
            Main.projFrames[Projectile.type] = 3;

            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.damage = 45;
            Projectile.knockBack = 18;

            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 150;
            Projectile.ownerHitCheck = false;
        }

        public override void AI()
        {
            if (++Projectile.frameCounter >= 20)
            {
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
                Projectile.frameCounter = 0;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            // Also check if the septima is not Carrera's, when the septima is added
            AdeptPlayer adept = target.GetModPlayer<AdeptPlayer>();
            if (adept.Septima != null)
            {
                adept.overheat(0);
                target.AddBuff(ModContent.BuffType<Chaff>(), 300);
            }
            base.OnHitPlayer(target, info);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Chaff>(), 600);
            base.OnHitNPC(target, hit, damageDone);
        }
    }
}
