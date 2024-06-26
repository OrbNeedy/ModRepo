﻿using gvmod.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Common.Players.Septimas.Skills
{
    public class SeptimalShield : Special
    {
        public SeptimalShield(Player player, AdeptPlayer adept, string type) : base(player, adept, type)
        {
            ApUsage = 1;
            SpecialCooldownTime = 600;
            CooldownTimer = SpecialCooldownTime;
            BeingUsed = false;
            SpecialTimer = 1;
            SpecialDuration = 60;
        }

        public override int UnlockLevel => 40;

        public override bool IsOffensive => false;

        public override bool StayInPlace => false;

        public override bool GivesIFrames => true;

        public override string Name => "Septimal Shield";

        public override void Effects()
        {
            if (BeingUsed)
            {
                Dust.NewDust(Player.Center, 10, 10, DustID.IceTorch);
            }
        }

        public override void Attack()
        {
            if (BeingUsed)
            {
                Player.AddBuff(ModContent.BuffType<SeptimalShieldBuff>(), 1800);
            }
        }

        public override void Update()
        {
            if (CooldownTimer < SpecialCooldownTime)
            {
                CooldownTimer++;
                InCooldown = true;
            }
            if (CooldownTimer >= SpecialCooldownTime)
            {
                InCooldown = false;
            }
            if (SpecialTimer == 0 && !InCooldown)
            {
                CooldownTimer = 0;
                BeingUsed = true;
            }
            if (BeingUsed)
            {
                Adept.IsUsingSpecialAbility = true;
                SpecialTimer++;
                if (SpecialTimer >= SpecialDuration)
                {
                    BeingUsed = false;
                    Adept.IsUsingSpecialAbility = false;
                }
            }
        }
    }
}
