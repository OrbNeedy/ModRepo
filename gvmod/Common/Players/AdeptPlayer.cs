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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace gvmod.Common.Players
{
    public class AdeptPlayer : ModPlayer
    {
        public Septima septima;
        public float maxSeptimalPower;
        public float septimalPower;
        public float maxAbilityPower;
        public float abilityPower;
        public int level;
        public int experience;
        public int maxEXP;
        public int extraEXP;
        public List<string> activeSlot;

        public int rechargeComboCount;
        public const int rechargeCooldown = 60;
        public const int rechargeDuration = 30;
        public int rechargeDelay = 0;
        public int rechargeTimer = 0;
        public bool recharging = false;

        public float primaryDamageLevelMult;
        public float secondaryDamageLevelMult;
        public float specialDamageLevelMult;

        public float primaryDamageEquipMult;
        public float secondaryDamageEquipMult;
        public float specialDamageEquipMult;

        public float APUsageModifier;
        public float SPUsageModifier;
        public float APRegenModifier;
        public float SPRegenModifier;
        public float SPRegenOverheatModifier;

        public bool hasMirrorShard;
        public bool hasBattlePod;
        public bool hasMusesPendant;
        public bool anthemState;

        public bool isUsingPrimaryAbility;
        public bool isUsingSecondaryAbility;
        public bool isUsingSpecialAbility;
        public bool isRecharging;

        public bool secondaryInUse = false;
        public int timeSinceSecondary = 0;
        public bool secondaryInCooldown = false;
        public bool isOverheated = false;
        public int timeSincePrimary = 0;

        public override void Initialize()
        {
            base.Initialize();
            maxSeptimalPower = 300;
            septimalPower = maxSeptimalPower;
            maxAbilityPower = 3;
            abilityPower = maxAbilityPower;
            septima = GetSeptima(Main.rand.Next(2));
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
                septima = GetSeptima(tag.GetString("Septima"));
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag["Level"] = level;
            tag["Experience"] = experience;
            tag["Septima"] = septima.Name;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (KeybindSystem.primaryAbility.JustPressed)
            {
                isUsingPrimaryAbility = true;
                Main.NewText("Septima: " + septima.Name);
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
                if (!special1.BeingUsed && abilityPower >= special1.ApUsage && !special1.InCooldown && (special1 != null || special1.Name != "") && !isUsingSpecialAbility)
                {
                    abilityPower -= (special1.ApUsage * APUsageModifier);
                    special1.SpecialTimer = 0;
                }
            }
            if (KeybindSystem.special2.JustPressed)
            {
                Special special2 = GetSpecial(activeSlot[1]);
                if (!special2.BeingUsed && abilityPower >= special2.ApUsage && !special2.InCooldown && (special2 != null || special2.Name != "") && !isUsingSpecialAbility)
                {
                    abilityPower -= (special2.ApUsage * APUsageModifier);
                    special2.SpecialTimer = 0;
                }
            }
            if (KeybindSystem.special3.JustPressed)
            {
                Special special3 = GetSpecial(activeSlot[2]);
                if (!special3.BeingUsed && abilityPower >= special3.ApUsage && !special3.InCooldown && (special3 != null || special3.Name != "") && !isUsingSpecialAbility)
                {
                    abilityPower -= (special3.ApUsage * APUsageModifier);
                    special3.SpecialTimer = 0;
                }
            }
            if (KeybindSystem.special4.JustPressed)
            {
                Special special4 = GetSpecial(activeSlot[3]);
                if (!special4.BeingUsed && abilityPower >= special4.ApUsage && !special4.InCooldown && (special4 != null || special4.Name != "") && !isUsingSpecialAbility)
                {
                    abilityPower -= (special4.ApUsage * APUsageModifier);
                    special4.SpecialTimer = 0;
                }
            }

            
        }

        public override void PostUpdateMiscEffects()
        {
            if (septimalPower <= 0 && timeSincePrimary <= 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust.NewDust(Player.position, 10, 10, DustID.Electric, 0, 0);
                }
            }
            if (isUsingPrimaryAbility && !isOverheated && !isUsingSpecialAbility && !isRecharging)
            {
                septima.FirstAbilityEffects();
            }
            if (secondaryInUse && !isUsingSpecialAbility && !isRecharging)
            {
                septima.SecondAbilityEffects();
            }
            if (isUsingSpecialAbility && !isRecharging)
            {
                for (int i = 0; i < activeSlot.Count; i++)
                {
                    GetSpecial(activeSlot[i])?.Effects();
                }
            }

            if (septima.CanRecharge && rechargeComboCount != 0 && rechargeDelay == 0 && !isOverheated)
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
                SPRegenModifier = maxSeptimalPower / 60;
                timeSincePrimary = 60;
                isRecharging = true;
            } else
            {
                isRecharging = false;
            }
        }

        public override void SetControls()
        {
            if (isUsingSpecialAbility)
            {
                Player.controlJump = false;
                Player.controlLeft = false;
                Player.controlRight = false;
                Player.controlDown = false;
            }
        }

        public override void PostUpdate()
        {
            UpdateLevelMultipliers();
            if (isUsingPrimaryAbility && !isOverheated && !isUsingSpecialAbility)
            {
                septima.FirstAbility();
            }
            if (secondaryInUse && !isUsingSpecialAbility)
            {
                septima.SecondAbility();
            }
            if (isUsingSpecialAbility)
            {
                for (int i = 0; i < activeSlot.Count; i++)
                {
                    GetSpecial(activeSlot[i])?.Attack();
                }
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
            septima.Updates();
            septima.MiscEffects();
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
            if (anthemState && (septima.Name == "Azure Striker"  || septima.Name == "Azure Thunderclap") && Main.CalculateDamagePlayersTake(damage, Player.statDefense) <= ((Player.statLifeMax + Player.statLifeMax2)/4))
            {
                Main.NewText("Prevasion");
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
            return septimalPower / maxSeptimalPower;
        }

        public float ExperienceToFraction()
        {
            float exp = experience;
            float maxExp = maxEXP;
            return exp / maxExp;
        }

        public void UpdateSeptimalPower()
        {
            if (septimalPower <= 0)
            {
                isOverheated = true;
            }
            if (septimalPower >= maxSeptimalPower)
            {
                isOverheated = false;
            }
            if (timeSinceSecondary < septima.SecondaryCooldownTime)
            {
                timeSinceSecondary++;
                secondaryInCooldown = true;
            }
            if (timeSinceSecondary >= septima.SecondaryCooldownTime)
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
                if (septimalPower > 0) septimalPower -= (septima.SpUsage * SPUsageModifier);
                timeSincePrimary = 0;
            }
            if (!isUsingPrimaryAbility || isOverheated || isUsingSecondaryAbility || isUsingSpecialAbility) timeSincePrimary++;
            if (timeSincePrimary >= 60)
            {
                if (!isOverheated)
                {
                    septimalPower += (2 * SPRegenModifier);
                }
                else
                {
                    septimalPower += (1 * SPRegenOverheatModifier);
                }
                if (septimalPower > maxSeptimalPower) septimalPower = maxSeptimalPower;
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
                    if (timeSinceSecondary > septima.SecondaryCooldownTime) timeSinceSecondary = septima.SecondaryCooldownTime;
                    isUsingSecondaryAbility = false;
                }
            } else
            {
                if (timeSinceSecondary > septima.SecondaryCooldownTime) timeSinceSecondary = septima.SecondaryCooldownTime;
            }
        }

        public void UpdateSeptimaForSpecial()
        {
            foreach (Special special in septima.Abilities)
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
            for (int i = 0; i < septima.Abilities.Count; i++)
            {
                Special temp = septima.Abilities[i];
                if (temp.Name == name)
                {
                    return temp;
                }
            }
            return null;
        }
    }
}