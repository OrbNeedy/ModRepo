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
            _abilityMenu = new UserInterface();
            abilityMenu.Activate();
            _abilityMenu.SetState(abilityMenu);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (_abilityMenu?.CurrentState != null)
            {
                _abilityMenu?.Update(gameTime);
            }
        }

        internal void ShowUI()
        {
            _abilityMenu?.SetState(abilityMenu);
        }

        internal void HideUI()
        {
            _abilityMenu?.SetState(null);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "GvMod: Menus",
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
