﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using gvmod.Content.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using gvmod.Common.Players.Septimas.Abilities;
using gvmod.Common.Configs.CustomDataTypes;
using gvmod.Common.GlobalNPCs;

namespace gvmod.Common.Players.Septimas
{
    internal class AzureStriker : Septima
    {
        private int secondaryDuration = 15;
        private bool isFalling = false;
        private List<Tag> taggedNPCs = new List<Tag>();
        private int flashfieldIndex;
        private bool flashfieldExists = false;
        public AzureStriker(AdeptPlayer adept, Player player) : base(adept, player)
        {
            SpUsage = 1f;
            SecondaryCooldownTime = 300;
        }

        public override string Name => "Azure Striker";

        public override bool CanRecharge => true;

        public override void InitializeAbilitiesList()
        {
            Abilities.Add(new None(Player, Adept));
            Abilities.Add(new Astrasphere(Player, Adept));
            Abilities.Add(new GalvanicPatch(Player, Adept));
            Abilities.Add(new Sparkcaliburg(Player, Adept));
            Abilities.Add(new GalvanicRenewal(Player, Adept));
            Abilities.Add(new VoltaicChains(Player, Adept));
            Abilities.Add(new GloriousStrizer(Player, Adept));
            Abilities.Add(new SeptimalSurge(Player, Adept));
        }

        public override void FirstAbilityEffects()
        {
            if (Player.wet)
            {
                SpUsage = Adept.MaxSeptimalPower * 10;
                return;
            }
            else
            {
                SpUsage = 1f;
            }
        }

        public override void FirstAbility()
        {
            if (!flashfieldExists)
            {
                flashfieldIndex = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(0f, 0f), ModContent.ProjectileType<FlashfieldStriker>(), (int)(2 * Adept.PrimaryDamageLevelMult * Adept.PrimaryDamageEquipMult), 0, Player.whoAmI, -1, 0);
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
                NPC theNpcInQuestion = Main.npc[tag.NpcIndex];
                float tagMultiplier = (float)((tag.Level * 0.75));
                if (tag.ShockIframes == 0)
                {
                    theNpcInQuestion.StrikeNPC((int)(15 * Adept.PrimaryDamageLevelMult * Adept.PrimaryDamageEquipMult * tagMultiplier), 0, Player.direction);
                    tag.ShockIframes = 6;
                }
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
            if (SecondaryTimer <= 1 && Adept.SecondaryInUse)
            {
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(0), ModContent.ProjectileType<Thunder>(), (int)(50 * Adept.SecondaryDamageLevelMult * Adept.SecondaryDamageEquipMult), 8, Player.whoAmI);
            }
        }

        public override void MiscEffects()
        {
            if (isFalling && Adept.IsUsingPrimaryAbility && !Adept.IsOverheated)
            {
                VelocityMultiplier = new Vector2(1f, 0.7f);
                Player.slowFall = true;
            }
            else
            {
                if (Adept.SecondaryInUse)
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
            UpdateTaggedNPCs();
            if (Adept.IsUsingSecondaryAbility && Adept.TimeSinceSecondary >= SecondaryCooldownTime)
            {
                SecondaryTimer++;
                if (SecondaryTimer >= secondaryDuration)
                {
                    Adept.SecondaryInUse = false;
                    SecondaryTimer = 0;
                    Adept.SecondaryInCooldown = true;
                    Adept.TimeSinceSecondary = 0;
                    Adept.IsUsingSecondaryAbility = false;
                }
            }

            Projectile flashfield = Main.projectile[flashfieldIndex];
            if (flashfield.active && flashfield.ModProjectile is FlashfieldStriker)
            {
                flashfieldExists = true;
            } else
            {
                flashfieldExists = false;
            }

            Player.velocity *= VelocityMultiplier;
        }

        public void UpdateTaggedNPCs()
        {
            for (int i = 0; i < taggedNPCs.Count; i++)
            {
                Tag theTagInQuestion = taggedNPCs[i];
                TaggedNPC globalTag = Main.npc[theTagInQuestion.NpcIndex].GetGlobalNPC<TaggedNPC>();
                theTagInQuestion.Update();

                if (!theTagInQuestion.Active)
                {
                    globalTag.Shocked = false;
                    taggedNPCs.Remove(theTagInQuestion);
                }
                globalTag.TagLevel = theTagInQuestion.Level;

                if (theTagInQuestion.Active && Adept.IsUsingPrimaryAbility && Adept.CanUsePrimary)
                {
                    globalTag.Shocked = true;
                } else
                {
                    globalTag.Shocked = false;
                }
            }
        }

        public void AddTaggedNPC(NPC target)
        {
            foreach (Tag tag in taggedNPCs)
            {
                NPC theNpcInQuestion = Main.npc[tag.NpcIndex];
                if (target == theNpcInQuestion && tag.Active)
                {
                    tag.IncreaseTag();
                    return;
                }
            }
            taggedNPCs.Add(new Tag(target.whoAmI));
        }
    }
}
