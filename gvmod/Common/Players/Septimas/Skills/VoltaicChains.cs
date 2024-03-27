using gvmod.Common.Configs.CustomDataTypes;
using gvmod.Content.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace gvmod.Common.Players.Septimas.Skills
{
    public class VoltaicChains : Special
    {
        private int firstChain;
        private bool isNotFirstChain = false;
        private int baseDamage;
        private int maxExtend;
        private int extend;
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

        public override bool StayInPlace => true;

        public override bool GivesIFrames => true;

        public override string Name => "Voltaic Chains";

        public override void Attack()
        {
            if (BeingUsed)
            {
                if (Type == "S")
                {
                    if (!TypeSAttack())
                    {
                        return;
                    }
                }
                if (Type == "T")
                {
                    if (!TypeTAttack())
                    {
                        return;
                    }
                }
                if (SpecialTimer % 20 == 0 && SpecialTimer <= 210)
                {
                    ChainPositions positions = new ChainPositions(Player);
                    if (isNotFirstChain)
                    {
                        int firstChainSource = Main.projectile[firstChain].whoAmI;
                        Projectile.NewProjectile(Player.GetSource_FromThis(), positions.StartingPosition, positions.GetVelocity(), ModContent.ProjectileType<ChainTip>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 0, Player.whoAmI, firstChainSource, 0);
                    }
                    else
                    {
                        firstChain = Projectile.NewProjectile(Player.GetSource_FromThis(), positions.StartingPosition, positions.GetVelocity(), ModContent.ProjectileType<ChainTip>(), (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 0, Player.whoAmI, 0, 1);
                        isNotFirstChain = true;
                    }
                }
            }
        }

        public override void MoveOverride()
        {
            VelocityMultiplier = new Vector2(0f, 0.00001f);
            Player.slowFall = true;
        }

        public override void Update()
        {
            if (!BeingUsed)
            {
                isNotFirstChain = false;
                extend = maxExtend;
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
        }

        private bool TypeSAttack()
        {
            switch (Adept.PowerLevel)
            {
                case 1:
                    baseDamage = 50;
                    break;
                case 2:
                case 3:
                    baseDamage = 75;
                    if (extend < 2)
                    {
                        if (SpecialTimer >= 299)
                        {
                            extend--;
                            SpecialTimer = 238;
                        }
                    } else
                    {
                        if (SpecialTimer == 299)
                        {
                            for (int i = -9; i < 10; i++)
                            {
                                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(68 * i, 0f), Vector2.Zero, ModContent.ProjectileType<Thunder>(), (int)(175 * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 10f, Player.whoAmI);
                            }
                        }
                    }
                    break;
            }
            return true;
        }

        private bool TypeTAttack()
        {
            switch (Adept.PowerLevel)
            {
                case 1:
                    baseDamage = 50;
                    break;
                case 2:
                    baseDamage = 65;
                    if (SpecialTimer == (SpecialDuration - 1) && extend > 0)
                    {
                        SpecialTimer = 0;
                        extend--;
                        isNotFirstChain = false;
                    }
                    break;
                case 3:
                    baseDamage = 85;
                    if (SpecialTimer == 1)
                    {
                        ChainPositions temporaryChainPositions = new ChainPositions(Player);
                        int maxX = 950;
                        int maxY = 440;
                        List<Vector2> positions = new() { new(-maxX, -maxY), new(maxX, -maxY), new(maxX, maxY), 
                            new(-maxX, maxY) };
                        for (int i = 0; i < 4; i++)
                        {
                            Vector2 start = positions[i];
                            Vector2 end;
                            if (i+1 >= positions.Count)
                            {
                                end = positions[0];
                            } else
                            {
                                end = positions[i+1];
                            }
                            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + start,
                                temporaryChainPositions.GetVelocity(start, end),
                                ModContent.ProjectileType<ChainTip>(),
                                (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 
                                0, Player.whoAmI, 420 - SpecialTimer, 2, 40);
                        }
                    }
                    if (SpecialTimer % 60 == 0 && SpecialTimer <= 210)
                    {
                        ChainPositions temporaryChainPositions = new ChainPositions(Player);
                        List<(Vector2, Vector2)> positions = temporaryChainPositions.
                            SpontaneousPositionObtainer(Main.MouseWorld);
                        foreach ((Vector2, Vector2) position in positions)
                        {
                            Projectile.NewProjectile(Player.GetSource_FromThis(), position.Item1,
                                temporaryChainPositions.GetVelocity(position.Item1, position.Item2),
                                ModContent.ProjectileType<ChainTip>(),
                                (int)(baseDamage * Adept.SpecialDamageLevelMult * Adept.SpecialDamageEquipMult), 0,
                                Player.whoAmI, 420 - SpecialTimer, 2, 60);
                        }
                    }
                    return false;
            }
            return true;
        }
    }
}
