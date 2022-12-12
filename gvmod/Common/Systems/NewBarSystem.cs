using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using System.Collections.Generic;
using gvmod.UI.Bars;

namespace ExampleMod.Common.UI.ExampleResourceUI
{
    class NewBarSystem : ModSystem
    {
        private UserInterface SPInterface;
        private UserInterface APInterface;

        private NewSPBar SPBar;
        private NewAPBar APBar;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                SPBar = new();
                SPInterface = new();
                SPInterface.SetState(SPBar);

                APBar = new();
                APInterface = new();
                APInterface.SetState(APBar);
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            SPInterface?.Update(gameTime);
            APInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "GvMod: SP Bar",
                    delegate {
                        SPInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }

            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "GvMod: AP Bar",
                    delegate {
                        APInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}