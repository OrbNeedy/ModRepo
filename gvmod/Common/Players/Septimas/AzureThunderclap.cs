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
using Terraria.DataStructures;

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
            SecondaryCooldownTime = 600;
            SpBaseUsage = 0.5f;
            SpBaseRegen = 2.5f;
            SpBaseOverheatRegen = 1f;
            ApBaseRegen = (1f / 3600f);
        }

        public override string Name => "Azure Thunderclap";

        public override bool CanRecharge => true;

        public override Color ClearColor => new Color(77, 175, 232);
        public override Color MainColor => new Color(44, 143, 205);
        public override Color DarkColor => new Color(12, 112, 179);

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

        public override void OnOverheat()
        {
        }

        public override void OnRecovery()
        {
            SpBaseUsage = 0.5f;
        }

        public override void DuringOverheat()
        {
            Player.controlJump = false;
            Player.moveSpeed *= 0.2f;
        }

        public override void FirstAbilityEffects()
        {
            if (Player.wet)
            {
                SpBaseUsage = Adept.MaxSeptimalPower;
                return;
            }
            else
            {
                SpBaseUsage = 0.5f;
            }
        }

        public override void FirstAbility()
        {
            if (!flashfieldExists)
            {
                flashfieldIndex = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(0f, 0f), ModContent.ProjectileType<FlashfieldStriker>(), (int)(15 * Adept.PrimaryDamageLevelMult * Adept.PrimaryDamageEquipMult), 12, Player.whoAmI, -1, 0);
            }
            foreach (Tag tag in taggedNPCs)
            {
                NPC theNpcInQuestion = Main.npc[tag.NpcIndex];
                float tagMultiplier = (float)((tag.Level * 0.75) + 0.25);
                if (tag.ShockIframes == 0)
                {
                    theNpcInQuestion.StrikeNPC((int)(20 * Adept.PrimaryDamageLevelMult * Adept.PrimaryDamageEquipMult * tagMultiplier), 0, Player.direction);
                    tag.ShockIframes = 6;
                }
            }
        }

        public override void SecondAbilityEffects()
        {
            if (SecondaryTimer <= 5 && Adept.SecondaryInUse)
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
            if (SecondaryTimer <= 1 && Adept.SecondaryInUse)
            {
                Projectile.NewProjectile(Player.GetSource_FromThis(), new Vector2(Player.Center.X, Player.position.Y + Player.height), new Vector2(10, 0), ModContent.ProjectileType<LightningCreeper>(), (int)(35 * Adept.SecondaryDamageLevelMult * Adept.SecondaryDamageEquipMult), 10, Player.whoAmI);
                Projectile.NewProjectile(Player.GetSource_FromThis(), new Vector2(Player.Center.X, Player.position.Y + Player.height), new Vector2(-10, 0), ModContent.ProjectileType<LightningCreeper>(), (int)(35 * Adept.SecondaryDamageLevelMult * Adept.SecondaryDamageEquipMult), 10, Player.whoAmI);
            }
        }

        public override bool OnPrevasion(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            if (!Adept.IsOverheated && Adept.AnthemLevel >= 1 && Main.CalculateDamagePlayersTake(damage, Player.statDefense) <= ((Player.statLifeMax + Player.statLifeMax2)*2 / 5))
            {
                Main.NewText("Prevasion");
                Adept.TimeSincePrimary = 0;
                Adept.IsRecharging = false;
                Adept.RechargeTimer = 0;
                Player.immune = true;
                Player.AddImmuneTime(cooldownCounter, 60);
                return true;
            }
            return false;
        }

        public override void MiscEffects()
        {
        }

        public override void Updates()
        {
            UpdateMarkedNPCs();
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
            }
            else
            {
                flashfieldExists = false;
            }

            Player.velocity *= VelocityMultiplier;
        }

        public void UpdateMarkedNPCs()
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
