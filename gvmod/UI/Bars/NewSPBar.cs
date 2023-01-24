using Microsoft.Xna.Framework;
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
        private UIText percentage;
        private UIText label;
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

            percentage = new UIText("0%", 0.8f);
            percentage.Width.Set(120, 0f);
            percentage.Height.Set(30, 0f);
            percentage.Top.Set(40, 0f);
            percentage.Left.Set(0, 0f);

            label = new UIText("", 0.8f);
            label.Width.Set(120, 0f);
            label.Height.Set(30, 0f);
            label.Top.Set(8, 0f);
            label.Left.Set(0, 0f);

            gradientA = new Color(12, 179, 173); // The color on the left
            gradientB = new Color(77, 232, 227); // The color on the right

            area.Append(percentage);
            area.Append(barFrame);
            area.Append(label);
            Append(area);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var adept = Main.CurrentPlayer.GetModPlayer<AdeptPlayer>();
            if (adept.Septima.Name == "Human") return;

            if (adept.IsOverheated)
            {
                gradientA = new Color(100, 11, 11);
                gradientB = new Color(155, 21, 21);
            }
            else
            {
                gradientA = adept.Septima.DarkColor;
                gradientB = adept.Septima.ClearColor;
            }
            base.Draw(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            var adept = Main.CurrentPlayer.GetModPlayer<AdeptPlayer>();
            float quotient = adept.SeptimalPower / adept.MaxSeptimalPower2;
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
                Main.CurrentPlayer.mouseInterface = true;
            }

            if (dragging)
            {
                Left.Set(Main.mouseX - offset.X, 0f);
                Top.Set(Main.mouseY - offset.Y, 0f);
                Recalculate();
            }

            var adept = Main.CurrentPlayer.GetModPlayer<AdeptPlayer>();
            int percentage = (int)(adept.SeptimalPower / adept.MaxSeptimalPower2 * 100);
            label.SetText(adept.Septima.Name);
            this.percentage.SetText(percentage + "%");
            base.Update(gameTime);
        }

        public override void MouseDown(UIMouseEvent evt)
        {
            base.MouseDown(evt);
            if (barFrame.ContainsPoint(evt.MousePosition))
            {
                DragStart(evt);
            }
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            base.MouseUp(evt);
            if (barFrame.ContainsPoint(evt.MousePosition))
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
