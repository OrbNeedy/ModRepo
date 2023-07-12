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
        private int state;
        private int counting;
        private Vector2 axis;
        private Vector2 truePosition;
        private Vector2 position;
        private Vector2 target;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Electric Sphere");
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
            switch (Projectile.ai[0])
            {
                case -1:
                    Projectile.penetrate = 2;
                    break;
                default:
                    Projectile.penetrate = -1;
                    break;
            }

            position = position.RotatedBy(MathHelper.ToRadians(rotation), axis);

            switch (Projectile.ai[1])
            {
                case 1:
                    if (adept.IsUsingSpecialAbility)
                    {
                        Projectile.timeLeft = 2;
                        Projectile.penetrate = 2;
                    }
                    break;
                default:
                    counting++;
                    if ((state == 2 && counting >= 60) || (counting >= 120))
                    {
                        state++;
                        counting = 0;
                    }
                    MovementAI();
                    break;
            }
            Projectile.Center = position;
        }

        public override void OnSpawn(IEntitySource source)
        {
            state = 1;
            counting = 0;
            if (Projectile.ai[1] >= 2)
            {
                Projectile.penetrate = -1;
                Projectile.timeLeft = 300;
            }
            base.OnSpawn(source);
            Player player = Main.player[Projectile.owner];
            axis = target = player.Center;
            position = truePosition = Projectile.Center;
        }

        private void SimpleMovementAI()
        {
            Player player = Main.player[Projectile.owner];
            target = player.Center;

            if (axis.Distance(target) > 10)
            {
                axis += axis.DirectionTo(target).SafeNormalize(Vector2.Zero) * 10;
                position += axis.DirectionTo(target).SafeNormalize(Vector2.Zero) * 10;
            }
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
        }
    }
}
