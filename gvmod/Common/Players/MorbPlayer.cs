using Terraria;
using Terraria.ModLoader;

namespace gvmod.Common.Players
{
    public class MorbPlayer : ModPlayer
    {
        public bool FateFetus { get; set; }
        public int AttackTimer { get; set; }

        public override void Initialize()
        {
            FateFetus = false;
        }

        public override void ResetEffects()
        {
            FateFetus = false;
        }

        public override void PostUpdate()
        {
            if (AttackTimer > 0) AttackTimer--;
            AdeptPlayer adept = Player.GetModPlayer<AdeptPlayer>();
            if (FateFetus)
            {
                adept.SPRegenOverheatModifier -= 0.5f;
                if (!adept.IsOverheated)
                {
                    adept.SeptimalPower += 0.5f;
                } else
                {
                    Player.statDefense *= 0.5f;
                    Player.endurance *= 0.8f;
                }
                TheAttackInQuestion();
            }
        }

        public void TheAttackInQuestion()
        {
            AdeptPlayer adept = Player.GetModPlayer<AdeptPlayer>();
            if (AttackTimer > 0)
            {
                adept.Septima.MorbAttack(AttackTimer);
            }
        }
    }
}
