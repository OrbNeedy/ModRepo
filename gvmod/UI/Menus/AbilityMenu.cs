using gvmod.Common.Players;
using gvmod.Common.Players.Septimas.Abilities;
using gvmod.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace gvmod.UI.Menus
{
    internal class AbilityMenu : UIState
    {
        public int x = Main.screenWidth - 160;
        public int y = (int)(Main.screenHeight * 0.4);
        public AbilityMenuBack abilityMenuBack;
        public AbilitySlot[] abilitySlots;
        public SelectionMenu selectionMenu;
        public SelectionOption selectionOption;
        public UIImageButton selectionRight;
        public UIImageButton selectionLeft;
        public UIText level;
        //TODO: implement cooldownFill
        //public UIImage cooldownFill;
        //public Texture2D cooldownFilling = ModContent.Request<Texture2D>("gvmod/Assets/Bars/APBarFilling", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        public Texture2D expFilling = (Texture2D)ModContent.Request<Texture2D>("gvmod/Assets/Bars/EXPBarFilling",
            ReLogic.Content.AssetRequestMode.ImmediateLoad);
        public bool barReposition = false;
        public int editingSlot = 0;
        public bool selecting = false;
        public int specialIndex = 0;

        public override void OnInitialize()
        {
            abilityMenuBack = new AbilityMenuBack();
            abilityMenuBack.Left.Set(x, 0f);
            abilityMenuBack.Top.Set(y, 0f);
            abilityMenuBack.Width.Set(150, 0f);
            abilityMenuBack.Height.Set(140, 0f);

            abilitySlots = new AbilitySlot[4];
            for (int i = 0; i < 4; i++)
            {
                abilitySlots[i] = new AbilitySlot(ModContent.Request<Texture2D>("gvmod/Assets/Menus/AbilityEmpty" + (i + 1), ReLogic.Content.AssetRequestMode.ImmediateLoad), i);
                abilitySlots[i].Width.Set(58, 0f);
                abilitySlots[i].Height.Set(48, 0f);
                int currentI = i;
                abilitySlots[i].OnClick += (evt, listener) => { OnSlotClick(evt, listener, currentI); };
            }

            selectionMenu = new SelectionMenu();
            selectionMenu.Width.Set(150, 0f);
            selectionMenu.Height.Set(60, 0f);

            selectionRight = new UIImageButton(ModContent.Request<Texture2D>("gvmod/Assets/Icons/ArrowRight", ReLogic.Content.AssetRequestMode.ImmediateLoad));
            selectionRight.Width.Set(20, 0f);
            selectionRight.Height.Set(48, 0f);
            selectionRight.OnMouseDown += OnClickRightArrow;

            selectionLeft = new UIImageButton(ModContent.Request<Texture2D>("gvmod/Assets/Icons/ArrowLeft", ReLogic.Content.AssetRequestMode.ImmediateLoad));
            selectionLeft.Width.Set(20, 0f);
            selectionLeft.Height.Set(48, 0f);
            selectionLeft.OnMouseDown += OnClickLeftArrow;

            selectionOption = new SelectionOption(ModContent.Request<Texture2D>("gvmod/Assets/Icons/None", ReLogic.Content.AssetRequestMode.ImmediateLoad), null);
            selectionOption.Width.Set(26, 0f);
            selectionOption.Height.Set(22, 0f);
            selectionOption.OnMouseDown += OnOptionClick;

            level = new UIText("1", 1.2f);
            level.Width.Set(12, 0f);
            level.Height.Set(16, 0f);

            Append(abilityMenuBack);
            Append(selectionMenu);
            Append(selectionLeft);
            Append(selectionRight);
            Append(selectionOption);
            Append(level);
            for (int i = 0; i < 4; i++)
            {
                Append(abilitySlots[i]);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            var adept = Main.LocalPlayer.GetModPlayer<AdeptPlayer>();
            if (adept.septima.Name == "Human") Deactivate();
            spriteBatch.Draw(expFilling, new Rectangle((int)(abilityMenuBack.Left.Pixels + 92), (int)(abilityMenuBack.Top.Pixels + 128), (int)(adept.ExperienceToFraction() * 46), 4), Color.White);
        }

        private void OnSlotClick(UIMouseEvent evt, UIElement listeningElement, int i)
        {
            if (!selecting)
            {
                selecting = true;
                editingSlot = i;
            }
        }

        private void OnOptionClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (selecting)
            {
                selecting = false;
                abilitySlots[editingSlot].assignedSpecial = selectionOption.assignedSpecial;
            }
        }

        private void OnClickRightArrow(UIMouseEvent evt, UIElement listeningElement)
        {
            var adept = Main.LocalPlayer.GetModPlayer<AdeptPlayer>();
            List<Special> posibleList = adept.septima.AvaliableSpecials();
            if (specialIndex < posibleList.Count -1)
            {
                specialIndex++;
            } else
            {
                specialIndex = 0;
            }
        }

        private void OnClickLeftArrow(UIMouseEvent evt, UIElement listeningElement)
        {
            var adept = Main.LocalPlayer.GetModPlayer<AdeptPlayer>();
            List<Special> posibleList = adept.septima.AvaliableSpecials();
            if (specialIndex > 0)
            {
                specialIndex--;
            } else
            {
                specialIndex = posibleList.Count - 1;
            }
        }

        public override void Update(GameTime gameTime)
        {
            var adept = Main.LocalPlayer.GetModPlayer<AdeptPlayer>();
            List<Special> posibleList = adept.septima.AvaliableSpecials();
            base.Update(gameTime);
            level.SetText(adept.level.ToString());
            selectionOption.assignedSpecial = posibleList[specialIndex];
            for (int i = 0; i < adept.activeSlot.Count; i++)
            {
                abilitySlots[i].assignedSpecial = adept.GetSpecial(adept.activeSlot[i]);
            }
            if (IsMouseHovering)
            {
                Main.hoverItemName = adept.septimalPower.ToString();
            }
            UpdatePositions();
            if (!selectionMenu.IsMouseHovering && !selectionLeft.IsMouseHovering && !selectionRight.IsMouseHovering && Main.mouseLeft)
            {
                selecting = false;
            }
            if (!selecting)
            {
                adept.activeSlot[editingSlot] = selectionOption.assignedSpecial.Name;
                selectionMenu.Remove();
                selectionOption.Remove();
                selectionLeft.Remove();
                selectionRight.Remove();
            } else
            {
                Append(selectionMenu);
                Append(selectionOption);
                Append(selectionLeft);
                Append(selectionRight);
            }
        }

        public void UpdatePositions()
        {
            abilitySlots[0].Left.Set(abilityMenuBack.Left.Pixels + 12, 0);
            abilitySlots[0].Top.Set(abilityMenuBack.Top.Pixels + 8, 0);
            abilitySlots[1].Left.Set(abilityMenuBack.Left.Pixels + 76, 0);
            abilitySlots[1].Top.Set(abilityMenuBack.Top.Pixels + 8, 0);
            abilitySlots[2].Left.Set(abilityMenuBack.Left.Pixels + 8, 0);
            abilitySlots[2].Top.Set(abilityMenuBack.Top.Pixels + 68, 0);
            abilitySlots[3].Left.Set(abilityMenuBack.Left.Pixels + 70, 0);
            abilitySlots[3].Top.Set(abilityMenuBack.Top.Pixels + 68, 0);

            selectionMenu.Left.Set(abilityMenuBack.Left.Pixels, 0f);
            selectionMenu.Top.Set(abilityMenuBack.Top.Pixels + 40, 0f);

            selectionOption.Left.Set(abilityMenuBack.Left.Pixels + 62, 0f);
            selectionOption.Top.Set(abilityMenuBack.Top.Pixels + 59, 0f);

            selectionLeft.Left.Set(abilityMenuBack.Left.Pixels + 36, 0f);
            selectionLeft.Top.Set(abilityMenuBack.Top.Pixels + 54, 0f);

            selectionRight.Left.Set(abilityMenuBack.Left.Pixels + 96, 0f);
            selectionRight.Top.Set(abilityMenuBack.Top.Pixels + 54, 0f);

            level.Left.Set(abilityMenuBack.Left.Pixels + 38, 0f);
            level.Top.Set(abilityMenuBack.Top.Pixels + 116, 0f);
        }
    }
}
