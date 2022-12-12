using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using gvmod.Common.Players;
using Terraria.GameContent;
using SteelSeries.GameSense;

namespace gvmod.UI.Bars
{
    internal class NewAPBar : UIState
    {
        private UIElement area;
        private UIImage barFrame;
        private UIImage fullBar1;
        private UIImage fullBar2;
        private UIImage fullBar3;
        private Vector2 offset;
        private bool dragging;

        public override void OnInitialize()
        {
            area = new UIElement();
            area.Left.Set(-area.Width.Pixels - 1200, 1f);
            area.Width.Set(60, 0f);
            area.Height.Set(28, 0f);

            barFrame = new UIImage(ModContent.Request<Texture2D>("gvmod/Assets/Bars/APBarBack", ReLogic.Content.AssetRequestMode.ImmediateLoad));
            barFrame.Left.Set(0, 0f);
            barFrame.Top.Set(0, 0f);
            barFrame.Width.Set(16, 0f);
            barFrame.Height.Set(28, 0f);

            fullBar1 = new UIImage(ModContent.Request<Texture2D>("gvmod/Assets/Bars/APBarFull", ReLogic.Content.AssetRequestMode.ImmediateLoad));
            fullBar1.Left.Set(0, 0f);
            fullBar1.Top.Set(0, 0f);
            fullBar1.Width.Set(16, 0f);
            fullBar1.Height.Set(28, 0f);

            fullBar2 = new UIImage(ModContent.Request<Texture2D>("gvmod/Assets/Bars/APBarFull", ReLogic.Content.AssetRequestMode.ImmediateLoad));
            fullBar2.Left.Set(22, 0f);
            fullBar2.Top.Set(0, 0f);
            fullBar2.Width.Set(16, 0f);
            fullBar2.Height.Set(28, 0f);

            fullBar3 = new UIImage(ModContent.Request<Texture2D>("gvmod/Assets/Bars/APBarFull", ReLogic.Content.AssetRequestMode.ImmediateLoad));
            fullBar3.Left.Set(44, 0f);
            fullBar3.Top.Set(0, 0f);
            fullBar3.Width.Set(16, 0f);
            fullBar3.Height.Set(28, 0f);

            area.Append(barFrame);
            area.Append(fullBar1);
            area.Append(fullBar2);
            area.Append(fullBar3);
            Append(area);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var adept = Main.LocalPlayer.GetModPlayer<AdeptPlayer>();
            if (adept.Septima.Name == "Human") return;
            base.Draw(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            var adept = Main.LocalPlayer.GetModPlayer<AdeptPlayer>();
            float quotient = adept.SeptimalPower / adept.MaxSeptimalPower;
            quotient = Utils.Clamp(quotient, 0f, 1f);

            Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();

            int left = hitbox.Left;
            float[] bars = new float[3];
            if (adept.abilityPower <= 1)
            {
                fullBar1.Remove();
                fullBar2.Remove();
                fullBar3.Remove();
                bars[2] = 0;
                bars[1] = 0;
                bars[0] = adept.abilityPower;
            }
            else if (adept.abilityPower <= 2)
            {
                area.Append(fullBar1);
                bars[2] = 0;
                fullBar2.Remove();
                bars[1] = adept.abilityPower - 1;
                fullBar3.Remove();
            }
            else if (adept.abilityPower < 3)
            {
                area.Append(fullBar1);
                area.Append(fullBar2);
                bars[2] = adept.abilityPower - 2;
                fullBar3.Remove();
            }
            if (adept.abilityPower >= 3)
            {
                area.Append(fullBar1);
                area.Append(fullBar2);
                area.Append(fullBar3);
            }

            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + 2, (int)(hitbox.Y + 28 - (bars[0] * 28)), 12, (int)(bars[0] * 28)), new Color(183, 113, 34));
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + 24, (int)(hitbox.Y + 28 - (bars[1] * 28)), 12, (int)(bars[1] * 28)), new Color(183, 113, 34));
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + 46, (int)(hitbox.Y + 28 - (bars[2] * 28)), 12, (int)(bars[2] * 28)), new Color(183, 113, 34));
        }

        public override void Update(GameTime gameTime)
        {
            if (area.ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            if (dragging)
            {
                Left.Set(Main.mouseX - offset.X, 0f);
                Top.Set(Main.mouseY - offset.Y, 0f);
                Recalculate();
            }
            base.Update(gameTime);
        }

        public override void MouseDown(UIMouseEvent evt)
        {
            base.MouseDown(evt);
            if (area.ContainsPoint(evt.MousePosition))
            {
                DragStart(evt);
            }
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            base.MouseUp(evt);
            if (area.ContainsPoint(evt.MousePosition))
            {
                DragEnd(evt);
            }
        }

        private void DragStart(UIMouseEvent evt)
        {
            offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
            dragging = true;
        }

        private void DragEnd(UIMouseEvent evt)
        {
            Vector2 end = evt.MousePosition;
            dragging = false;

            Left.Set(end.X - offset.X, 0f);
            Top.Set(end.Y - offset.Y, 0f);

            Recalculate();
        }
    }
}
