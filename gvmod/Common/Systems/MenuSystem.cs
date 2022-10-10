using Microsoft.Xna.Framework;
using Terraria.UI;
using Terraria;
using Terraria.ModLoader;
using gvmod.UI.Menus;
using System.Collections.Generic;

namespace gvmod.Common.Systems
{
    public class MenuSystem : ModSystem
    {
        internal AbilityMenu abilityMenu;
        private UserInterface _abilityMenu;

        public override void Load()
        {
            abilityMenu = new AbilityMenu();
            abilityMenu.Activate();
            _abilityMenu = new UserInterface();
            _abilityMenu.SetState(abilityMenu);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            _abilityMenu?.Update(gameTime);
            if (KeybindSystem.abilityMenu.JustPressed)
            {
                if (_abilityMenu.IsVisible)
                {
                    HideUI();
                } else
                {
                    ShowUI();
                }
            }
        }

        internal void ShowUI()
        {
            _abilityMenu.SetState(abilityMenu);
        }

        internal void HideUI()
        {
            _abilityMenu.SetState(null);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name == "Vanilla: Mouse Text");
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "gvmod: menus",
                    delegate
                    {
                        _abilityMenu.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
