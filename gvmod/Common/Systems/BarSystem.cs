using Microsoft.Xna.Framework;
using Terraria.UI;
using Terraria;
using Terraria.ModLoader;
using gvmod.UI.Bars;
using System.Collections.Generic;
using gvmod.UI.Menus;

namespace gvmod.Common.Systems
{
    public class BarSystem : ModSystem
    {
        internal SPBar spBar;
        private UserInterface _spBar;
        internal APBar apBar;
        private UserInterface _apBar;
        internal AbilityMenu abilityMenu;
        private UserInterface _abilityMenu;

        public override void Load()
        {
            spBar = new SPBar();
            spBar.Activate();
            apBar = new APBar();
            apBar.Activate();
            abilityMenu = new AbilityMenu();
            abilityMenu.Activate();
            _spBar = new UserInterface();
            _spBar.SetState(spBar);
            _apBar = new UserInterface();
            _apBar.SetState(apBar);
            _abilityMenu = new UserInterface();
            _abilityMenu.SetState(abilityMenu);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            _spBar?.Update(gameTime);
            _apBar?.Update(gameTime);
            _abilityMenu?.Update(gameTime);
        }

        internal void ShowUI()
        {
            _spBar.SetState(spBar);
            _apBar.SetState(apBar);
            _abilityMenu.SetState(abilityMenu);
        }

        internal void HideUI()
        {
            _spBar.SetState(null);
            _apBar.SetState(null);
            _abilityMenu.SetState(null);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name == "Vanilla: Mouse Text");
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "gvmod: Sp Bar",
                    delegate
                    {
                        _spBar.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "gvmod: Ap Bar",
                    delegate
                    {
                        _apBar.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "gvmod: Ability Menu",
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
