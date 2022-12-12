using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using gvmod.Common.Systems;
using gvmod.Common.Players.Septimas;
using Terraria.ID;
using Terraria.ModLoader.IO;
using gvmod.Common.Players.Septimas.Abilities;
using System.Collections.Generic;
using System;
using Terraria.DataStructures;
using gvmod.Content.Buffs;

namespace gvmod.Common.Players
{
    public class AdeptPlayer : ModPlayer
    {
        public Septima Septima { get; set; }
        public float MaxSeptimalPower { get; set; }
        public float SeptimalPower { get; set; }
        public float maxAbilityPower { get; set; }
        public float abilityPower { get; set; }
        public int level { get; set; }
        public int experience { get; set; }
        public int maxEXP { get; set; }
        public int extraEXP { get; set; }
        public List<string> activeSlot { get; set; }

        private int rechargeComboCount;
        private const int rechargeCooldown = 60;
        private const int rechargeDuration = 30;
        private int rechargeDelay;
        private int rechargeTimer;

        public float primaryDamageLevelMult { get; set; }
        public float secondaryDamageLevelMult { get; set; }
        public float specialDamageLevelMult { get; set; }
        public float primaryDamageEquipMult { get; set; }
        public float secondaryDamageEquipMult { get; set; }
        public float specialDamageEquipMult { get; set; }

        public float APUsageModifier { get; set; }
        public float SPUsageModifier { get; set; }
        public float APRegenModifier { get; set; }
        public float SPRegenModifier { get; set; }
        public float SPRegenOverheatModifier { get; set; }

        public bool hasMirrorShard { get; set; }
        public bool hasBattlePod { get; set; }
        public bool hasMusesPendant { get; set; }
        public bool anthemState { get; set; }

        public bool isUsingPrimaryAbility { get; set; }
        public bool canUsePrimary { get; set; }
        public bool isUsingSecondaryAbility { get; set; }
        public bool canUseSecondary { get; set; }
        public bool isUsingSpecialAbility { get; set; }
        public bool isRecharging { get; set; }

        public bool secondaryInUse { get; set; }
        public int timeSinceSecondary { get; set; }
        public bool secondaryInCooldown { get; set; }
        public bool isOverheated { get; set; }
        public int timeSincePrimary { get; set; }
        private bool cantMove;

        public override void Initialize()
        {
            base.Initialize();
            MaxSeptimalPower = 300;
            SeptimalPower = MaxSeptimalPower;
            maxAbilityPower = 3;
            abilityPower = maxAbilityPower;
            Septima = GetSeptima(Main.rand.Next(2));
            cantMove = false;
            level = 1;
            experience = 0;
            extraEXP = 0;
            maxEXP = 1999999999;
            primaryDamageLevelMult = 1;
            secondaryDamageLevelMult = 1;
            specialDamageLevelMult = 1;
            activeSlot = new List<string>() { "", "", "", ""};
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("Level"))
            {
                level = tag.GetInt("Level");
            }
            if (tag.ContainsKey("Experience"))
            {
                experience = tag.GetInt("Experience");
            }
            if (tag.ContainsKey("Septima"))
            {
                Septima = GetSeptima(tag.GetString("Septima"));
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag["Level"] = level;
            tag["Experience"] = experience;
            tag["Septima"] = Septima.Name;
        }

