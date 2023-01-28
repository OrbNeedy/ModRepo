using gvmod.Common.Configs.CustomDataTypes;
using gvmod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace gvmod.Common.Players.Septimas.Abilities
{
    public class VoltaicChains : Special
    {
        private int firstChain;
        private bool isNotFirstChain = false;
        private int baseDamage;
        private int maxExtend;
        private int extend;
        private Vector2 lastPlayerPos = new Vector2(0);
        public VoltaicChains(Player player, AdeptPlayer adept, string type) : base(player, adept, type)
        {
            ApUsage = 3;
            SpecialCooldownTime = 600;
            CooldownTimer = SpecialCooldownTime;
            BeingUsed = false;
            SpecialTimer = 1;
            SpecialDuration = 300;
            extend = 3;
            maxExtend = 3;
            baseDamage = 50;
        }

        public override int UnlockLevel => 40;

        public override bool IsOffensive => true;

        public override bool GivesIFrames => true;

        public override string Name => "Voltaic Chains";

        public override void Attack()
        {
            if (BeingUsed)
            {
                if (Type == "S")
                {
                    TypeSAttack();
                }
                if (Type == "T")
                {
                    TypeTAttack();
                }
                if (SpecialTimer % 20 == 0 && SpecialTimer <= 210)
                {
                    ChainPositions positions = new ChainPositions(Player);
                    if (isNotFirstChain)
                    {
                        // Pass the IEntitySource from the first chain as a parameter to ai0 or ai1
                        int firstChainSource = Main.projectile[firstChain].whoAmI;
                        Projectile.NewProjectile(Player.GetSource_FromThis(), positions.startingPosition, positions.GetVelocity(), ModContent.ProjectileType<ChainTip>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 0, Player.whoAmI, firstChainSource, 0);
                    }
                    else
                    {
                        // Save the IEntitySource from the first chain here
                        firstChain = Projectile.NewProjectile(Player.GetSource_FromThis(), positions.startingPosition, positions.GetVelocity(), ModContent.ProjectileType<ChainTip>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 0, Player.whoAmI, 0, 1);
                        isNotFirstChain = true;
                    }
                }
            }
        }

        public override void Effects()
        {
        }

        public override void Update()
        {
            if (!BeingUsed)
            {
                VelocityMultiplier = new Vector2(1f, 1f);
                isNotFirstChain = false;
                lastPlayerPos = Player.Center;
                extend = maxExtend;
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

        private void TypeSAttack()
        {
            switch (Adept.PowerLevel)
            {
                case 1:
                    baseDamage = 50;
                    break;
                case 2:
                    baseDamage = 75;
                    if (SpecialTimer == 275)
                    {
                        for (int i = -8; i < 9; i++)
                        {
                            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(64 * i, 0f), Vector2.Zero, ModContent.ProjectileType<Thunder>(), (int)(175 * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 10f, Player.whoAmI);
                        }
                    }
                    break;
            }
        }

        private void TypeTAttack()
        {
            switch (Adept.PowerLevel)
            {
                case 1:
                    baseDamage = 50;
                    break;
                case 2:
                    baseDamage = 65;
                    if (SpecialTimer == 299 && extend > 0)
                    {
                        SpecialTimer = 0;
                        extend--;
                        isNotFirstChain = false;
                    }
                    break;
            }
        }
    }
}
