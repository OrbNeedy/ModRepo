using gvmod.Common.Players;
using gvmod.Common.Players.Septimas;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace gvmod.Content.Projectiles
{
    internal class Shock : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shock");
        }

        public override void SetDefaults()
        {
            Projectile.damage = 20;
            Projectile.knockBack = 10;
            Projectile.Size = new Vector2(42);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.timeLeft = 2;
            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.ownerHitCheck = false;
        }

        public override void AI()
        {
            AdeptPlayer adept = Main.player[Projectile.owner].GetModPlayer<AdeptPlayer>();
            if (Projectile.ai[0] == -1)
            {
                Projectile.penetrate = 2;
            }

            switch (Projectile.ai[1])
            {
                case 1:
                    if (adept.isUsingSpecialAbility)
                    {
                        Projectile.timeLeft = 2;
                    }
                    break;
                default:
                    if (adept.isUsingPrimaryAbility && adept.canUsePrimary)
                    {
                        Projectile.timeLeft = 2;
                    }
                    break;
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.ai[0] == 10)
            {
                Projectile.timeLeft = 10;
            }
        }
    }
}
