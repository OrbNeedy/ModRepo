using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using gvmod.Content.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using gvmod.Common.Players.Septimas.Skills;
using gvmod.Common.Configs.CustomDataTypes;
using gvmod.Common.GlobalNPCs;
using System;
using gvmod.Content;

namespace gvmod.Common.Players.Septimas
{
    public class AzureStriker : Septima
    {
        private bool crashing = false;
        private int crashMeter = 0;
        private int maxCrashMeter = 90;
        private int secondaryDuration = 15;
        private int visualProjectileTimer = 14;
        private bool isFalling = false;
        private List<Tag> taggedNPCs = new List<Tag>();
        private int flashfieldIndex;
        private bool flashfieldExists = false;
        public AzureStriker(AdeptPlayer adept, AdeptMuse muse, Player player) : base(adept, muse, player)
        {
            SecondaryCooldownTime = 300;
            SpBaseUsage = 1f;
            SpBaseRegen = 2f;
            SpBaseOverheatRegen = 1f;
            ApBaseRegen = (1f / 4020f);
            InitializeAbilitiesList();
        }

        public override string Name => "Azure Striker";

        public override bool CanRecharge => true;

        public override Color ClearColor => new Color(77, 232, 227);
        public override Color MainColor => new Color(44, 205, 195);
        public override Color DarkColor => new Color(12, 179, 173);

        public override Dictionary<int, float> ProjectileInteractions => new Dictionary<int, float>
        {
            [ProjectileID.MagnetSphereBolt] = 1,
            [ProjectileID.MartianTurretBolt] = 2,
            [ProjectileID.Electrosphere] = 1,
            [ProjectileID.ElectrosphereMissile] = 1,
            [ProjectileID.MagnetSphereBall] = 1,
            [ProjectileID.VortexLightning] = 1,
            [ProjectileID.VortexVortexLightning] = 1,
            [ProjectileID.VortexVortexPortal] = 1,
            [ProjectileID.CultistBossLightningOrb] = 0.5f,
            [ProjectileID.CultistBossLightningOrbArc] = 0.5f,
            [ProjectileID.DD2LightningBugZap] = 1.5f,
            [ModContent.ProjectileType<FlashfieldStriker>()] = -0.5f,
            [ModContent.ProjectileType<ElectricSphere>()] = -0.5f,
            [ModContent.ProjectileType<ElectricSword>()] = -0.5f,
            [ModContent.ProjectileType<GloriousSword>()] = -1f,
            [ModContent.ProjectileType<ChainTip>()] = -0.5f,
            [ModContent.ProjectileType<ChainMeteor>()] = -0.5f,
            [ModContent.ProjectileType<Thunder>()] = -0.5f,
            [ModContent.ProjectileType<ElectricBolt>()] = -0.5f,
            [ProjectileID.WaterBolt] = -1.5f,
            [ProjectileID.WaterStream] = -1,
            [ProjectileID.WaterGun] = -1,
            [ProjectileID.InfluxWaver] = -1
        };

        public override Dictionary<int, float> NPCInteractions => new Dictionary<int, float>
        {
            [NPCID.WaterSphere] = -1.5f,
            [NPCID.DetonatingBubble] = -1f
        };

        public override void InitializeAbilitiesList()
        {
            Abilities.Clear();
            Abilities.Add(new None(Player, Adept, "S"));
            Abilities.Add(new Astrasphere(Player, Adept, "S"));
            Abilities.Add(new GalvanicPatch(Player, Adept, "S"));
            Abilities.Add(new Sparkcaliburg(Player, Adept, "S"));
            Abilities.Add(new SplitSecond(Player, Adept, "S"));
            Abilities.Add(new GalvanicRenewal(Player, Adept, "S"));
            Abilities.Add(new VoltaicChains(Player, Adept, "S"));
            Abilities.Add(new GloriousStrizer(Player, Adept, "S"));
            Abilities.Add(new SeptimalSurge(Player, Adept, "S"));
            Abilities.Add(new SeptimalShield(Player, Adept, "S"));
            Abilities.Add(new InfiniteSurge(Player, Adept, "S"));
            Abilities.Add(new AlchemicalField(Player, Adept, "S"));
            Abilities.Sort(SortAbilities);
        }

        public override void OnOverheat()
        {
        }

        public override void OnRecovery()
        {
            MorbPlayer morb = Player.GetModPlayer<MorbPlayer>();
            SpBaseUsage = 1f;
            morb.AttackTimer = 22;
        }

        public override void DuringOverheat()
        {

        }

