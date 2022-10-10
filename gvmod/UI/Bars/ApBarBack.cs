using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;

namespace gvmod.UI.Bars
{
    public class ApBarBack : UIPanel
    {
        public Color color = Color.White;
        public Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("gvmod/Assets/Bars/APBarBack",
            ReLogic.Content.AssetRequestMode.ImmediateLoad);
        private Vector2 offset;
        public bool dragging;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

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

        public override void OnInitialize()
        {
            base.OnInitialize();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)Left.Pixels, (int)Top.Pixels, 60, 28), color);
        }

        public override void MouseDown(UIMouseEvent evt)
        {
            base.MouseDown(evt);
            DragStart(evt);
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            base.MouseUp(evt);
            DragEnd(evt);
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
