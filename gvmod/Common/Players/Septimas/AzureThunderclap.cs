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
    internal class AzureThunderclap : Septima
    {
        private bool crashing = false;
        private int crashMeter = 0;
        private int maxCrashMeter = 90;
        private int secondaryDuration = 5;
        private int visualProjectileTimer = 12;
        private List<Tag> taggedNPCs = new List<Tag>();
        private int flashfieldIndex;
        private bool flashfieldExists = false;
        private int sphere1Index;
        private bool sphere1Exists = false;
        private int sphere2Index;
        private bool sphere2Exists = false;
        private int sphere3Index;
        private bool sphere3Exists = false;
        private Vector2 basePosition = new Vector2(128);

        public AzureThunderclap(AdeptPlayer adept, AdeptMuse muse, Player player) : base(adept, muse, player)
        {
            SecondaryCooldownTime = 600;
            SpBaseUsage = 0.5f;
            SpBaseRegen = 2f;
            SpBaseOverheatRegen = 0.5f;
            ApBaseRegen = (1f / 3820f);
            InitializeAbilitiesList();
        }

        public override string Name => "Azure Thunderclap";

        public override bool CanRecharge => true;

        public override Color ClearColor => new Color(77, 175, 232);
        public override Color MainColor => new Color(44, 143, 205);
        public override Color DarkColor => new Color(12, 112, 179);

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
            [ProjectileID.InfluxWaver] = -1,
            [ProjectileID.EyeBeam] = -1.5f
        };

        public override Dictionary<int, float> NPCInteractions => new Dictionary<int, float> 
        {
            [NPCID.WaterSphere] = -1.5f,
            [NPCID.DetonatingBubble] = -1f
        };

        public override void InitializeAbilitiesList()
        {
            Abilities.Clear();
            Abilities.Add(new None(Player, Adept, "T"));
            Abilities.Add(new Astrasphere(Player, Adept, "T"));
            Abilities.Add(new GalvanicPatch(Player, Adept, "T"));
            Abilities.Add(new Sparkcaliburg(Player, Adept, "T"));
            Abilities.Add(new SplitSecond(Player, Adept, "T"));
            Abilities.Add(new GalvanicRenewal(Player, Adept, "T"));
            Abilities.Add(new VoltaicChains(Player, Adept, "T"));
            if (Adept.PowerLevel >= 2)
            {
                Abilities.Add(new VoltaicChainsMeteor(Player, Adept, "T"));
            }
            Abilities.Add(new GloriousStrizer(Player, Adept, "T"));
            Abilities.Add(new SeptimalSurge(Player, Adept, "T"));
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
            SpBaseUsage = 0.5f;
            morb.AttackTimer = 22;
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
                Adept.overheat(60);
                return;
            }
            else
            {
                SpBaseUsage = 0.5f;
            }
        }

        public override void FirstAbility()
        {
            int totalSphereDamage = (int)(60 * Adept.PrimaryDamageLevelMult * Adept.PrimaryDamageEquipMult);
            int totalFlashfieldDamage = 1 + (int)(10 * Adept.PrimaryDamageLevelMult * Adept.PrimaryDamageEquipMult * 1.4);
            float baseTagDamage = 3 * Adept.PrimaryDamageLevelMult * Adept.PrimaryDamageEquipMult * 1.2f;
            float sizeMod = 1;

            switch (Adept.PowerLevel)
            {
                case 2:
                    sizeMod = 0.5f;
                    totalFlashfieldDamage = 1 + (int)(25 * Adept.PrimaryDamageLevelMult * Adept.PrimaryDamageEquipMult * 1.2);
                    baseTagDamage *= 1.4f;
                    break;
                case 3:
                    sizeMod = 0.8f;
                    totalFlashfieldDamage = 1 + (int)(40 * Adept.PrimaryDamageLevelMult * Adept.PrimaryDamageEquipMult * 1.2);
                    baseTagDamage *= 1.8f;
                    break;
            }

            if (!flashfieldExists) flashfieldIndex = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<FlashfieldStriker>(), totalFlashfieldDamage, 10, Player.whoAmI, 0, ai2:sizeMod);

            if (Adept.PowerLevel >= 3)
            {
                if (!sphere1Exists) sphere1Index = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + basePosition, new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), totalSphereDamage, 8, Player.whoAmI, 2, 0, flashfieldIndex);
                if (!sphere2Exists) sphere2Index = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + basePosition.RotatedBy(MathHelper.ToRadians(120)), new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), totalSphereDamage, 8, Player.whoAmI, 2, 1, flashfieldIndex);
                if (!sphere3Exists) sphere3Index = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + basePosition.RotatedBy(MathHelper.ToRadians(-120)), new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), totalSphereDamage, 8, Player.whoAmI, 2, 2, flashfieldIndex);
                basePosition = new Vector2(128) * sizeMod;
                basePosition = basePosition.RotatedBy(MathHelper.ToRadians(3.5f));
            }

            foreach (Tag tag in taggedNPCs)
            {
                float tagMultiplier = (float)(tag.Level * 0.6);
                if (tag.ShockIframes == 0)
                {
                    Player.ApplyDamageToNPC(Main.npc[tag.NpcIndex], (int)(baseTagDamage * tagMultiplier), 0, Player.direction, damageType: ModContent.GetInstance<SeptimaDamageClass>(), damageVariation: true);
                    tag.ShockIframes = 12;
                }
            }
        }

        public override void SecondAbilityEffects()
        {
            if (SecondaryTimer <= 5 && Adept.SecondaryInUse)
            {
                switch (Adept.PowerLevel)
                {
                    case 1:
                        for (int i = 0; i < 7; i++)
                        {
                            Dust.NewDust(new Vector2(Player.Center.X, Player.position.Y + Player.height), 10, 10, DustID.MartianSaucerSpark, 46);
                            Dust.NewDust(new Vector2(Player.Center.X, Player.position.Y + Player.height), 10, 10, DustID.MartianSaucerSpark, -46);
                        }
                        break;
                }
            }
        }

        public override void SecondAbility()
        {
            if (SecondaryTimer <= 1 && Adept.SecondaryInUse)
            {
                switch (Adept.PowerLevel)
                {
                    case 1:
                        Projectile.NewProjectile(Player.GetSource_FromThis(), new Vector2(Player.Center.X, Player.position.Y + Player.height), new Vector2(10, 0), ModContent.ProjectileType<LightningCreeper>(), (int)(30 * Adept.SecondaryDamageLevelMult * Adept.SecondaryDamageEquipMult), 10, Player.whoAmI);
                        Projectile.NewProjectile(Player.GetSource_FromThis(), new Vector2(Player.Center.X, Player.position.Y + Player.height), new Vector2(-10, 0), ModContent.ProjectileType<LightningCreeper>(), (int)(30 * Adept.SecondaryDamageLevelMult * Adept.SecondaryDamageEquipMult), 10, Player.whoAmI);
                        break;
                    case 2:
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(70, -58), Vector2.Zero, ModContent.ProjectileType<CapsuleSphere>(), (int)(80 * Adept.SecondaryDamageLevelMult * Adept.SecondaryDamageEquipMult), 14, Player.whoAmI);
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(-70, -58), Vector2.Zero, ModContent.ProjectileType<CapsuleSphere>(), (int)(80 * Adept.SecondaryDamageLevelMult * Adept.SecondaryDamageEquipMult), 14, Player.whoAmI);
                        break;
                    case 3:
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Player.Center.DirectionTo(Main.MouseWorld) * 20, ModContent.ProjectileType<ElectricBullet>(), (int)(184 * Adept.SecondaryDamageLevelMult * Adept.SecondaryDamageEquipMult), 12, Player.whoAmI);
                        break;
                }
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
            if (!Adept.IsOverheated && muse.AnthemLevel >= 1 && info.SourceDamage <= ((Player.statLifeMax + Player.statLifeMax2) / 12))
            {
                Main.NewText("Prevasion");
                Adept.TimeSincePrimary = 0;
                Adept.IsRecharging = false;
                Adept.RechargeTimer = 0;
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
            if (timer == 21)
            {
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(0), ModContent.ProjectileType<Thunder>(), (int)(200 * Math.Pow(Adept.SpecialDamageLevelMult, 1.5)), 14, Player.whoAmI, 2);
            }
            int xPos = 20 - timer;
            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(20 * xPos, 0), new Vector2(0), ModContent.ProjectileType<Thunder>(), (int)(200 * Math.Pow(Adept.SpecialDamageLevelMult, 1.5)), 14, Player.whoAmI, 2);
            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center - new Vector2(20 * xPos, 0), new Vector2(0), ModContent.ProjectileType<Thunder>(), (int)(200 * Math.Pow(Adept.SpecialDamageLevelMult, 1.5)), 14, Player.whoAmI, 2);
        }

        public override void MiscEffects()
        {
        }

        public override void Updates()
        {
            UpdateMarkedNPCs();
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
            }
            else
            {
                flashfieldExists = false;
            }

            Projectile sphere1 = Main.projectile[sphere1Index];
            if (sphere1.active && sphere1.ModProjectile is ElectricSphere && sphere1.owner == Player.whoAmI)
            {
                sphere1Exists = true;
            }
            else
            {
                sphere1Exists = false;
            }

            Projectile sphere2 = Main.projectile[sphere2Index];
            sphere2Exists = sphere2.active && sphere2.ModProjectile is ElectricSphere && sphere2.owner == Player.whoAmI;

            Projectile sphere3 = Main.projectile[sphere3Index];
            if (sphere3.active && sphere3.ModProjectile is ElectricSphere && sphere3.owner == Player.whoAmI)
            {
                sphere3Exists = true;
            }
            else
            {
                sphere3Exists = false;
            }

            Player.velocity *= VelocityMultiplier;
        }

        public override void CheckEvolution()
        {
            if (Adept.Level >= 30 && NPC.downedMechBoss1 &&
                NPC.downedMechBoss2 && NPC.downedMechBoss3 &&
                Adept.PowerLevel == 1 && Muse.HasAMuseItem)
            {
                Adept.PowerLevel++;
                InitializeAbilitiesList();
                Main.NewText("Your septima has reached new heights!", ClearColor);
            }

            if (Adept.Level >= 60 && NPC.downedMoonlord &&
                Adept.PowerLevel == 2 && Adept.UnlockedPotential)
            {
                Adept.PowerLevel++;
                InitializeAbilitiesList();
                Main.NewText("Your septima has reached it's pinnacle!", ClearColor);
            }
        }

        public override void UpdateEvolution()
        {
            if (Adept.PowerLevel == 2)
            {
                secondaryDuration = 90;
            }
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
            }
            else
            {
                if (Adept.IsUsingPrimaryAbility && Adept.CanUsePrimary)
                {
                    crashMeter--;
                }
                else
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
            foreach (Player player in Main.player)
            {
                // The target is not the player
                if (player == Player) continue;

                // The target is in range
                if (!player.WithinRange(Player.Center, 704)) continue;

                // The target's septima is Azure Thunderclap or Azure Striker
                AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
                if (adept.Septima is AzureThunderclap || adept.Septima is AzureStriker)
                {
                    // The target can use and is using it's primary ability, flashfield
                    if (adept.IsUsingPrimaryAbility && adept.CanUsePrimary)
                    {
                        crashing = true;
                    }
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
