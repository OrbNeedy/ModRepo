﻿using gvmod.Common.GlobalNPCs;
using gvmod.Common.Players;
using gvmod.Content.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace gvmod.Content.Projectiles
{
    public class ChainMeteor : ModProjectile
    {
        // 1: Accumulate chains 2: Shoot thunders 3: Fly offscreen 4-7: Attack pattern 8: Either the last
        // movement before disappearing or the movement where it summons a glorious strizer to break the chain
        private int phase;
        private int timer;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chain Meteor");
        }

        public override void SetDefaults()
        {
            Projectile.light = 1f;
            Projectile.ignoreWater = true;
            Projectile.damage = 0;
            Projectile.knockBack = 15;
            Projectile.Size = new Vector2(192);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.timeLeft = 3000;
            Projectile.DamageType = ModContent.GetInstance<SeptimaDamageClass>();
            Projectile.ownerHitCheck = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            phase = 1;
            timer = 0;
            base.OnSpawn(source);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            timer++;
            MovementAI(player);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            return base.PreDraw(ref lightColor);
        }

        private void MovementAI(Player player)
        {
            AdeptPlayer adept = player.GetModPlayer<AdeptPlayer>();
            float xPos = 0;
            float yPos = 0;
            int velocity = timer * 8;
            int velocity2 = timer * 6;
            switch (phase)
            {
                case 1 or 2:
                    xPos = 0;
                    if (timer == 240)
                    {
                        Projectile.damage = (int)(360 * adept.SpecialDamageEquipMult * adept.SpecialDamageEquipMult);
                    }
                    if (timer >= 200)
                    {
                        yPos -= ((timer - 200) * 4);
                    } else
                    {
                        yPos = -200;
                    }
                    if (timer >= 300)
                    {
                        if (adept.PowerLevel >= 3)
                        {
                            phase++;
                        } else
                        {
                            phase += 2;
                        }
                        timer = 0;
                    }
                    break;
                case 3:
                    xPos = 800 - velocity;
                    yPos = -600 + velocity2;
                    if (timer >= 180)
                    {
                        phase++;
                        timer = 0;
                    }
                    break;
                case 4:
                    xPos = -800 + velocity;
                    yPos = -600 + velocity2;
                    if (timer >= 180)
                    {
                        phase++;
                        timer = 0;
                    }
                    break;
                case 5:
                    xPos = -800 + velocity;
                    yPos = 0;
                    if (timer >= 180)
                    {
                        phase++;
                        timer = 0;
                    }
                    break;
                case 6:
                    xPos = 800 - velocity;
                    yPos = 0;
                    if (timer >= 180)
                    {
                        phase++;
                        timer = 0;
                    }
                    break;
                case 7:
                    // If ai0 = 1, do an enhanced finish
                    xPos = 0;
                    yPos = -600 + velocity2;
                    if (timer >= 180)
                    {
                        phase++;
                        timer = 0;
                    }
                    break;
                default:
                    Projectile.timeLeft = 0;
                    break;
            }
            Projectile.Center = player.Center + new Vector2(xPos, yPos);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 ellipsePosition = new Vector2(projHitbox.Left, projHitbox.Top);
            Vector2 ellipseDimentions = new Vector2(projHitbox.Width, projHitbox.Height);
            Vector2 ellipseCenter = ellipsePosition + 0.5f * ellipseDimentions;
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
            return (x * x) / (a * a) + (y * y) / (b * b) <= (1 + Main.rand.NextFloat(0.01f, 0.05f));
        }
    }
}