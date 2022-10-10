using gvmod.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace gvmod.UI.Bars
{
    public class APBar : UIState
    {
        public int x = (int)(Main.screenWidth * 0.35f);
        public int y = (Main.screenHeight - 32);
        public ApBarBack apBarBack;
        public Texture2D filling = ModContent.Request<Texture2D>("gvmod/Assets/Bars/APBarFilling", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        public Texture2D full = ModContent.Request<Texture2D>("gvmod/Assets/Bars/APBarFull", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        public override void OnInitialize()
        {
            apBarBack = new ApBarBack();
            apBarBack.Left.Set(x, 0f);
            apBarBack.Top.Set(y, 0f);
            apBarBack.Width.Set(60, 0f);
            apBarBack.Height.Set(28, 0f);

            Append(apBarBack);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            var adept = Main.LocalPlayer.GetModPlayer<AdeptPlayer>();
            if (adept.septima.Name == "Human") return;
            float[] bars = new float[3];
            if (adept.abilityPower <= 1)
            {
                bars[2] = 0;
                bars[1] = 0;
                bars[0] = adept.abilityPower;
            } else if (adept.abilityPower <= 2)
            {
                spriteBatch.Draw(full, new Rectangle((int)apBarBack.Left.Pixels, (int)apBarBack.Top.Pixels, 16, 28), Color.White);
                bars[2] = 0;
                bars[1] = adept.abilityPower - 1;
            } else if (adept.abilityPower < 3)
            {
                spriteBatch.Draw(full, new Rectangle((int)apBarBack.Left.Pixels, (int)apBarBack.Top.Pixels, 16, 28), Color.White);
                spriteBatch.Draw(full, new Rectangle((int)apBarBack.Left.Pixels + 22, (int)apBarBack.Top.Pixels, 16, 28), Color.White);
                bars[2] = adept.abilityPower - 2;
            }if(adept.abilityPower >= 3)
            {
                spriteBatch.Draw(full, new Rectangle((int)apBarBack.Left.Pixels, (int)apBarBack.Top.Pixels, 16, 28), Color.White);
                spriteBatch.Draw(full, new Rectangle((int)apBarBack.Left.Pixels + 22, (int)apBarBack.Top.Pixels, 16, 28), Color.White);
                spriteBatch.Draw(full, new Rectangle((int)apBarBack.Left.Pixels + 44, (int)apBarBack.Top.Pixels, 16, 28), Color.White);
            }
            spriteBatch.Draw(filling, new Rectangle((int)apBarBack.Left.Pixels + 2, (int)(apBarBack.Top.Pixels + 28 - (bars[0] * 28)), 12, (int)(bars[0] * 28)), Color.White);
            spriteBatch.Draw(filling, new Rectangle((int)apBarBack.Left.Pixels + 24, (int)(apBarBack.Top.Pixels + 28 - (bars[1] * 28)), 12, (int)(bars[1] * 28)), Color.White);
            spriteBatch.Draw(filling, new Rectangle((int)apBarBack.Left.Pixels + 46, (int)(apBarBack.Top.Pixels + 28 - (bars[2] * 28)), 12, (int)(bars[2] * 28)), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var adept = Main.LocalPlayer.GetModPlayer<AdeptPlayer>();
            if (IsMouseHovering)
            {
                Main.hoverItemName = adept.septimalPower.ToString();
            }
        }
    }
}
