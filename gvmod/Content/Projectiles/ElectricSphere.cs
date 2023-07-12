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
            Player player = Main.player[Projectile.owner];
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();

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

            switch (Projectile.ai[1])
            {
                case 1:
                    if (adept.IsUsingSpecialAbility)
                    {
                        Projectile.timeLeft = 2;
                        Projectile.penetrate = 2;
                    }
                    position = position.RotatedBy(MathHelper.ToRadians(rotation), axis);
                    Projectile.Center = position;
                    break;
                case 3:
                    if (adept.IsUsingPrimaryAbility && adept.CanUsePrimary)
                    {
                        Projectile.timeLeft = 2;
                        Projectile.penetrate = 2;
                    }
                    SimpleMovementAI();
                    break;
                default:
                    counting++;
                    if ((state == 2 && counting >= 60) || (counting >= 120))
                    {
                        state++;
                        counting = 0;
                    }
                    position = position.RotatedBy(MathHelper.ToRadians(rotation), axis);
                    Projectile.Center = position;
                    MovementAI();
                    break;
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];
            state = 1;
            counting = 0;
            if (Projectile.ai[1] >= 2)
            {
                Projectile.penetrate = -1;
                Projectile.timeLeft = 300;
            }
            switch (Projectile.ai[2])
            {
                case 1:
                    truePosition = player.Center + new Vector2(128);
                    break;
                case 2:
                    truePosition = player.Center + new Vector2(128).RotatedBy(MathHelper.ToRadians(120));
                    break;
                case 3:
                    truePosition = player.Center + new Vector2(128).RotatedBy(MathHelper.ToRadians(-120));
                    break;
            }
            base.OnSpawn(source);
            axis = target = player.Center;
            position = truePosition = Projectile.Center;
        }

        private void SimpleMovementAI()
        {
            Player player = Main.player[Projectile.owner];
            switch (Projectile.ai[2])
            {
                case 1:
                    truePosition = player.Center + new Vector2(128).RotatedBy(MathHelper.ToRadians(3.5f * counting)); ;
                    break;
                case 2:
                    truePosition = player.Center + new Vector2(128).RotatedBy(MathHelper.ToRadians(120 + (3.5f * counting)));
                    break;
                case 3:
                    truePosition = player.Center + new Vector2(128).RotatedBy(MathHelper.ToRadians(-120 + (3.5f * counting)));
                    break;
            }
            Main.NewText("Projectile position: " + Projectile.Center);
            Main.NewText("True position: " + truePosition);
            Projectile.Center = truePosition;
            counting++;
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
