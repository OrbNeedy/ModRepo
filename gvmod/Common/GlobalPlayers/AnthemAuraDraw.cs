using gvmod.Common.Players;
using gvmod.Content.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace gvmod.Common.GlobalPlayers
{
    public class AnthemAuraDraw : PlayerDrawLayer
    {
        private Asset<Texture2D> anthemAura = ModContent.Request<Texture2D>("gvmod/Assets/Effects/Anthem");

        private int wouldBeFrame = 0;
        private int wouldBeMaxFrames = 4;
        private int frameChangeTimer = 0;

        public override Position GetDefaultPosition()
        {
            return new Between(PlayerDrawLayers.CaptureTheGem, PlayerDrawLayers.BeetleBuff);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.shadow == 0f)
            {
                frameChangeTimer++;
                if (frameChangeTimer >= 6)
                {
                    wouldBeFrame++;
                    if (wouldBeFrame >= wouldBeMaxFrames)
                    {
                        wouldBeFrame = 0;
                    }
                    frameChangeTimer = 0;
                }

                Texture2D texture = anthemAura.Value;
                int frameHeight = texture.Height / wouldBeMaxFrames;
                Rectangle sourceRect = new(0, frameHeight*wouldBeFrame, texture.Width, frameHeight);

                if (drawInfo.drawPlayer.HasBuff(ModContent.BuffType<AnthemBuff>()) && drawInfo.drawPlayer.GetModPlayer<AdeptMuse>().AnthemLevel < 5)
                {
                    var position = drawInfo.Center - Main.screenPosition;
                    position = new Vector2((int)position.X, (int)position.Y);
                    drawInfo.DrawDataCache.Add(new DrawData(
                        anthemAura.Value,
                        position,
                        sourceRect,
                        Color.White * 0.25f,
                        0f,
                        anthemAura.Size() * new Vector2(0.5f, 0.16f),
                        1f,
                        SpriteEffects.None,
                        0
                    ));
                }
            }
            
        }
    }
}
