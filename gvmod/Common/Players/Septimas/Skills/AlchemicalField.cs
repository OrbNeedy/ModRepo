using gvmod.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Common.Players.Septimas.Skills
{
    public class AlchemicalField : Special
    {
        public AlchemicalField(Player player, AdeptPlayer adept, string type) : base(player, adept, type)
        {
            ApUsage = 1;
            SpecialCooldownTime = 600;
            CooldownTimer = SpecialCooldownTime;
            BeingUsed = false;
            SpecialTimer = 1;
            SpecialDuration = 60;
        }

        public override int UnlockLevel => 24;

        public override bool IsOffensive => false;

        public override bool StayInPlace => false;

        public override bool GivesIFrames => true;

        public override string Name => "Alchemical Field";

        public override void Effects()
        {
            if (BeingUsed)
            {
                Dust.NewDust(Player.Center, 10, 10, DustID.YellowTorch);
            }
        }

        public override void Attack()
        {
            if (BeingUsed)
            {
                Player.AddBuff(ModContent.BuffType<AlchemicalFieldBuff>(), 1800);
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
