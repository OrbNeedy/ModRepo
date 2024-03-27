using gvmod.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace gvmod.Content.Projectiles
{
    public class FlashfieldStriker : ModProjectile
    {
        private bool keepOn;
        private int timer;
        private Vector2 positionOffset;
        private Vector2 position;
        private Vector2 target;

        // MP Issues
        // 1: Won't appear when using flashfield if another player is already using flashfield
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(352);
            Projectile.light = 1f;
            Projectile.scale = 1f;

            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.damage = 1;
            Projectile.knockBack = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.penetrate = -1;

            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 3;
            Projectile.ownerHitCheck = false;
            Projectile.alpha = 128;
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            keepOn = true;
            target = position = Main.player[Projectile.owner].Center;
            if (Projectile.ai[0] == 3) positionOffset = new Vector2(0, 1);
            else positionOffset = Vector2.Zero;

            // ai0 defines how it behaves
            // 0: Regular flashfield behaviour, being on as long as the player holds the primary key and follows it
            // 1: Altered from 0, it does the same, but only when the player holds the special key
            // 2: It is sent to the mouse position after a while
            // 3: It is assigned a rotation in ai1, then separates while rotating, and finally moves towards the center

            switch (Projectile.ai[1])
            {
                case 1:
                    positionOffset = positionOffset.RotatedBy(2.094395);
                    break;
                case 2:
                    positionOffset = positionOffset.RotatedBy(-2.094395);
                    break;
                default:
                    break;
            }

            // ai2 defines it's scale
            if (Projectile.ai[2] > 0) Projectile.scale = Projectile.ai[2];
        }

        public override void AI()
        {
            if (Projectile.ai[2] > 0) Projectile.scale = Projectile.ai[2];
            Projectile.timeLeft = 3;
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.netUpdate = true;
                Player player = Main.player[Projectile.owner];
                AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
                Projectile.Center = position + positionOffset;

                if (Projectile.ai[0] == 0)
                {
                    keepOn = adept.IsUsingPrimaryAbility && adept.CanUsePrimary;
                }
                else
                {
                    keepOn = adept.IsUsingSpecialAbility;
                }

                if (!keepOn) Projectile.Kill();

                switch (Projectile.ai[0])
                {
                    case 0:
                        Projectile.Center = player.Center;
                        break;
                    case 1:
                        break;
                    case 2:
                        MovementAI(90);
                        break;
                    case 3:
                        TripleMovementAI(player);
                        break;
                }
            }
        }

        private void MovementAI(int sendingTime)
        {
            if (timer >= 150) Projectile.timeLeft = 0;

            if (timer == sendingTime)
            {
                target = Main.MouseWorld;
            }

            if (target.DistanceSQ(position) >= 40)
            {
                position += position.DirectionTo(target) * 12;
            }

            timer++;
        }

        private void TripleMovementAI(Player player)
        {
            if (timer < 180)
            {
                positionOffset = positionOffset.RotatedBy(0.0418879f);
            }
            if (timer >= 30)
            {
                if (timer < 90)
                {
                    positionOffset += Projectile.Center.DirectionFrom(player.Center) * 5;
                }
                if (timer == 210) target = Projectile.Center.DirectionTo(player.Center) * 20000;
            } else
            {
                Projectile.Center = position;
            }

            if (target.DistanceSQ(Projectile.Center) >= 40 && timer >= 210)
            {
                positionOffset += Projectile.Center.DirectionTo(position + target) * 12;
            }

            timer++;
        }

        // Blushiemagic
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 ellipsePosition = new Vector2(projHitbox.Left, projHitbox.Top);
            Vector2 ellipseDimentions = new Vector2(projHitbox.Width, projHitbox.Height);
            Vector2 ellipseCenter = ellipsePosition + 0.5f * ellipseDimentions;
            ellipseDimentions *= Projectile.scale;
            float x = 0f;
            float y = 0f;
            if (targetHitbox.Left > ellipseCenter.X)
            {
                x = targetHitbox.Left - ellipseCenter.X;
            }
            else if (targetHitbox.Left + targetHitbox.Width < ellipseCenter.X)
            {
                x = targetHitbox.Left + targetHitbox.Width - ellipseCenter.X;
            }
            if (targetHitbox.Top > ellipseCenter.Y)
            {
                y = targetHitbox.Top - ellipseCenter.Y;
            }
            else if (targetHitbox.Top + targetHitbox.Height < ellipseCenter.Y)
            {
                y = targetHitbox.Top + targetHitbox.Height - ellipseCenter.Y;
            }
            float a = ellipseDimentions.X / 2f;
            float b = ellipseDimentions.Y / 2f;
            return (x * x) / (a * a) + (y * y) / (b * b) <= 1;
        }

        // direwolf420
        private Projectile GetNetProjectile(int owner, int identity, int type, out int index)
        {
            for (short i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == owner && proj.identity == identity && proj.type == type)
                {
                    index = i;
                    return proj;
                }
            }
            index = Main.maxProjectiles;
            return null;
        }
    }
}
