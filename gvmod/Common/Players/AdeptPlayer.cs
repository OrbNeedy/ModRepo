using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using gvmod.Common.Systems;
using gvmod.Common.Players.Septimas;
using Terraria.ID;
using Terraria.ModLoader.IO;
using gvmod.Common.Players.Septimas.Skills;
using System.Collections.Generic;
using System;
using Terraria.DataStructures;
using gvmod.Content.Buffs;
using Microsoft.Xna.Framework;

namespace gvmod.Common.Players
{
    public class AdeptPlayer : ModPlayer
    {
        public Septima Septima { get; set; }
        public float MaxSeptimalPower { get; set; }
        public float MaxSeptimalPower2 { get; set; }
        public int PowerLevel { get; set; }
        public bool UnlockedPotential { get; set; }

        public bool[] SPUpgrades { get; set; }
        public float SeptimalPower { get; set; }
        public float MaxAbilityPower { get; set; }
        public float AbilityPower { get; set; }
        
        public int Level { get; set; }
        public int Experience { get; set; }
        public int MaxEXP { get; set; }
        public int ExtraEXP { get; set; }

        public int Kudos { get; set; }
        public int KudosTimer { get; set; }
        public int BossKudosGainTimer { get; set; }
        public int SpecificallyBossTimer { get; set; }
        
        public List<int> ActiveSlots { get; set; }

        private int rechargeComboCount;
        private const int rechargeCooldown = 60;
        private const int rechargeDuration = 30;
        private int rechargeDelay;
        public int RechargeTimer { get; set; }

        private int slotToUse;

        public float PrimaryDamageLevelMult { get; set; }
        public float SecondaryDamageLevelMult { get; set; }
        public float SpecialDamageLevelMult { get; set; }
        public float PrimaryDamageEquipMult { get; set; }
        public float SecondaryDamageEquipMult { get; set; }
        public float SpecialDamageEquipMult { get; set; }

        public float APUsageModifier { get; set; }
        public float SPUsageModifier { get; set; }
        public float APRegenModifier { get; set; }
        public float SPRegenModifier { get; set; }
        public float SPRegenOverheatModifier { get; set; }

        public bool IsUsingPrimaryAbility { get; set; }
        public bool CanUsePrimary { get; set; }
        public bool IsUsingSecondaryAbility { get; set; }
        public bool CanUseSecondary { get; set; }
        public bool IsUsingSpecialAbility { get; set; }
        public bool IsRecharging { get; set; }

        public bool SecondaryInUse { get; set; }
        public int TimeSinceSecondary { get; set; }
        public bool SecondaryInCooldown { get; set; }
        public bool IsOverheated { get; set; }
        public int TimeSincePrimary { get; set; }
        private bool cantMove;
        public bool SpecialInvincibility { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
            MaxSeptimalPower = 150;
            MaxSeptimalPower2 = MaxSeptimalPower;
            SeptimalPower = MaxSeptimalPower;
            MaxAbilityPower = 3;
            AbilityPower = MaxAbilityPower;
            Septima = GetSeptima(Main.rand.Next(2));
            cantMove = false;
            Level = 1;
            Experience = 0;
            ExtraEXP = 0;
            MaxEXP = 1999999999;
            Kudos = 0;
            KudosTimer = 0;
            BossKudosGainTimer = -1;
            PrimaryDamageLevelMult = 1;
            SecondaryDamageLevelMult = 1;
            SpecialDamageLevelMult = 1;
            ActiveSlots = new List<int>() { 0, 0, 0, 0};
            SPUpgrades = new bool[3] {false, false, false};
            PowerLevel = 1;
            slotToUse = 0;
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("Level"))
            {
                Level = tag.GetInt("Level");
            }
            if (tag.ContainsKey("Experience"))
            {
                Experience = tag.GetInt("Experience");
            }
            if (tag.ContainsKey("Septima"))
            {
                Septima = GetSeptima(tag.GetString("Septima"));
            }
            if (tag.ContainsKey("MaxSP"))
            {
                MaxSeptimalPower = tag.GetFloat("MaxSP");
            }
            if (tag.ContainsKey("Upgrade1"))
            {
                SPUpgrades[0] = tag.GetBool("Upgrade1");
            }
            if (tag.ContainsKey("Upgrade2"))
            {
                SPUpgrades[1] = tag.GetBool("Upgrade2");
            }
            if (tag.ContainsKey("Upgrade3"))
            {
                SPUpgrades[2] = tag.GetBool("Upgrade3");
            }
            if (tag.ContainsKey("PowerLevel"))
            {
                PowerLevel = tag.GetInt("PowerLevel");
            }
            if (tag.ContainsKey("UnlockedPotential"))
            {
                UnlockedPotential = tag.GetBool("UnlockedPotential");
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag["Level"] = Level;
            tag["Experience"] = Experience;
            tag["Septima"] = Septima.Name;
            tag["MaxSP"] = MaxSeptimalPower;
            tag["Upgrade1"] = SPUpgrades[0];
            tag["Upgrade2"] = SPUpgrades[1];
            tag["Upgrade3"] = SPUpgrades[2];
            tag["PowerLevel"] = PowerLevel;
            tag["UnlockedPotential"] = UnlockedPotential;
        }

