﻿using gvmod.Content.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace gvmod.Common.Players.Septimas.Skills
{
    public class SeptimalBurst : Special
    {
        public SeptimalBurst(Player player, AdeptPlayer adept, string type) : base(player, adept, type)
        {
            ApUsage = 1;
            SpecialCooldownTime = 600;
            CooldownTimer = SpecialCooldownTime;
            BeingUsed = false;
            SpecialTimer = 1;
            SpecialDuration = 60;
        }

        public override int UnlockLevel => 10;

        public override bool IsOffensive => false;

        public override bool StayInPlace => true;

        public override bool GivesIFrames => true;

        public override string Name => "Septimal Burst";

        public override void Effects()
        {
            if (BeingUsed)
            {
                Dust.NewDust(Player.Center, 10, 10, DustID.PurpleTorch);
            }
        }

        public override void Attack()
        {
            if (BeingUsed)
            {
                Player.endurance += 1f;
                Player.AddBuff(ModContent.BuffType<SeptimalBurstBuff>(), 1800);
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
