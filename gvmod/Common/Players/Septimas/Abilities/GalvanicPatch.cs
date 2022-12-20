using Terraria;
using Terraria.ID;

namespace gvmod.Common.Players.Septimas.Abilities
{
    internal class GalvanicPatch : Special
    {
        public GalvanicPatch(Player player, AdeptPlayer adept) : base(player, adept)
        {
            ApUsage = 1;
            SpecialCooldownTime = 600;
            CooldownTimer = SpecialCooldownTime;
            BeingUsed = false;
            SpecialTimer = 1;
            SpecialDuration = 25;
        }

        public override int UnlockLevel => 3;

        public override bool IsOffensive => false;

        public override bool GivesIFrames => true;

        public override string Name => "Galvanic Patch";

        public override void Effects()
        {
            if (BeingUsed)
            {
                Dust.NewDust(Player.Center, 10, 10, DustID.GreenTorch);
            }
        }

        public override void Attack()
        {
            float maxLife = Player.statLifeMax;
            float denominator = SpecialDuration;
            float ammount = maxLife / (4 * denominator);
            if (BeingUsed)
            {
                Main.NewText("Healing: " + ammount);
                Main.NewText("Max health: " + maxLife);
                Main.NewText("Denominator: " + denominator);
                Player.Heal((int)ammount);
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
                Adept.isUsingSpecialAbility = true;
                SpecialTimer++;
                if (SpecialTimer >= SpecialDuration)
                {
                    BeingUsed = false;
                    Adept.isUsingSpecialAbility = false;
                }
            }
        }
    }
}
