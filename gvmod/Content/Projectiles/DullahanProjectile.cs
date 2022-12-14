using gvmod.Common.Players.Septimas;
using gvmod.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using gvmod.Common.Configs.CustomDataTypes;
using gvmod.Common.GlobalNPCs;
using Terraria.DataStructures;

namespace gvmod.Content.Projectiles
{
    internal class DullahanProjectile : ModProjectile
    {
        public int septimaSource = 0;
        private List<int> npcsTaggedByThis;
        private float speedLimit;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dullahan bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.scale = 1f;
            Projectile.light = 0.25f;

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
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                default:
                    break;
            }
        }

        public override void AI()
        {
            NPC target = FindClosestNPC(200);
            switch (Projectile.ai[1])
            {
                case 1:
                    // Mizuchi homing
                    if (target != null && target.active && target.life > 0 && !target.immortal && !target.friendly)
                    {
                        if (Projectile.Center.X < target.Center.X && Projectile.velocity.X < 16)
                        {
                            Projectile.velocity.X += 0.4f;
                        }
                        else if (Projectile.Center.X > target.Center.X && Projectile.velocity.X > -16)
                        {
                            Projectile.velocity.X -= 0.4f;
                        }
                        if (Projectile.Center.Y < target.Center.Y && Projectile.velocity.Y < 16)
                        {
                            Projectile.velocity.Y += 0.4f;
                        }
                        else if (Projectile.Center.Y > target.Center.Y && Projectile.velocity.Y > -16)
                        {
                            Projectile.velocity.Y -= 0.4f;
                        }
                    }
                    break;
                case 3:
                    // Improved Mizuchi homing and vasuki effect
                    if (target != null && target.active && target.life > 0 && !target.immortal && !target.friendly && !target.GetGlobalNPC<TaggedNPC>().ContainsVasukiDart(Projectile.whoAmI))
                    {
                        if (Projectile.Center.X < target.Center.X && Projectile.velocity.X < 16)
                        {
                            Projectile.velocity.X += 0.7f;
                        }
                        else if (Projectile.Center.X > target.Center.X && Projectile.velocity.X > -16)
                        {
                            Projectile.velocity.X -= 0.7f;
                        }
                        if (Projectile.Center.Y < target.Center.Y && Projectile.velocity.Y < 16)
                        {
                            Projectile.velocity.Y += 0.7f;
                        }
                        else if (Projectile.Center.Y > target.Center.Y && Projectile.velocity.Y > -16)
                        {
                            Projectile.velocity.Y -= 0.7f;
                        }
                    }
                    break;
                default:
                    break;
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
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

            if (septima is AzureStriker striker)
            {
                foreach (int tag in npcsTaggedByThis)
                {
                    if (tag == target.whoAmI)
                    {
                        return;
                    }
                }
                striker.AddTaggedNPC(target);
                AddTaggedNPC(target);
                return;
            }
            if (septima is AzureThunderclap thunderclap)
            {
                foreach (int tag in npcsTaggedByThis)
                {
                    if (tag == target.whoAmI)
                    {
                        return;
                    }
                }
                thunderclap.AddTaggedNPC(target);
                AddTaggedNPC(target);
                return;
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
                } else if (Vector2.Distance(Projectile.Center, Main.npc[i].Center) < Vector2.Distance(Projectile.Center, closestNPC.Center))
                {
                    closestNPC = npc;
                }
            }
            if (Vector2.Distance(Projectile.Center, closestNPC.Center) < range)
            {
                return closestNPC;
            } else
            {
                return null;
            }
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
    }
}
