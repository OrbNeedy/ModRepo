using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using gvmod.Content.Projectiles;

namespace gvmod.Common.GlobalNPCs
{
    public class TaggedNPC : GlobalNPC
    {
        private Asset<Texture2D> shock = ModContent.Request<Texture2D>("gvmod/Content/Projectiles/Shock");
        public int TagLevel { get; set; }
        public bool Shocked { get; set; }
        private int rotation;

        public int NextVasukiPlayerSource { get; set; }
        public float[] VasukiDartParams { get; set; }
        public bool VasukiIsAlsoDullahan { get; set; }
        public bool VasukiShoot { get; set; }
        public List<int> VasukiTags { get; set; }
        public int VasukiTimer { get; set; }

        public override bool InstancePerEntity => true;

        public override void SetDefaults(NPC npc)
        {
            VasukiDartParams = new float[2];
            VasukiTags = new List<int>();
            VasukiTimer = 0;
        }

        public override void AI(NPC npc)
        {
            if (VasukiTimer > 0) VasukiTimer--;
            if (VasukiTimer == 0 && VasukiShoot)
            {
                NPC target = FindClosestNPC(800, npc);
                if (target != null)
                {
                    if (VasukiIsAlsoDullahan)
                    {
                        VasukiTags.Add(Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Center, npc.DirectionTo(target.Center) * 16, ModContent.ProjectileType<DullahanProjectile>(), 60, 0, NextVasukiPlayerSource, VasukiDartParams[0], VasukiDartParams[1]));
                    } else
                    {
                        VasukiTags.Add(Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Center, npc.DirectionTo(target.Center) * 16, ModContent.ProjectileType<HairDartProjectile>(), 2, 0, NextVasukiPlayerSource, VasukiDartParams[0], VasukiDartParams[1]));
                    }
                    VasukiShoot = false;
                }
            }

            for (int i = 0; i < VasukiTags.Count; i++)
            {
                if (!Main.projectile[VasukiTags[i]].active || Main.projectile[VasukiTags[i]].timeLeft <= 0)
                {
                    VasukiTags.Remove(VasukiTags[i]);
                }
            }
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (Shocked)
            {
                var position = npc.Center - Main.screenPosition + shock.Size() / 2;
                position = new Vector2((int)position.X, (int)position.Y);
                spriteBatch.Draw(
                    shock.Value,
                    position,
                    null,
                    Color.White * 0.8f,
                    0f,
                    shock.Size(),
                    1f,
                    SpriteEffects.None,
                    0
                );
            }

            if (TagLevel > 0)
            {
                if (TagLevel == 2)
                {
                    Rotate();
                } else if (TagLevel == 3)
                {
                    ReverseRotate();
                } else {
                    rotation = 0;
                }
                Asset<Texture2D> tag = ModContent.Request<Texture2D>("gvmod/Assets/Effects/Tag" + TagLevel);
                var position = npc.Center - Main.screenPosition;
                position = new Vector2((int)position.X, (int)position.Y);
                spriteBatch.Draw(
                    tag.Value,
                    position,
                    null,
                    Color.White * 0.5f,
                    MathHelper.ToRadians(rotation),
                    tag.Size()*0.5f,
                    1f,
                    SpriteEffects.None,
                    0
                );
                Asset<Texture2D> mark = ModContent.Request<Texture2D>("gvmod/Assets/Effects/TagMark" + TagLevel);
                position = new Vector2((int)position.X, (int)position.Y);
                spriteBatch.Draw(
                    mark.Value,
                    position,
                    null,
                    Color.White * 0.5f,
                    0f,
                    tag.Size() * 0.5f,
                    1f,
                    SpriteEffects.None,
                    0
                );
            }
        }

        public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
        {
            if ((projectile.ModProjectile is HairDartProjectile || projectile.ModProjectile is DullahanProjectile) && ContainsVasukiDart(projectile.whoAmI)) return false;
            return base.CanBeHitByProjectile(npc, projectile);
        }

        private void Rotate()
        {
            rotation++;
            if (rotation >= 361)
            {
                rotation = 0;
            }
        }

        private void ReverseRotate()
        {
            rotation -= 2;
            if (rotation <= -361)
            {
                rotation = 0;
            }
        }

        public NPC FindClosestNPC(int range, NPC source)
        {
            NPC closestNPC = null;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                var npc = Main.npc[i];
                if (!npc.active && npc.life <= 0 || source.whoAmI == npc.whoAmI) continue;
                if (closestNPC == null)
                {
                    closestNPC = npc;
                }
                else if (Vector2.Distance(source.Center, Main.npc[i].Center) < Vector2.Distance(source.Center, closestNPC.Center))
                {
                    closestNPC = npc;
                }
            }
            if (Vector2.Distance(source.Center, closestNPC.Center) < range)
            {
                return closestNPC;
            }
            else
            {
                return null;
            }
        }

        public bool ContainsVasukiDart(int index)
        {
            for (int i = 0; i < VasukiTags.Count; i++)
            {
                if (VasukiTags[i] == index)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