        public override void OnEnterWorld(Player player)
        {
            activeSlot = new List<string>() { "", "", "", "" };
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (KeybindSystem.primaryAbility.JustPressed)
            {
                isUsingPrimaryAbility = true;
                Main.NewText("Septima: " + Septima.Name);
                Main.NewText("Anthem state: " + anthemState);
            }
            if (KeybindSystem.primaryAbility.JustReleased)
            {
                isUsingPrimaryAbility = false;
            }
            if (KeybindSystem.secondaryAbility.JustPressed)
            {
                isUsingSecondaryAbility = true;
                Main.NewText("Cooldown: " + timeSinceSecondary);
            }
            if (KeybindSystem.special1.JustPressed)
            {
                Special special1 = GetSpecial(activeSlot[0]);
                if (special1 != null && !special1.BeingUsed && abilityPower >= special1.ApUsage && !special1.InCooldown && (special1 != null || special1.Name != "") && !isUsingSpecialAbility)
                {
                    abilityPower -= (special1.ApUsage * APUsageModifier);
                    special1.SpecialTimer = 0;
                    if (special1.IsOffensive)
                    {
                        cantMove = true;
                    }
                }
            }
            if (KeybindSystem.special2.JustPressed)
            {
                Special special2 = GetSpecial(activeSlot[1]);
                if (special2 != null && !special2.BeingUsed && abilityPower >= special2.ApUsage && !special2.InCooldown && (special2 != null || special2.Name != "") && !isUsingSpecialAbility)
                {
                    abilityPower -= (special2.ApUsage * APUsageModifier);
                    special2.SpecialTimer = 0;
                    if (special2.IsOffensive)
                    {
                        cantMove = true;
                    }
                }
            }
            if (KeybindSystem.special3.JustPressed)
            {
                Special special3 = GetSpecial(activeSlot[2]);
                if (special3 != null && !special3.BeingUsed && abilityPower >= special3.ApUsage && !special3.InCooldown && (special3 != null || special3.Name != "") && !isUsingSpecialAbility)
                {
                    abilityPower -= (special3.ApUsage * APUsageModifier);
                    special3.SpecialTimer = 0;
                    if (special3.IsOffensive)
                    {
                        cantMove = true;
                    }
                }
            }
            if (KeybindSystem.special4.JustPressed)
            {
                Special special4 = GetSpecial(activeSlot[3]);
                if (special4 != null && !special4.BeingUsed && abilityPower >= special4.ApUsage && !special4.InCooldown && (special4 != null || special4.Name != "") && !isUsingSpecialAbility)
                {
                    abilityPower -= (special4.ApUsage * APUsageModifier);
                    special4.SpecialTimer = 0;
                    if (special4.IsOffensive)
                    {
                        cantMove = true;
                    }
                }
            }

            
        }

        public override void PostUpdateMiscEffects()
        {
            if (SeptimalPower <= 0 && timeSincePrimary <= 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust.NewDust(Player.position, 10, 10, DustID.Electric, 0, 0);
                }
            }
            if (isUsingPrimaryAbility && canUsePrimary)
            {
                Septima.FirstAbilityEffects();
            }
            if (secondaryInUse && canUseSecondary)
            {
                Septima.SecondAbilityEffects();
            }
            if (isUsingSpecialAbility)
            {
                for (int i = 0; i < activeSlot.Count; i++)
                {
                    GetSpecial(activeSlot[i])?.Effects();
                }
            }

            if (Septima.CanRecharge && rechargeComboCount != 0 && rechargeDelay == 0 && !isOverheated && !isUsingPrimaryAbility)
            {
                rechargeDelay = rechargeCooldown;
                rechargeTimer = rechargeDuration;
                // Insert visual effect
            }
            if (rechargeDelay > 0)
            {
                rechargeDelay--;
            }
            if (rechargeTimer > 0)
            {
                rechargeTimer--;
                SPRegenModifier = MaxSeptimalPower / 60;
                timeSincePrimary = 60;
                isRecharging = true;
            } else
            {
                isRecharging = false;
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
                Player.direction = Player.oldDirection;
            }
        }

        public override void PostUpdate()
        {
            FigureAvailability();
            UpdateLevelMultipliers();
            if (isUsingPrimaryAbility && canUsePrimary)
            {
                Septima.FirstAbility();
            }
            if (secondaryInUse && canUseSecondary)
            {
                Septima.SecondAbility();
            }
            if (isUsingSpecialAbility)
            {
                for (int i = 0; i < activeSlot.Count; i++)
                {
                    GetSpecial(activeSlot[i])?.Attack();
                }
            } else
            {
                cantMove = false;
            }
            UpdateSeptimalPower();
            maxEXP = (int)Math.Pow(level * 80, 1.47f);
            if (experience >= maxEXP)
            {
                level++;
                if (experience > maxEXP)
                {
                    extraEXP = experience - maxEXP;
                } 
                experience = 0;
            } else
            {
                experience += extraEXP;
                extraEXP = 0;
            }
            Septima.Updates();
            Septima.MiscEffects();

        }

        private void FigureAvailability()
        {
            if (!isRecharging)
            {
                if (!isOverheated && (!secondaryInUse || secondaryInCooldown) && !isUsingSpecialAbility)
                {
                    canUsePrimary = true;
                } else
                {
                    canUsePrimary = false;
                }

                if (!isUsingSpecialAbility && !secondaryInCooldown)
                {
                    canUseSecondary = true;
                }
                else
                {
                    canUseSecondary = false;
                }
            } else
            {
                canUsePrimary = false;
                canUseSecondary = false;
            }
        }

