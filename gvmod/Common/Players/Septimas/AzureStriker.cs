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
using Terraria.DataStructures;
using gvmod.Content.Buffs;

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
        private List<Tag> taggedEntities = new List<Tag>();
        private int flashfieldIndex;
        private bool flashfieldExists = false;
        private int sphere1Index;
        private bool sphere1Exists = false;
        private int sphere2Index;
        private bool sphere2Exists = false;
        private int sphere3Index;
        private bool sphere3Exists = false;
        private Vector2 basePosition = new Vector2(128);

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

        // Value of less than 0 penetrates prevasion, value of less than -1.5 overheat on hit.
        // Value of more than 1 increases SP.
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
            if (Adept.PowerLevel >= 3)
            {
                Abilities.Add(new OctisVeto(Player, Adept, "S"));
            }
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
            int totalSphereDamage = 1 + (int)(40 * Adept.PrimaryDamageLevelMult * Adept.PrimaryDamageEquipMult);
            int totalFlashfieldDamage = 1 + (int)(1 * Adept.PrimaryDamageLevelMult * Adept.PrimaryDamageEquipMult * 2);
            float baseTagDamage = 1 * Adept.PrimaryDamageLevelMult * Adept.PrimaryDamageEquipMult * 2.4f;
            
            switch (Adept.PowerLevel)
            {
                case 2:
                    totalFlashfieldDamage = 1 + (int)(5 * Adept.PrimaryDamageLevelMult * Adept.PrimaryDamageEquipMult * 2);
                    baseTagDamage *= 1.5f;
                    break;
                case 3:
                    totalFlashfieldDamage = 1 + (int)(18 * Adept.PrimaryDamageLevelMult * Adept.PrimaryDamageEquipMult * 2);
                    baseTagDamage *= 2f;
                    break;
            }

            if (!flashfieldExists) flashfieldIndex = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(0f, 0f), ModContent.ProjectileType<FlashfieldStriker>(), totalFlashfieldDamage, 0, Player.whoAmI, 0);
            
            if (Adept.PowerLevel >= 3)
            {
                if (!sphere1Exists) sphere1Index = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + basePosition, new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), totalSphereDamage, 8, Player.whoAmI, 1, 0, flashfieldIndex);
                if (!sphere2Exists) sphere2Index = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + basePosition.RotatedBy(MathHelper.ToRadians(120)), new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), totalSphereDamage, 8, Player.whoAmI, 1, 1, flashfieldIndex);
                if (!sphere3Exists) sphere3Index = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + basePosition.RotatedBy(MathHelper.ToRadians(-120)), new Vector2(0f, 0f), ModContent.ProjectileType<ElectricSphere>(), totalSphereDamage, 8, Player.whoAmI, 1, 2, flashfieldIndex);
                basePosition = basePosition.RotatedBy(MathHelper.ToRadians(3.5f));
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

            foreach (Tag tag in taggedEntities)
            {
                float tagMultiplier = (float)((tag.Level * 0.7) + 0.2);
                if (tag.ShockIframes == 0)
                {
                    if (tag.IsPlayer) Main.player[tag.Index].Hurt(PlayerDeathReason.ByCustomReason("Tag"), (int)(baseTagDamage * tagMultiplier), 0, true, false, -1, false, 20, 0, 0);
                    else Player.ApplyDamageToNPC(Main.npc[tag.Index], (int)(baseTagDamage * tagMultiplier), 0, Player.direction, damageType: ModContent.GetInstance<SeptimaDamageClass>(), damageVariation: true);
                    tag.ShockIframes = 8;
                }
            }

            FlashfieldCrashCheck();
        }

        public override void SecondAbilityEffects()
        {
            switch (Adept.PowerLevel)
            {
                case 1 or 2:
                    for (int i = 0; i < 20; i++)
                    {
                        float xPos = Main.rand.NextFloat(-16, 16);
                        float yPos = Main.rand.NextFloat(-500, 500);
                        Dust.NewDust(Player.Center + new Vector2(xPos, yPos), 10, 10, DustID.MartianSaucerSpark, 0, 0, 100,
                            Color.DeepSkyBlue);
                    }
                    break;
                case 3:
                    if (SecondaryTimer == 1 && Adept.SecondaryInUse)
                    {
                        Vector2 direction = Player.Center.DirectionTo(Main.MouseWorld);
                        int segments = (int)(Player.Center.Distance(Main.MouseWorld)/5);
                        for (int i = 0; i < segments; i++)
                        {
                            float xPos = Player.Center.X + (direction.X * i * 5);
                            float yPos = Player.Center.Y + (direction.Y * i * 5);
                            Dust.NewDust(new(xPos, yPos), 10, 10, DustID.MartianSaucerSpark, 
                                0, 0, 100, Color.DeepSkyBlue);
                        }
                    }
                    break;
            }
        }

        public override bool CanUseSecondary()
        {
            if (Adept.PowerLevel >= 3)
            {
                Point position = Main.MouseWorld.ToTileCoordinates();
                return !Collision.SolidTiles(position.X - 1, position.X + 1, position.Y - 2, position.Y + 1);
            }
            return true;
        }

        public override void SecondAbility()
        {
            if (SecondaryTimer == 2 && Adept.SecondaryInUse)
            {
                switch (Adept.PowerLevel)
                {
                    case 1 or 2:
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(0),
                            ModContent.ProjectileType<Thunder>(),
                            (int)(50 * Adept.SecondaryDamageLevelMult * Adept.SecondaryDamageEquipMult), 8, 
                            Player.whoAmI);
                        break;
                    case 3:
                        Player.Center = Main.MouseWorld;
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
                                if (interactions <= -1.5f)
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
            if (!Adept.IsOverheated && muse.AnthemLevel >= 1 && info.Damage <= ((Player.statLifeMax + Player.statLifeMax2) / 15))
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
            if (Player.HasBuff(BuffID.Electrified))
            {
                Player.ClearBuff(BuffID.Electrified);
                Player.AddBuff(ModContent.BuffType<GoodElectrified>(), 180);
            }
        }

        public override void MiscMoveOverride()
        {
            if (isFalling && Adept.IsUsingPrimaryAbility && Adept.CanUsePrimary)
            {
                VelocityMultiplier = new Vector2(1f, 0.85f);
                Player.slowFall = true;
            }
            if (Adept.IsUsingSecondaryAbility && Adept.CanUseSecondary)
            {
                VelocityMultiplier = new Vector2(0f, 0.00001f);
                Player.slowFall = true;
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
            flashfieldExists = flashfield.active && flashfield.ModProjectile is FlashfieldStriker;

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
            if (sphere2.active && sphere2.ModProjectile is ElectricSphere && sphere2.owner == Player.whoAmI)
            {
                sphere2Exists = true;
            }
            else
            {
                sphere2Exists = false;
            }

            Projectile sphere3 = Main.projectile[sphere3Index];
            if (sphere3.active && sphere3.ModProjectile is ElectricSphere && sphere3.owner == Player.whoAmI)
            {
                sphere3Exists = true;
            }
            else
            {
                sphere3Exists = false;
            }
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
            if (Adept.PowerLevel == 3)
            {
                secondaryDuration = 5;
                SecondaryCooldownTime = 600;
            }
        }

        public void UpdateTaggedNPCs()
        {
            for (int i = 0; i < taggedEntities.Count; i++)
            {
                Tag theTagInQuestion = taggedEntities[i];
                TaggedNPC globalTag = Main.npc[theTagInQuestion.Index].GetGlobalNPC<TaggedNPC>();
                theTagInQuestion.Update();

                if (!theTagInQuestion.Active)
                {
                    globalTag.Shocked = false;
                    taggedEntities.Remove(theTagInQuestion);
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

        public void FlashfieldCrashCheck(float flashfieldSize=1)
        {
            // The player has pvp on
            if (!Player.hostile) return;

            foreach (Player player in Main.player)
            {
                // The target is not the player itself
                if (player == Player) continue;

                // The target has pvp active an is from an opposite team
                if (!player.InOpposingTeam(Player) || !player.hostile) continue;

                // The target is in range
                if (!player.WithinRange(Player.Center, 704*flashfieldSize)) continue;

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
            foreach (Tag tag in taggedEntities)
            {
                NPC theNpcInQuestion = Main.npc[tag.Index];
                if (target == theNpcInQuestion && tag.Active)
                {
                    tag.IncreaseTag();
                    return;
                }
            }
            taggedEntities.Add(new Tag(target.whoAmI));
        }

        public void AddTaggedPlayer(Player target)
        {
            foreach (Tag tag in taggedEntities)
            {
                Player thePlayerInQuestion = Main.player[tag.Index];
                if (target == thePlayerInQuestion && tag.Active)
                {
                    tag.IncreaseTag();
                    return;
                }
            }
            taggedEntities.Add(new Tag(target.whoAmI));
        }
    }
}