        public override void FirstAbilityEffects()
        {
            if (Player.wet)
            {
                Adept.overheat(60);
                return;
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
                float tagMultiplier = (float)((tag.Level * 0.75));
                if (tag.ShockIframes == 0)
                {
                    Player.ApplyDamageToNPC(Main.npc[tag.NpcIndex], (int)(15 * Adept.PrimaryDamageLevelMult * Adept.PrimaryDamageEquipMult * tagMultiplier), 0, Player.direction, damageType: ModContent.GetInstance<SeptimaDamageClass>(), damageVariation: true);
                    tag.ShockIframes = 6;
                }
            }

            FlashfieldCrashCheck();
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

        public override bool PrePrevasion(Player.HurtInfo info)
        {
            if (info.DamageSource.TryGetCausingEntity(out Entity entity))
            {
                if (entity is Projectile projectile)
                {
                    if (ProjectileInteractions.TryGetValue(projectile.type, out float interactions))
                    {
                        switch (interactions)
                        {
                            case < 0:
                                if (interactions == -1.5f)
                                {
                                    Adept.overheat(60);
                                }
                                return false;
                            case > 1:
                                if (interactions >= 1.5f)
                                {
                                    Adept.SeptimalPower += 50;
                                }
                                return false;
                            default:
                                return true;
                        }
                    }
                }
                else if (entity is NPC npc)
                {
                    if (NPCInteractions.TryGetValue(npc.type, out float interactions))
                    {
                        switch (interactions)
                        {
                            case < 0:
                                if (interactions == -1.5f)
                                {
                                    Adept.overheat(60);
                                }
                                return false;
                            case > 1:
                                if (interactions >= 1.5f)
                                {
                                    Adept.SeptimalPower += 50;
                                }
                                return false;
                            default:
                                return true;
                        }
                    }
                }
            }
            return true;
        }

        public override bool OnPrevasion(Player.HurtInfo info)
        {
            AdeptMuse muse = Player.GetModPlayer<AdeptMuse>();
            if (!Adept.IsOverheated && muse.AnthemLevel >= 1 && info.Damage <= ((Player.statLifeMax + Player.statLifeMax2) / 4))
            {
                if (muse.AnthemLevel < 5 && Adept.IsUsingPrimaryAbility) return false;
                Main.NewText("Prevasion");
                Adept.TimeSincePrimary = 0;
                Adept.IsRecharging = false;
                Adept.RechargeTimer = 0;
                Player.immune = true;
                Player.immuneTime = 60;
                return true;
            }
            return false;
        }

        public override void OnHit(ref Player.HurtModifiers modifiers)
        {
            if (modifiers.DamageSource.TryGetCausingEntity(out Entity entity))
            {
                if (entity is Projectile projectile)
                {
                    if (ProjectileInteractions.TryGetValue(projectile.type, out float interactions))
                    {
                        switch (interactions)
                        {
                            case -1.5f:
                                Adept.overheat(60);
                                return;
                            case 1 or 0.5f:
                                modifiers.IncomingDamageMultiplier *= 0.5f;
                                return;
                            case > 1:
                                modifiers.IncomingDamageMultiplier *= 0;
                                return;
                            default:
                                return;
                        }
                    }
                }
                else if (entity is NPC npc)
                {
                    if (NPCInteractions.TryGetValue(npc.type, out float interactions))
                    {
                        switch (interactions)
                        {
                            case -1.5f:
                                Adept.overheat(60);
                                return;
                            case 1 or 0.5f:
                                modifiers.IncomingDamageMultiplier *= 0.5f;
                                return;
                            default:
                                return;
                        }
                    }
                }
            }
        }

        public override void MorbAttack(int timer)
        {
            if (timer == 1)
            {
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center - new Vector2(184, 0), new Vector2(0), ModContent.ProjectileType<Thunder>(), (int)(150 * Math.Pow(Adept.SpecialDamageLevelMult, 2)), 14, Player.whoAmI, 1);
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
            FlashfieldCrashUpdate();
            UpdateTaggedNPCs();
            CheckEvolution();
            UpdateEvolution();
            if (visualProjectileTimer > 0) visualProjectileTimer--;
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

        public override void CheckEvolution()
        {
            if (Adept.Level >= 20 && NPC.downedMechBossAny &&
                Adept.PowerLevel == 1 && Muse.AnthemLevel > 0 &&
                Muse.MinutesWithMuseItems >= 10)
            {
                Adept.PowerLevel++;
                Main.NewText("Your septima has reached new heights!", ClearColor);
            }

            if (Adept.Level >= 45 && NPC.downedMoonlord &&
                Adept.PowerLevel == 2 && Adept.UnlockedPotential)
            {
                Adept.PowerLevel++;
                Main.NewText("Your septima has reached it's pinnacle!", ClearColor);
            }
        }

        public override void UpdateEvolution()
        {
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

        public void FlashfieldCrashUpdate()
        {
            if (!Adept.IsUsingPrimaryAbility || !Adept.CanUsePrimary)
            {
                crashing = false;
            }

            if (crashing)
            {
                Dust.NewDust(Player.Center, 12, 12, DustID.AncientLight);
                crashMeter++;
            } else
            {
                if (Adept.IsUsingPrimaryAbility && Adept.CanUsePrimary)
                {
                    crashMeter--;
                } else
                {
                    crashMeter -= 2;
                }
            }

            if (crashMeter >= maxCrashMeter)
            {
                Adept.overheat(20);
                crashMeter = 0;
                crashing = false;
            }
        }

        public void FlashfieldCrashCheck()
        {
            // The player has pvp on
            if (!Player.hostile) return;

            foreach (Player player in Main.player)
            {
                // The target is not the player itself
                if (player == Player) continue;

                // The target has pvp active an is from an opposite team
                if (!player.InOpposingTeam(Player) || !player.hostile)

                // The target is in range
                if (!player.WithinRange(Player.Center, 704)) continue;

                // The target's septima is Azure Thunderclap or Azure Striker
                AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
                if (adept.Septima is AzureThunderclap || adept.Septima is AzureStriker)
                {
                    // The target can use and is using flashfield
                    if (adept.IsUsingPrimaryAbility && adept.CanUsePrimary)
                    {
                        crashing = true;
                        Dust.NewDust((player.Center + Player.Center) / 2, 12, 12, DustID.AncientLight);
                        Dust.NewDust(Player.Center, 12, 12, DustID.AncientLight);
                    }
                } else
                {
                    continue;
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
