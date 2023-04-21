using gvmod.Content.Buffs;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace gvmod.Common.Players.Septimas.Skills
{
    public class InfiniteSurge : Special
    {
        public InfiniteSurge(Player player, AdeptPlayer adept, string type) : base(player, adept, type)
        {
            ApUsage = 2;
            SpecialCooldownTime = 600;
            CooldownTimer = SpecialCooldownTime;
            BeingUsed = false;
            SpecialTimer = 1;
            SpecialDuration = 60;
        }

        public override int UnlockLevel => 27;

        public override bool IsOffensive => false;

        public override bool GivesIFrames => true;

        public override string Name => "Infinite Surge";

        public override void Effects()
        {
            if (BeingUsed)
            {
                Dust.NewDust(Player.Center, 10, 10, DustID.BlueTorch);
            }
        }

        public override void Attack()
        {
            if (BeingUsed)
            {
                Player.AddBuff(ModContent.BuffType<InfiniteSurgeBuff>(), 1800);
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
