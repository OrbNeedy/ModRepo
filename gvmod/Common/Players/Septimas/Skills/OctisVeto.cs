using gvmod.Common.Configs.CustomDataTypes;
using gvmod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace gvmod.Common.Players.Septimas.Skills
{
    public class OctisVeto : Special
    {
        ChainPositions octis;
        int intervalCounter = 0;
        static int chainInterval = 30;
        int positionCounter = 0;
        private Vector2 lastPlayerPos = new Vector2(0);
        public OctisVeto(Player player, AdeptPlayer adept, string type) : base(player, adept, type)
        {
            ApUsage = 3;
            SpecialCooldownTime = 1200;
            CooldownTimer = SpecialCooldownTime;
            BeingUsed = false;
            SpecialTimer = 1;
            SpecialDuration = 900;
            octis = new ChainPositions(player);
        }

        public override int UnlockLevel => 80;

        public override string Name => "Octis Veto";

        public override bool IsOffensive => true;

        public override bool GivesIFrames => false;

        public override void Attack()
        {
            if (BeingUsed)
            {
                if (intervalCounter == chainInterval && SpecialTimer <= 250)
                {
                    (Vector2, Vector2) currentPositions = octis.OctisPositions[positionCounter];
                    Projectile.NewProjectile(Player.GetSource_FromThis(), currentPositions.Item1, octis.GetVelocity(currentPositions.Item1, currentPositions.Item2), ModContent.ProjectileType<ChainTip>(), (int)(200 * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 8, Player.whoAmI, SpecialDuration - SpecialTimer, 2, 290 - SpecialTimer);
                    intervalCounter = 0;
                    positionCounter++;
                }
                if (SpecialTimer == 290)
                {
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(0, 0), ModContent.ProjectileType<ElectricPilar>(), (int)(180 * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 8, Player.whoAmI, -1);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(0, 0), ModContent.ProjectileType<ElectricPilar>(), (int)(180 * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 8, Player.whoAmI, 1);
                }
                intervalCounter++; 
            }
        }

        public override void Update()
        {
            if (!BeingUsed)
            {
                VelocityMultiplier = new Vector2(1f, 1f);
                lastPlayerPos = Player.Center;
                intervalCounter = 0;
                positionCounter = 0;
                octis = new ChainPositions(Player);
            }
            else
            {
                VelocityMultiplier *= 0f;
                Player.Center = lastPlayerPos;
                Player.slowFall = true;
            }
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
            Player.velocity *= VelocityMultiplier;
        }
    }
}
