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
    internal class AzureThunderclap : Septima
    {
        private int secondaryDuration = 5;
        private List<Mark> markedNPCs = new List<Mark>();

        public AzureThunderclap(AdeptPlayer adept, Player player) : base(adept, player)
        {
            SpUsage = 0.5f;
            SecondaryCooldownTime = 600;
        }

        public override string Name => "Azure Thunderclap";

        public override void InitializeAbilitiesList()
        {
            Abilities.Add(new Astrasphere(Player, Adept));
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
                SpUsage = 0.5f;
            }
            Vector2 pos = new Vector2(128);
            for (int i = 0; i < 360; i++)
            {
                pos = pos.RotatedBy(MathHelper.ToRadians(1));
                if (i % 5 == 0) Dust.NewDustDirect(Player.Center + pos, 10, 10, DustID.MartianSaucerSpark, 0, 0, 0, Color.DeepSkyBlue);
            }
        }

        public override void FirstAbility()
        {
            List<NPC> closeNPCs = GetNPCsInRadius(176);
            foreach (NPC npc in closeNPCs)
            {
                npc.AddBuff(ModContent.BuffType<ThunderclapElectrifiedDebuff>(), 10);
            }

            foreach (Mark mark in markedNPCs)
            {
                //Make a projectile for this
                for (int i = 0; i < mark.level; i++)
                {
                    Projectile.NewProjectile(Player.GetSource_FromThis(), mark.npc.Center, new Vector2(0), ModContent.ProjectileType<ElectricSphere>(), (int)(10 * Adept.primaryDamageLevelMult * Adept.primaryDamageEquipMult), 8, Player.whoAmI);
                }
            }
        }

        public override void SecondAbilityEffects()
        {
            if (SecondaryTimer <= 5 && Adept.secondaryInUse)
            {
                for (int i = 0; i < 7; i++)
                {
                    Dust.NewDust(new Vector2(Player.Center.X, Player.position.Y + Player.height), 10, 10, DustID.MartianSaucerSpark, 46);
                    Dust.NewDust(new Vector2(Player.Center.X, Player.position.Y + Player.height), 10, 10, DustID.MartianSaucerSpark, -46);
                }
            }
        }

        public override void SecondAbility()
        {
            if (SecondaryTimer <= 1 && Adept.secondaryInUse)
            {
                Projectile.NewProjectile(Player.GetSource_FromThis(), new Vector2(Player.Center.X, Player.position.Y + Player.height), new Vector2(10, 0), ModContent.ProjectileType<LightningCreeper>(), (int)(35 * Adept.secondaryDamageLevelMult * Adept.secondaryDamageEquipMult), 10, Player.whoAmI);
                Projectile.NewProjectile(Player.GetSource_FromThis(), new Vector2(Player.Center.X, Player.position.Y + Player.height), new Vector2(-10, 0), ModContent.ProjectileType<LightningCreeper>(), (int)(35 * Adept.secondaryDamageLevelMult * Adept.secondaryDamageEquipMult), 10, Player.whoAmI);
            }
        }

        public override void MiscEffects()
        {
            if (Adept.isOverheated)
            {
                VelocityMultiplier *= 0f;
            } else
            {
                VelocityMultiplier = new Vector2(1f, 1f);
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
                    }
                    else
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
            for (int i = 0; i < markedNPCs.Count; i++)
            {
                markedNPCs[i].Update();
                if (!markedNPCs[i].active)
                {
                    markedNPCs.Remove(markedNPCs[i]);
                    continue;
                }
                markedNPCs[i].timer++;
            }
        }

        public void AddMarkedNPC(NPC target)
        {
            foreach (Mark mark in markedNPCs)
            {
                if (target == mark.npc && target.active)
                {
                    mark.IncreaseMark();
                    return;
                }
            }
            markedNPCs.Add(new Mark(target));
        }
    }
}
