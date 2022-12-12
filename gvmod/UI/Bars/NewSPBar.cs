﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using gvmod.Common.Players;
using Terraria.GameContent;

namespace gvmod.UI.Bars
{
    public class NewSPBar : UIState
    {
        private UIText text;
        private UIElement area;
        private UIImage barFrame;
        private Color gradientA;
        private Color gradientB;
        private Vector2 offset;
        private bool dragging;

        public override void OnInitialize()
        {
            area = new UIElement();
            area.Left.Set(-area.Width.Pixels - 600, 1f);
            area.Width.Set(120, 0f);
            area.Height.Set(30, 0f);

            barFrame = new UIImage(ModContent.Request<Texture2D>("gvmod/Assets/Bars/SPBar", ReLogic.Content.AssetRequestMode.ImmediateLoad));
            barFrame.Left.Set(0, 0f);
            barFrame.Top.Set(0, 0f);
            barFrame.Width.Set(120, 0f);
            barFrame.Height.Set(30, 0f);

            text = new UIText("0%", 0.8f);
            text.Width.Set(120, 0f);
            text.Height.Set(30, 0f);
            text.Top.Set(40, 0f);
            text.Left.Set(0, 0f);

            gradientA = new Color(12, 179, 173); // The color on the left
            gradientB = new Color(77, 232, 227); // The color on the right

            area.Append(text);
            area.Append(barFrame);
            Append(area);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var adept = Main.LocalPlayer.GetModPlayer<AdeptPlayer>();
            if (adept.Septima.Name == "Human") return;

            if (adept.isOverheated)
            {
                gradientA = new Color(135, 11, 11);
                gradientB = new Color(173, 21, 21);
            }
            else
            {
                gradientA = new Color(12, 179, 173);
                gradientB = new Color(77, 232, 227);
            }
            base.Draw(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            var adept = Main.LocalPlayer.GetModPlayer<AdeptPlayer>();
            float quotient = adept.SeptimalPower / adept.MaxSeptimalPower;
            quotient = Utils.Clamp(quotient, 0f, 1f);

            Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();
            hitbox.X += 4;
            hitbox.Width -= 8;
            hitbox.Y += 4;
            hitbox.Height -= 8;

            int left = hitbox.Left;
            int right = hitbox.Right;
            int steps = (int)((right - left) * quotient);
            for (int i = 0; i < steps; i += 1)
            {
                float percent = (float)i / (right - left);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
            }
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

            var modPlayer = Main.LocalPlayer.GetModPlayer<AdeptPlayer>();
            int percentage = (int)(modPlayer.SeptimalPower / modPlayer.MaxSeptimalPower * 100);
            text.SetText(percentage + "%");
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
