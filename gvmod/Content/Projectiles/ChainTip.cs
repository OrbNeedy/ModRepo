using gvmod.Common.GlobalNPCs;
using gvmod.Common.Players;
using gvmod.Common.Players.Septimas;
using gvmod.Content.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Projectiles
{
    public class ChainTip: ModProjectile
    {
        private int aiState = 1;
        private bool electrify = false;
        private Vector2 startingPosition = new Vector2(0, 0);
        private int desynchTime = Main.rand.Next(20, 60);
        private Dictionary<int, int> trappedNPCs = new Dictionary<int, int>();
        private Dictionary<int, int> entrapmentPotency = new Dictionary<int, int>();
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Voltaic chain");
        }

        public override void SetDefaults()
        {
            Projectile.light = 1f;
            Projectile.ignoreWater = true;
            Projectile.damage = 50;
            Projectile.knockBack = 0;
            Projectile.Size = new Vector2(42);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.timeLeft = 600;
            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.ownerHitCheck = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            startingPosition = Projectile.Center;
            if (Projectile.ai[1] == 1)
            {
                Projectile.timeLeft = 300;
            }
            base.OnSpawn(source);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.ai[1] == 1)
            {
                target.AddBuff(ModContent.BuffType<Chained>(), Projectile.timeLeft);
            } else
            {
                target.GetGlobalNPC<ChainedNPC>().ChainedTime = Main.projectile[(int)Projectile.ai[0]].timeLeft;
            }
            foreach (int index in trappedNPCs.Keys)
            {
                if (index == target.whoAmI)
                {
                    entrapmentPotency[index]++;
                    return;
                }
            }
            trappedNPCs.Add(target.whoAmI, 0);
            entrapmentPotency.Add(target.whoAmI, 0);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            AdeptPlayer adept = Main.player[Projectile.owner].GetModPlayer<AdeptPlayer>();
            if (Projectile.ai[1] != 1)
            {

                if (Projectile.timeLeft <= 580)
                {
                    Projectile.velocity *= 0.25f;
                }
                if (Main.projectile[(int)Projectile.ai[0]].timeLeft <= 70)
                {
                    aiState = 2;
                }
                if (aiState == 2 && !electrify)
                {
                    Projectile.timeLeft = desynchTime;
                    electrify = true;
                }
            }
            if (Projectile.ai[1] == 1)
            {
                if (Projectile.timeLeft <= 280)
                {
                    Projectile.velocity *= 0.25f;
                }
                if (Projectile.timeLeft == 70)
                {
                    electrify = true;
                    aiState = 2;
                }
            }
            if (Projectile.timeLeft <= 70 && electrify)
            {
                foreach (int index in trappedNPCs.Keys)
                {
                    NPC theNpcInQuestion = Main.npc[index];
                    if (theNpcInQuestion.active && theNpcInQuestion.life > 0)
                    {
                        theNpcInQuestion.AddBuff(ModContent.BuffType<VoltaicElectrocution>(), 10);
                        if (trappedNPCs[index] <= 0)
                        {
                            theNpcInQuestion.StrikeNPC((int)(100 * adept.SpecialDamageLevelMult * adept.SpecialDamageEquipMult * (1 + (trappedNPCs.Count * 0.5)) * (1 + (entrapmentPotency[index] * 0.5))), 0, 0);
                            trappedNPCs[index] = 6;
                        }
                    }
                }
            }
            foreach (int index in trappedNPCs.Keys)
            {
                trappedNPCs[index]--;
            }
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

        public override bool PreDrawExtras()
        {
            Asset<Texture2D> chainTexture = ModContent.Request<Texture2D>("gvmod/Content/Projectiles/ChainTipChain");
            Vector2 origin = startingPosition;
            Vector2 center = Projectile.Center;
            Vector2 directionToOrigin = origin - Projectile.Center;
            float chainRotation = directionToOrigin.ToRotation() - MathHelper.PiOver2;
            float distanceToOrigin = directionToOrigin.Length();

            while (distanceToOrigin > 128f && !float.IsNaN(distanceToOrigin))
            {
                directionToOrigin /= distanceToOrigin;
                directionToOrigin *= chainTexture.Height();

                center += directionToOrigin;
                directionToOrigin = origin - center;
                distanceToOrigin = directionToOrigin.Length();

                Color drawColor = Color.White;

                Main.EntitySpriteDraw(chainTexture.Value, center - Main.screenPosition,
                    chainTexture.Value.Bounds, drawColor, chainRotation,
                    chainTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0);
            }
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Vector2 vector = startingPosition - Projectile.Center;
            int magnitude = (int)Math.Sqrt(Math.Pow(startingPosition.X - Projectile.Center.X, 2) + Math.Pow(startingPosition.Y - Projectile.Center.Y, 2));
            for (int i = 1; i <= magnitude + 1; i++)
            {
                Vector2 unitVector = vector.SafeNormalize(Vector2.Zero);
                for (int k = 0; k <= 5; k++)
                {
                    Dust.NewDust((unitVector * magnitude) + new Vector2(Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-6, 6)), 10, 10, DustID.BlueTorch);
                }
            }
            base.Kill(timeLeft);
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(startingPosition);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            startingPosition = reader.ReadVector2();
        }
    }
}
