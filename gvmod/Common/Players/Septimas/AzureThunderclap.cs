using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using gvmod.Content.Buffs;
using gvmod.Content.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using gvmod.Common.Players.Septimas.Abilities;
using gvmod.Common.Configs.CustomDataTypes;
using gvmod.Common.GlobalNPCs;

namespace gvmod.Common.Players.Septimas
{
    internal class AzureThunderclap : Septima
    {
        private int secondaryDuration = 5;
        private List<Tag> taggedNPCs = new List<Tag>();
        private int flashfieldIndex;
        private bool flashfieldExists = false;

        public AzureThunderclap(AdeptPlayer adept, Player player) : base(adept, player)
        {
            SpUsage = 0.5f;
            SecondaryCooldownTime = 600;
        }

        public override string Name => "Azure Thunderclap";

        public override bool CanRecharge => true;

        public override void InitializeAbilitiesList()
        {
            Abilities.Add(new None(Player, Adept));
            Abilities.Add(new Astrasphere(Player, Adept));
            Abilities.Add(new Sparkcaliburg(Player, Adept));
            Abilities.Add(new VoltaicChains(Player, Adept));
            Abilities.Add(new SeptimalSurge(Player, Adept));
        }

        public override void FirstAbilityEffects()
        {
            if (Player.wet)
            {
                SpUsage = Adept.MaxSeptimalPower;
                return;
            }
            else
            {
                SpUsage = 0.5f;
            }
        }

        public override void FirstAbility()
        {
            if (!flashfieldExists)
            {
                flashfieldIndex = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(0f, 0f), ModContent.ProjectileType<FlashfieldStriker>(), (int)(2 * Adept.primaryDamageLevelMult * Adept.primaryDamageEquipMult), 0, Player.whoAmI, -1, 0);
            }
            foreach (Tag tag in taggedNPCs)
            {
                NPC theNpcInQuestion = Main.npc[tag.npcIndex];
                float tagMultiplier = (float)((tag.level * 0.75) + 0.25);
                if (tag.shockIframes == 0)
                {
                    theNpcInQuestion.StrikeNPC((int)(20 * Adept.primaryDamageLevelMult * Adept.primaryDamageEquipMult * tagMultiplier), 0, Player.direction);
                    tag.shockIframes = 6;
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

            Projectile flashfield = Main.projectile[flashfieldIndex];
            if (flashfield.active && flashfield.ModProjectile is FlashfieldStriker)
            {
                flashfieldExists = true;
            }
            else
            {
                flashfieldExists = false;
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
            for (int i = 0; i < taggedNPCs.Count; i++)
            {
                Tag theTagInQuestion = taggedNPCs[i];
                TaggedNPC globalTag = Main.npc[theTagInQuestion.npcIndex].GetGlobalNPC<TaggedNPC>();
                theTagInQuestion.Update();

                if (!theTagInQuestion.active)
                {
                    globalTag.shocked = false;
                    taggedNPCs.Remove(theTagInQuestion);
                }
                globalTag.tagLevel = theTagInQuestion.level;

                if (theTagInQuestion.active && Adept.isUsingPrimaryAbility && Adept.canUsePrimary)
                {
                    globalTag.shocked = true;
                } else
                {
                    globalTag.shocked = false;
                }
            }
        }

        public void AddMarkedNPC(NPC target)
        {
            foreach (Tag tag in taggedNPCs)
            {
                NPC theNpcInQuestion = Main.npc[tag.npcIndex];
                if (target == theNpcInQuestion && tag.active)
                {
                    tag.IncreaseTag();
                    return;
                }
            }
            taggedNPCs.Add(new Tag(target.whoAmI));
        }
    }
}
