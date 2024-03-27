using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace gvmod.Common.GlobalPlayers
{
    public class TaggedPlayer : PlayerDrawLayer
    {
        private Asset<Texture2D> shock = ModContent.Request<Texture2D>("gvmod/Content/Projectiles/Shock");
        public int TagLevel { get; set; }
        public bool Shocked { get; set; }
        private int rotation;
        public override Position GetDefaultPosition()
        {
            return new Between(PlayerDrawLayers.CaptureTheGem, PlayerDrawLayers.BeetleBuff);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (Shocked)
            {
                var position = drawInfo.Center - Main.screenPosition + shock.Size() / 2;
                position = new Vector2((int)position.X, (int)position.Y);
                drawInfo.DrawDataCache.Add(new DrawData(
                    shock.Value,
                    position,
                    null,
                    Color.White * 0.8f,
                    0f,
                    shock.Size(),
                    1f,
                    SpriteEffects.None,
                    0
                ));
            }

            if (TagLevel > 0)
            {
                if (TagLevel == 2)
                {
                    Rotate();
                }
                else if (TagLevel == 3)
                {
                    ReverseRotate();
                }
                else
                {
                    rotation = 0;
                }
                Asset<Texture2D> tag = ModContent.Request<Texture2D>("gvmod/Assets/Effects/Tag" + TagLevel);
                var position = drawInfo.Center - Main.screenPosition;
                position = new Vector2((int)position.X, (int)position.Y);
                drawInfo.DrawDataCache.Add(new DrawData(
                    tag.Value,
                    position,
                    null,
                    Color.White * 0.5f,
                    MathHelper.ToRadians(rotation),
                    tag.Size() * 0.5f,
                    1f,
                    SpriteEffects.None,
                    0
                ));
                Asset<Texture2D> mark = ModContent.Request<Texture2D>("gvmod/Assets/Effects/TagMark" + TagLevel);
                position = new Vector2((int)position.X, (int)position.Y);
                drawInfo.DrawDataCache.Add(new DrawData(
                    mark.Value,
                    position,
                    null,
                    Color.White * 0.5f,
                    0f,
                    tag.Size() * 0.5f,
                    1f,
                    SpriteEffects.None,
                    0
                ));
            }
        }

        private void Rotate()
        {
            rotation++;
            if (rotation >= 361)
            {
                rotation = 0;
            }
        }

        private void ReverseRotate()
        {
            rotation -= 2;
            if (rotation <= -361)
            {
                rotation = 0;
            }
        }
    }
}
