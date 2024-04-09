using gvmod.Content.Buffs;
using gvmod.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Common.Players.Septimas.Skills
{
    public class SplitSecond : Special
    {
        public SplitSecond(Player player, AdeptPlayer adept, string type) : base(player, adept, type)
        {
            ApUsage = 1;
            SpecialCooldownTime = 600;
            CooldownTimer = SpecialCooldownTime;
            BeingUsed = false;
            SpecialTimer = 1;
            SpecialDuration = 30;
        }

        public override int UnlockLevel => 18;

        public override string Name => "Split Second";

        public override bool IsOffensive => false;

        public override bool StayInPlace => false;

        public override bool GivesIFrames => true;

        public override void Attack()
        {
            float maxSP = Adept.MaxSeptimalPower + Adept.MaxSeptimalPower2;
            float denominator = SpecialDuration;
            float ammount = maxSP / denominator;
            if (ammount <= 0)
            {
                ammount = 1;
            }
            if (BeingUsed)
            {
                Player.ClearBuff(ModContent.BuffType<Chaff>());
                Adept.TimeSincePrimary = 0;
                Adept.IsOverheated = false;
                Adept.SeptimalPower += ammount;
            }
        }

        public override void Effects()
        {
            if (BeingUsed)
            {
                Dust.NewDust(Player.Center, 10, 10, DustID.IceTorch);
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
