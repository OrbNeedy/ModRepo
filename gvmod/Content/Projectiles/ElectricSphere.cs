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
        private int origin;
        private int state;
        private int counting;
        private Vector2 axis;
        private Vector2 position;
        private Vector2 target;
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

            float rotation = 3.5f;
            if (adept.PowerLevel >= 2 && adept.Septima is AzureThunderclap)
            {
                rotation = 3.5f;
            }
            if (Projectile.ai[1] >= 0)
            {
                position = position.RotatedBy(MathHelper.ToRadians(rotation), axis);
            }

            switch (Projectile.ai[1])
            {
                case 1:
                    if (adept.IsUsingSpecialAbility)
                    {
                        Projectile.timeLeft = 2;
                        Projectile.penetrate = 2;
                    }
                    break;
                case 2:
                    counting++;
                    if ((state == 2 && counting >= 60) || (counting >= 120))
                    {
                        state++;
                        counting = 0;
                    }
                    MovementAI();
                    break;
                case 3:
                    counting++;
                    if ((state == 2 && counting >= 60) || (counting >= 120))
                    {
                        state++;
                        counting = 0;
                    }
                    MovementAI();
                    break;
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            Main.NewText("Hello there");
            state = 1;
            counting = 0;
            if (Projectile.ai[1] >= 2)
            {
                Projectile.penetrate = -1;
                Projectile.timeLeft = 300;
            }
            base.OnSpawn(source);
            Player player = Main.player[Projectile.owner];
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            if (source == player.GetSource_FromThis())
            {
                if (adept.Septima is AzureStriker || adept.Septima is AzureThunderclap)
                {
                    if (!adept.IsUsingSpecialAbility)
                    {
                        origin = 1;
                    } else 
                    {
                        origin = 2;
                    }
                } else
                {
                    origin = 0;
                }
            }
            axis = target = player.Center;
            position = Projectile.Center;
        }

        private void MovementAI()
        {
            if (state == 2)
            {
                if (counting <= 1)
                {
                    target = Main.MouseWorld;
                }
                if (axis.Distance(target) > 10)
                {
                    axis += axis.DirectionTo(target).SafeNormalize(Vector2.Zero) * 12;
                    position += axis.DirectionTo(target).SafeNormalize(Vector2.Zero) * 12;
                }
            }
            if (state == 3)
            {
                position += position.DirectionFrom(axis).SafeNormalize(Vector2.Zero) * 6;
            }
            Projectile.Center = position;
        }
    }
}