        public override void OnEnterWorld(Player player)
        {
            ActiveSlots = new List<int>() { 0, 0, 0, 0};
            Septima.InitializeAbilitiesList();
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (KeybindSystem.primaryAbility.JustPressed)
            {
                IsUsingPrimaryAbility = true;
            }
            if (KeybindSystem.primaryAbility.JustReleased)
            {
                IsUsingPrimaryAbility = false;
            }
            if (KeybindSystem.secondaryAbility.JustPressed)
            {
                IsUsingSecondaryAbility = true;
                Main.NewText("Cooldown: " + TimeSinceSecondary);
            }
            if (KeybindSystem.special1.JustPressed)
            {
                Special special = Septima.Abilities[ActiveSlots[0]];
                if (FigureSpecialAvailability(special))
                {
                    AbilityPower -= (special.ApUsage * APUsageModifier);
                    special.SpecialTimer = 0;
                    slotToUse = 0;
                    if (special.IsOffensive)
                    {
                        cantMove = true;
                    }
                    if (special.GivesIFrames)
                    {
                        SpecialInvincibility = true;
                    }
                }
            }
            if (KeybindSystem.special2.JustPressed)
            {
                Special special = Septima.Abilities[ActiveSlots[1]];
                if (FigureSpecialAvailability(special))
                {
                    AbilityPower -= (special.ApUsage * APUsageModifier);
                    special.SpecialTimer = 0;
                    slotToUse = 1;
                    if (special.IsOffensive)
                    {
                        cantMove = true;
                    }
                    if (special.GivesIFrames)
                    {
                        SpecialInvincibility = true;
                    }
                }
            }
            if (KeybindSystem.special3.JustPressed)
            {
                Special special = Septima.Abilities[ActiveSlots[2]];
                if (FigureSpecialAvailability(special))
                {
                    AbilityPower -= (special.ApUsage * APUsageModifier);
                    special.SpecialTimer = 0;
                    slotToUse = 2;
                    if (special.IsOffensive)
                    {
                        cantMove = true;
                    }
                    if (special.GivesIFrames)
                    {
                        SpecialInvincibility = true;
                    }
                }
            }
            if (KeybindSystem.special4.JustPressed)
            {
                Special special = Septima.Abilities[ActiveSlots[3]];
                if (FigureSpecialAvailability(special))
                {
                    AbilityPower -= (special.ApUsage * APUsageModifier);
                    special.SpecialTimer = 0;
                    slotToUse = 3;
                    if (special.IsOffensive)
                    {
                        cantMove = true;
                    }
                    if (special.GivesIFrames)
                    {
                        SpecialInvincibility = true;
                    }
                }
            }

            
        }

        public override void PostUpdateMiscEffects()
        {
            if (SeptimalPower <= 0 && TimeSincePrimary <= 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust.NewDust(Player.position, 10, 10, DustID.Electric, 0, 0);
                }
            }
            if (IsUsingPrimaryAbility && CanUsePrimary)
            {
                Septima.FirstAbilityEffects();
            }
            if (SecondaryInUse && CanUseSecondary)
            {
                Septima.SecondAbilityEffects();
            }
            if (IsUsingSpecialAbility)
            {
                Septima.Abilities[ActiveSlots[slotToUse]]?.Effects();
            }

            if (IsOverheated)
            {
                DuringOverheat();
            }

            if (Septima.CanRecharge && rechargeComboCount != 0 && rechargeDelay == 0 && !IsOverheated && !IsUsingPrimaryAbility)
            {
                rechargeDelay = rechargeCooldown;
                RechargeTimer = rechargeDuration;
                // Insert visual effect
            }
            if (rechargeDelay > 0)
            {
                rechargeDelay--;
            }
            if (RechargeTimer > 0)
            {
                RechargeTimer--;
                SPRegenModifier = MaxSeptimalPower2 / 60;
                TimeSincePrimary = 60;
                IsRecharging = true;
            } else
            {
                IsRecharging = false;
            }
        }

        public override void SetControls()
        {
            if (cantMove)
            {
                Player.controlJump = false;
                Player.controlLeft = false;
                Player.controlRight = false;
                Player.controlDown = false;
                Player.controlUp = false;
                Player.controlHook = false;
                Player.controlUseItem = false;
                Player.controlUseTile = false;
                Player.controlMount = false;
            }
        }