        public override void ResetEffects()
        {
            base.ResetEffects();
            ResetEquipMultipliers();
            ResetResources();
            if (Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[0] < 15 && !isUsingSpecialAbility)
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
            if ((hasMirrorShard || hasBattlePod || hasMusesPendant) && !anthemState)
            {
                int fullHealth = (Player.statLifeMax + Player.statLifeMax2);
                Player.statLife += fullHealth;
                Player.HealEffect(fullHealth);
                Player.AddBuff(ModContent.BuffType<AnthemBuff>(), 7200);
                anthemState = true;
                return false;
            }
            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            if (anthemState && (Septima.Name == "Azure Striker"  || Septima.Name == "Azure Thunderclap") && Main.CalculateDamagePlayersTake(damage, Player.statDefense) <= ((Player.statLifeMax + Player.statLifeMax2)/4))
            {
                Main.NewText("Prevasion");
                timeSincePrimary = 0;
                isRecharging = false;
                rechargeTimer = 0;
                Player.immune = true;
                Player.AddImmuneTime(cooldownCounter, 60);
                return false;
            }
            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, ref cooldownCounter);
        }

        public void UpdateLevelMultipliers()
        {
            primaryDamageLevelMult = (1 + level * 0.012f);
            secondaryDamageLevelMult = (1 + level * 0.01f);
            specialDamageLevelMult = (1 + level * 0.011f);
        }

        public void ResetEquipMultipliers()
        {
            primaryDamageEquipMult = 1;
            secondaryDamageEquipMult = 1;
            specialDamageEquipMult = 1;
        }

        public float SeptimalPowerToFraction()
        {
            return SeptimalPower / MaxSeptimalPower;
        }

        public float ExperienceToFraction()
        {
            float exp = experience;
            float maxExp = maxEXP;
            return exp / maxExp;
        }

        public void UpdateSeptimalPower()
        {
            if (SeptimalPower <= 0)
            {
                isOverheated = true;
            }
            if (SeptimalPower >= MaxSeptimalPower)
            {
                isOverheated = false;
            }
            if (timeSinceSecondary < Septima.SecondaryCooldownTime)
            {
                timeSinceSecondary++;
                secondaryInCooldown = true;
            }
            if (timeSinceSecondary >= Septima.SecondaryCooldownTime)
            {
                secondaryInCooldown = false;
            }
            if (abilityPower < maxAbilityPower)
            {
                abilityPower += (1f/4020f) * APRegenModifier;
            }
            UpdateSeptimaForFirst();
            UpdateSeptimaForSecond();
            UpdateSeptimaForSpecial();
        }

        public void ResetResources()
        {
            APUsageModifier = 1;
            SPUsageModifier = 1;
            APRegenModifier = 1;
            SPRegenModifier = 1;
            SPRegenOverheatModifier = 1;
        }

        public void UpdateSeptimaForFirst()
        {
            if (isUsingPrimaryAbility && !isOverheated && !(isUsingSecondaryAbility || isUsingSpecialAbility))
            {
                if (SeptimalPower > 0) SeptimalPower -= (Septima.SpUsage * SPUsageModifier);
                timeSincePrimary = 0;
            }
            if (!isUsingPrimaryAbility || isOverheated || isUsingSecondaryAbility || isUsingSpecialAbility) timeSincePrimary++;
            if (timeSincePrimary >= 60)
            {
                if (!isOverheated)
                {
                    SeptimalPower += (2 * SPRegenModifier);
                }
                else
                {
                    SeptimalPower += (1 * SPRegenOverheatModifier);
                }
                if (SeptimalPower > MaxSeptimalPower) SeptimalPower = MaxSeptimalPower;
                timeSincePrimary = 60;
            }
        }

        public void UpdateSeptimaForSecond()
        {
            if (isUsingSecondaryAbility)
            {
                if (!secondaryInCooldown)
                {
                    secondaryInUse = true;
                } else
                {
                    if (timeSinceSecondary > Septima.SecondaryCooldownTime) timeSinceSecondary = Septima.SecondaryCooldownTime;
                    isUsingSecondaryAbility = false;
                }
            } else
            {
                if (timeSinceSecondary > Septima.SecondaryCooldownTime) timeSinceSecondary = Septima.SecondaryCooldownTime;
            }
        }

        public void UpdateSeptimaForSpecial()
        {
            foreach (Special special in Septima.Abilities)
            {
                special.Update();
            }
            if (isUsingSpecialAbility)
            {
                Player.immune = true;
            }
        }

        public Septima GetSeptima(int choice)
        {
            return choice switch
            {
                0 => new AzureStriker(this, Player),
                1 => new AzureThunderclap(this, Player),
                _ => new AzureStriker(this, Player),
            };
        }

        public Septima GetSeptima(string name)
        {
            return name switch
            {
                "Azure Striker" => new AzureStriker(this, Player),
                "Azure Thunderclap" => new AzureThunderclap(this, Player),
                _ => new AzureStriker(this, Player),
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