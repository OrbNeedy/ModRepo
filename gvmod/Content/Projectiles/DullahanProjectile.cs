using gvmod.Common.Players.Septimas;
using gvmod.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using gvmod.Common.GlobalNPCs;
using Terraria.DataStructures;

namespace gvmod.Content.Projectiles
{
    internal class DullahanProjectile : ModProjectile
    {
        public int septimaSource = 0;
        private List<int> npcsTaggedByThis;
        private List<int> playersTaggedByThis;

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.scale = 1f;
            Projectile.light = 0.25f;
            Main.projFrames[Projectile.type] = 3;

            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.damage = 30;
            Projectile.knockBack = 2;

            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 150;
            Projectile.ownerHitCheck = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            npcsTaggedByThis = new List<int>();

            if (Projectile.ai[0] % 2 == 1)
            {
                if (Projectile.ai[1] == 1 || Projectile.ai[1] == 3)
                {
                    Projectile.penetrate = 5;
                }
                else
                {
                    Projectile.penetrate = -1;
                }
            }

            // For color
            switch (Projectile.ai[0])
            {
                case 2:
                    break;
                case 4:
                    break;
                case 6:
                    break;
                default:
                    break;
            }
        }

        public override void AI()
        {
            if (++Projectile.frameCounter >= 10)
            {
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
                Projectile.frameCounter = 0;
            }

            NPC target = FindClosestNPC(200);

            if (target != null)
            {
                switch (Projectile.ai[1])
                {
                    case 1:
                        // Mizuchi homing
                        Projectile.velocity += Projectile.Center.DirectionTo(target.Center)*0.5f;
                        break;
                    case 3:
                        // Improved Mizuchi homing and vasuki effect
                        if (!target.GetGlobalNPC<TaggedNPC>().ContainsVasukiDart(Projectile.whoAmI))
                        {
                            Projectile.velocity += Projectile.Center.DirectionTo(target.Center) * 0.7f;
                        }
                        break;
                }
                Projectile.velocity.X = MathHelper.Clamp(Projectile.velocity.X, -16, 16);
                Projectile.velocity.Y = MathHelper.Clamp(Projectile.velocity.Y, -16, 16);
            }
            base.AI();
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            Septima septima = Main.player[Projectile.owner].GetModPlayer<AdeptPlayer>().Septima;

            if ((Projectile.ai[1] == 2 || Projectile.ai[1] == 3) && !target.immortal && !target.friendly)
            {
                TaggedNPC temp = target.GetGlobalNPC<TaggedNPC>();
                if (temp.VasukiTimer == 0 && !temp.VasukiShoot)
                {
                    temp.NextVasukiPlayerSource = Projectile.owner;
                    temp.VasukiDartParams[0] = Projectile.ai[0];
                    temp.VasukiDartParams[1] = Projectile.ai[1];
                    temp.VasukiIsAlsoDullahan = true; // This is set to false in the normal dart
                    temp.VasukiTimer = 60;
                    temp.VasukiShoot = true;
                }
            }

            if (septima is AzureStriker || septima is AzureThunderclap)
            {
                foreach (int tag in npcsTaggedByThis)
                {
                    if (tag == target.whoAmI)
                    {
                        return;
                    }
                }
                if (septima is AzureStriker striker) striker.AddTaggedNPC(target);
                if (septima is AzureThunderclap thunderclap) thunderclap.AddTaggedNPC(target);
                AddTaggedNPC(target);
                return;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            base.OnHitPlayer(target, info);
            Septima septima = Main.player[Projectile.owner].GetModPlayer<AdeptPlayer>().Septima;

            if (septima is AzureStriker || septima is AzureThunderclap)
            {
                foreach (int tag in npcsTaggedByThis)
                {
                    if (tag == target.whoAmI)
                    {
                        return;
                    }
                }
                if (septima is AzureStriker striker) striker.AddTaggedPlayer(target);
                if (septima is AzureThunderclap thunderclap) thunderclap.AddTaggedPlayer(target);
                AddTaggedPlayer(target);
                return;
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

        public void AddTaggedNPC(NPC target)
        {
            foreach (int tag in npcsTaggedByThis)
            {
                if (target.whoAmI == tag)
                {
                    return;
                }
            }
            npcsTaggedByThis.Add(target.whoAmI);
        }

        public void AddTaggedPlayer(Player target)
        {
            foreach (int tag in playersTaggedByThis)
            {
                if (target.whoAmI == tag)
                {
                    return;
                }
            }
            playersTaggedByThis.Add(target.whoAmI);
        }
    }
}