        public override void PostUpdate()
        {
            FigureAvailability();
            UpdateKudos();
            UpdateLevelMultipliers();
            if (IsUsingPrimaryAbility && CanUsePrimary)
            {
                Septima.FirstAbility();
            }
            if (SecondaryInUse && CanUseSecondary)
            {
                Septima.SecondAbility();
            }
            if (IsUsingSpecialAbility)
            {
                Septima.Abilities[ActiveSlots[slotToUse]]?.Attack();
            } else
            {
                cantMove = false;
                SpecialInvincibility = false;
            }
            UpdateSeptimalPower();
            MaxEXP = (int)Math.Pow(Level * 80, 1.47f);
            if (Experience >= MaxEXP)
            {
                Level++;
                if (Experience > MaxEXP)
                {
                    ExtraEXP = Experience - MaxEXP;
                } 
                Experience = 0;
            } else
            {
                Experience += ExtraEXP;
                ExtraEXP = 0;
            }
            Septima.Updates();
            Septima.MiscEffects();

        }

        private void FigureAvailability()
        {
            if (!IsRecharging)
            {
                if (!IsOverheated && (!SecondaryInUse || SecondaryInCooldown) && !IsUsingSpecialAbility && !Player.HasBuff(BuffID.Stoned))
                {
                    CanUsePrimary = true;
                } else
                {
                    CanUsePrimary = false;
                }

                if (!IsUsingSpecialAbility && !SecondaryInCooldown && !Player.HasBuff(BuffID.Stoned))
                {
                    CanUseSecondary = true;
                }
                else
                {
                    CanUseSecondary = false;
                }
            } else
            {
                CanUsePrimary = false;
                CanUseSecondary = false;
            }
        }

        private bool FigureSpecialAvailability(Special special)
        {
            if (special != null && !special.BeingUsed && AbilityPower >= special.ApUsage && !special.InCooldown && (special != null || special.Name != "") && !IsUsingSpecialAbility && !Player.HasBuff(BuffID.Stoned))
            {
                return true;
            } else
            {
                return false;
            }
        }

