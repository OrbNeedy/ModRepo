using gvmod.Common.Players;
using gvmod.Common.Players.Septimas;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace gvmod.Content.Projectiles
{
    internal class ElectricSphere : ModProjectile
    {
        private int aiForm = 1;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Electric Sphere");
        }

        public override void SetDefaults()
        {
            Projectile.damage = 60;
            Projectile.knockBack = 10;
            Projectile.Size = new Vector2(42);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.timeLeft = 2;
            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.ownerHitCheck = false;
            Projectile.light = 1f;
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
                    if (adept.IsUsingSpecialAbility)
                    {
                        Projectile.timeLeft = 2;
                        Projectile.Center = Projectile.Center.RotatedBy(MathHelper.ToRadians(3.5f), Main.player[Projectile.owner].Center);
                    }
                    break;
                default:
                    if (adept.IsUsingPrimaryAbility && adept.CanUsePrimary)
                    {
                        Projectile.timeLeft = 2;
                    }
                    break;
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            Player player = Main.player[Projectile.owner];
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            if (source == player.GetSource_FromThis())
            {
                if (adept.Septima is AzureStriker || adept.Septima is AzureThunderclap)
                {
                    if (!adept.IsUsingSpecialAbility)
                    {
                        aiForm = 1;
                    } else 
                    {
                        aiForm = 2;
                    }
                } else
                {
                    aiForm = 0;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }
    }
}
