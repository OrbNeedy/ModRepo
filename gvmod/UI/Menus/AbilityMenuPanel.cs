using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria;
using Terraria.GameContent.UI.Elements;
using ReLogic.Content;
using gvmod.Common.Players;
using Terraria.GameContent;

namespace gvmod.UI.Menus
{
    public class AbilityMenuPanel : UIImage
    {
        private Vector2 offset; 
        private bool dragging;
        private Color gradientA;
        private Color gradientB;
        private float quotient;
        public bool CanMove { get; set; }

        public AbilityMenuPanel(Asset<Texture2D> texture) : base(texture)
        {
            gradientA = new Color(255, 222, 60); // The color on the left
            gradientB = new Color(255, 162, 0); // The color on the right
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Rectangle hitbox = GetInnerDimensions().ToRectangle();
            hitbox.X += 92; // 92
            hitbox.Width -= 104; // 12
            hitbox.Y += 128; // 128
            hitbox.Height -= 136; // 8

            int left = hitbox.Left;
            int right = hitbox.Right;
            int steps = (int)((right - left) * quotient);
            for (int i = 0; i < steps; i += 1)
            {
                float percent = (float)i / (right - left);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
            }
        }

        public override void MouseDown(UIMouseEvent evt)
        {
            base.MouseDown(evt);
            if (ContainsPoint(evt.MousePosition) && CanMove)
            {
                DragStart(evt);
            }
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            base.MouseUp(evt);
            if (ContainsPoint(evt.MousePosition) && CanMove)
            {
                DragEnd(evt);
            }
        }

        private void DragStart(UIMouseEvent evt)
        {
            if (CanMove)
            {
                offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
                dragging = true;
            }
        }

        private void DragEnd(UIMouseEvent evt)
        {
            Vector2 endMousePosition = evt.MousePosition;
            dragging = false;

            Left.Set(endMousePosition.X - offset.X, 0f);
            Top.Set(endMousePosition.Y - offset.Y, 0f);

            Recalculate();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var adept = Main.CurrentPlayer.GetModPlayer<AdeptPlayer>();
            float currentExp = adept.Experience;
            float maxEXP = adept.MaxEXP;

            quotient = currentExp / maxEXP;
            quotient = Utils.Clamp(quotient, 0f, 1f);

            if (ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            if (dragging)
            {
                Left.Set(Main.mouseX - offset.X, 0f);
                Top.Set(Main.mouseY - offset.Y, 0f);
                Recalculate();
            }
            var parentSpace = Parent.GetDimensions().ToRectangle();
            if (!GetDimensions().ToRectangle().Intersects(parentSpace))
            {
                Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
                Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
                Recalculate();
            }
        }
    }
}
