using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;

namespace gvmod.UI.Menus
{
    public class SelectionPanel : UIImage
    {
        private Asset<Texture2D> background;

        public SelectionPanel(Asset<Texture2D> background): base(background)
        {
            this.background = background;
        }
    }
}