        public override void ResetEffects()
        {
            base.ResetEffects();
            ResetEquipMultipliers();
            ResetResources();
            if (Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[0] < 15 && !IsUsingSpecialAbility)
            {
                rechargeComboCount++;
            }
            else
            {
                rechargeComboCount = 0;
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            AdeptMuse muse = Player.GetModPlayer<AdeptMuse>();
            if ((muse.HasMirrorShard || muse.HasWholeMirror || muse.HasDjinnLamp) && muse.AnthemLevel <= 0)
            {
                int fullHealth = (Player.statLifeMax + Player.statLifeMax2);
                Player.statLife += fullHealth;
                Player.HealEffect(fullHealth);
                Player.AddBuff(ModContent.BuffType<AnthemBuff>(), 7200);
                return false;
            }
            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            if (IsUsingSpecialAbility && Septima.Abilities[ActiveSlots[slotToUse]].GivesIFrames)
            {
                Player.immune = true;
                return false;
            }
            return !Septima.OnPrevasion(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, ref cooldownCounter);
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            base.OnHitByNPC(npc, damage, crit);
            Kudos = 0;
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            base.OnHitByProjectile(proj, damage, crit);
            Kudos = 0;
        }

        public void UpdateLevelMultipliers()
        {
            PrimaryDamageLevelMult = (1 + Level * 0.012f);
            SecondaryDamageLevelMult = (1 + Level * 0.01f);
            SpecialDamageLevelMult = (1 + Level * 0.011f);
        }

        public void ResetEquipMultipliers()
        {
            PrimaryDamageEquipMult = 1;
            SecondaryDamageEquipMult = 1;
            SpecialDamageEquipMult = 1;
        }

        public void UpdateKudos()
        {
            AdeptMuse muse = Player.GetModPlayer<AdeptMuse>();
            List<NPC> npcs = GetNPCsInRadius(1600);
            int initialBossTimer = BossKudosGainTimer;
            KudosTimer++;
            foreach (NPC npc in npcs)
            {
                if (!npc.friendly && !npc.immortal)
                {
                    KudosTimer = 0;
                }
                if (npc.boss)
                {
                    BossKudosGainTimer++;
                    if (BossKudosGainTimer >= 6)
                    {
                        Kudos++;
                        BossKudosGainTimer = 0;
                    }
                    break;
                }
            }

            if (initialBossTimer <= -1 && BossKudosGainTimer >= 0)
            {
                Kudos = 0;
            }

            if (muse.AnthemLevel > 0 || KudosTimer >= 960)
            {
                BossKudosGainTimer = -1;
                Kudos = 0;
            }
        }

        public float SeptimalPowerToFraction()
        {
            return SeptimalPower / MaxSeptimalPower2;
        }

        public float ExperienceToFraction()
        {
            float exp = Experience;
            float maxExp = MaxEXP;
            return exp / maxExp;
        }

        public void UpdateSeptimalPower()
        {
            if (SeptimalPower <= 0)
            {
                if (!IsOverheated)
                {
                    OnOverheat();
                }
                IsOverheated = true;
                SeptimalPower = 0;
            }
            if (SeptimalPower >= MaxSeptimalPower2)
            {
                if (IsOverheated)
                {
                    OnRecover();
                }
                IsOverheated = false;
                SeptimalPower = MaxSeptimalPower2;
            }
            if (TimeSinceSecondary < Septima.SecondaryCooldownTime)
            {
                TimeSinceSecondary++;
                SecondaryInCooldown = true;
            }
            if (TimeSinceSecondary >= Septima.SecondaryCooldownTime)
            {
                SecondaryInCooldown = false;
            }
            if (AbilityPower < MaxAbilityPower)
            {
                AbilityPower += Septima.ApBaseRegen * APRegenModifier;
            }
            UpdateSeptimaForFirst();
            UpdateSeptimaForSecond();
            UpdateSeptimaForSpecial();
        }

        public void OnOverheat()
        {
            Septima.OnOverheat();
        }

        public void OnRecover()
        {
            Septima.OnRecovery();
        }

        public void DuringOverheat()
        {
            Septima.DuringOverheat();
        }

        public void ResetResources()
        {
            MaxSeptimalPower2 = MaxSeptimalPower;
            APUsageModifier = 1;
            SPUsageModifier = 1;
            APRegenModifier = 1;
            SPRegenModifier = 1;
            SPRegenOverheatModifier = 1;
        }

        public void UpdateSeptimaForFirst()
        {
            if (IsUsingPrimaryAbility && !IsOverheated && !(IsUsingSecondaryAbility || IsUsingSpecialAbility))
            {
                if (SeptimalPower > 0) SeptimalPower -= (Septima.SpBaseUsage * SPUsageModifier);
                TimeSincePrimary = 0;
            }
            if (!IsUsingPrimaryAbility || IsOverheated || IsUsingSecondaryAbility || IsUsingSpecialAbility) TimeSincePrimary++;
            if (TimeSincePrimary >= 60)
            {
                if (!IsOverheated)
                {
                    SeptimalPower += (Septima.SpBaseRegen * SPRegenModifier);
                }
                else
                {
                    SeptimalPower += (Septima.SpBaseOverheatRegen * SPRegenOverheatModifier);
                }
                TimeSincePrimary = 60;
            }
        }

        public void UpdateSeptimaForSecond()
        {
            if (IsUsingSecondaryAbility)
            {
                if (!SecondaryInCooldown)
                {
                    SecondaryInUse = true;
                } else
                {
                    if (TimeSinceSecondary > Septima.SecondaryCooldownTime) TimeSinceSecondary = Septima.SecondaryCooldownTime;
                    IsUsingSecondaryAbility = false;
                }
            } else
            {
                if (TimeSinceSecondary > Septima.SecondaryCooldownTime) TimeSinceSecondary = Septima.SecondaryCooldownTime;
            }
        }

        public void UpdateSeptimaForSpecial()
        {
            foreach (Special special in Septima.Abilities)
            {
                special.Update();
            }
        }

        public List<NPC> GetNPCsInRadius(int radius)
        {
            var closeNPCs = new List<NPC>();
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                var npc = Main.npc[i];
                if (!npc.active && npc.life <= 0) continue;
                if (Vector2.Distance(npc.Center, Player.Center) > radius)
                {
                    continue;
                }
                else
                {
                    closeNPCs.Add(Main.npc[i]);
                    break;
                };
            }
            return closeNPCs;
        }

        public Septima GetSeptima(int choice)
        {
            return choice switch
            {
                0 => new AzureStriker(this, Player.GetModPlayer<AdeptMuse>(), Player),
                1 => new AzureThunderclap(this, Player.GetModPlayer<AdeptMuse>(), Player),
                _ => new AzureStriker(this, Player.GetModPlayer<AdeptMuse>(), Player),
            };
        }

        public Septima GetSeptima(string name)
        {
            return name switch
            {
                "Azure Striker" => new AzureStriker(this, Player.GetModPlayer<AdeptMuse>(), Player),
                "Azure Thunderclap" => new AzureThunderclap(this, Player.GetModPlayer<AdeptMuse>(), Player),
                _ => new AzureStriker(this, Player.GetModPlayer<AdeptMuse>(), Player),
            };
        }

        public Special GetSpecial(string name)
        {
            for (int i = 0; i < Septima.Abilities.Count; i++)
            {
                Special temp = Septima.Abilities[i];
                if (temp.Name == name)
                {
                    return temp;
                }
            }
            return null;
        }
    }
}