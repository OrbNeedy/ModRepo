using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using gvmod.Content.Buffs;
using gvmod.Content.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using gvmod.Common.Players.Septimas.Abilities;
using gvmod.Common.Configs.CustomDataTypes;

namespace gvmod.Common.Players.Septimas
{
    internal class AzureStriker : Septima
    {
        private int secondaryDuration = 15;
        private bool isFalling = false;
        private List<Tag> taggedNPCs = new List<Tag>();
        public AzureStriker(AdeptPlayer adept, Player player) : base(adept, player)
        {
            SpUsage = 1f;
            SecondaryCooldownTime = 300;
        }

        public override string Name => "Azure Striker";

        public override void InitializeAbilitiesList()
        {
            Abilities.Add(new Astrasphere(Player, Adept));
            Abilities.Add(new Sparkcaliburg(Player, Adept));
            Abilities.Add(new VoltaicChains(Player, Adept));
        }

        public override void FirstAbilityEffects()
        {
            if (Player.wet)
            {
                SpUsage = Adept.maxSeptimalPower;
                return;
            }
            else
            {
                SpUsage = 1f;
            }
            Vector2 pos = new Vector2(128);
            pos = pos.RotatedByRandom(360);
            for (int i = 0; i < 360; i++)
            {
                pos = pos.RotatedBy(MathHelper.ToRadians(1));
                if (i % 5 == 0) Dust.NewDustDirect(Player.Center + pos, 10, 10, DustID.MartianSaucerSpark, 0, 0, 0, Color.DeepSkyBlue);
            }
        }

        public override void FirstAbility()
        {
            //TODO: Figure out a way of making this without having to change collision to be circular
            //Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, new Vector2(0), ModContent.ProjectileType<FlashfieldStriker>(), 4, 0, player.whoAmI);
            List<NPC> closeNPCs = GetNPCsInRadius(176);
            foreach (NPC npc in closeNPCs)
            {
                if (!npc.friendly)
                {
                    npc.AddBuff(ModContent.BuffType<StrikerElectrifiedDebuff>(), 10);
                }
            }
            if (Player.velocity.Y > 0)
            {
                isFalling = true;
                Player.slowFall = true;
            } else
            {
                isFalling = false;
                Player.slowFall = false;
            }
            foreach (Tag tag in taggedNPCs)
            {
                NPC theNpcInQuestion = Main.npc[tag.npcIndex];
                float tagMultiplier = (float)((tag.level * 0.75)+0.25);
                Projectile.NewProjectile(Player.GetSource_FromThis(), theNpcInQuestion.Center, new Vector2(0), ModContent.ProjectileType<Shock>(), (int)(10 * Adept.primaryDamageLevelMult * Adept.primaryDamageEquipMult * tagMultiplier), 0, Player.whoAmI);
            }
        }

        public override void SecondAbilityEffects()
        {
            for (int i = 0; i < 20; i++)
            {
                float xPos = Main.rand.NextFloat(-16, 16);
                float yPos = Main.rand.NextFloat(-500, 500);
                Dust.NewDust(Player.Center + new Vector2(xPos, yPos), 10, 10, DustID.MartianSaucerSpark, 0, 0, 100, Color.DeepSkyBlue);
            }
        }

        public override void SecondAbility()
        {
            if (SecondaryTimer <= 1 && Adept.secondaryInUse)
            {
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(0), ModContent.ProjectileType<Thunder>(), (int)(50 * Adept.secondaryDamageLevelMult * Adept.secondaryDamageEquipMult), 8, Player.whoAmI);

            }
        }

        public override void MiscEffects()
        {
            if (isFalling && Adept.isUsingPrimaryAbility && !Adept.isOverheated)
            {
                VelocityMultiplier = new Vector2(1f, 0.7f);
                Player.slowFall = true;
            }
            else
            {
                if (Adept.secondaryInUse)
                {
                    Player.slowFall = true;
                    VelocityMultiplier *= 0f;
                } else
                {
                    VelocityMultiplier = new Vector2(1f);
                    Player.slowFall = false;
                }
            }
        }

        public override void Updates()
        {
            UpdateMarkedNPCs();
            if (Adept.isUsingSecondaryAbility && Adept.timeSinceSecondary >= SecondaryCooldownTime)
            {
                SecondaryTimer++;
                if (SecondaryTimer >= secondaryDuration)
                {
                    Adept.secondaryInUse = false;
                    SecondaryTimer = 0;
                    Adept.secondaryInCooldown = true;
                    Adept.timeSinceSecondary = 0;
                    Adept.isUsingSecondaryAbility = false;
                }
            }
            Player.velocity *= VelocityMultiplier;
        }

        public List<NPC> GetNPCsInRadius(int radius)
        {
            var closeNPCs = new List<NPC>();
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                var npc = Main.npc[i];
                List<Vector2> points = new List<Vector2>{npc.TopLeft, npc.Top, npc.TopRight, 
                    npc.Left, npc.Center, npc.Right, 
                    npc.BottomLeft, npc.Bottom, npc.BottomRight};
                if (!npc.active && npc.life <= 0) continue;
                for (int j = 0; j < points.Count; j++)
                {
                    if (Vector2.Distance(points[j], Player.Center) > radius)
                    {
                        continue;
                    } else
                    {
                        closeNPCs.Add(Main.npc[i]);
                        break;
                    };
                }
            }
            return closeNPCs;
        }

        public void UpdateMarkedNPCs()
        {
            for (int i = 0; i < taggedNPCs.Count; i++)
            {
                taggedNPCs[i].Update();
                if (!taggedNPCs[i].active)
                {
                    taggedNPCs.Remove(taggedNPCs[i]);
                }
            }
        }

        public void AddMarkedNPC(NPC target)
        {
            foreach (Tag mark in taggedNPCs)
            {
                NPC theNpcInQuestion = Main.npc[mark.npcIndex];
                if (target == theNpcInQuestion && mark.active)
                {
                    mark.IncreaseMark();
                    return;
                }
            }
            taggedNPCs.Add(new Tag(target.whoAmI));
        }
    }
}
