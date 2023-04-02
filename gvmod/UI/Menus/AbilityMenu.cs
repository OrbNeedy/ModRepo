

using gvmod.Common.Players;
using gvmod.Common.Players.Septimas.Skills;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SteelSeries.GameSense;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace gvmod.UI.Menus
{
    internal class AbilityMenu : UIState
    {
        private AbilityDisplay[] abilityDisplays;
        private AbilityMenuPanel abilityMenuBack;
        private UIImage selectionBack;
        private UIImageButton selectionRight;
        private UIImageButton selectionLeft;
        private SpecialOption specialOption;
        private UIText level;
        private int editingSlot = 0;
        private bool selecting = false;
        private bool canMove = true;
        private int specialIndex = 0;

        public override void OnInitialize()
        {
            // Background
            Asset<Texture2D> backTexture = ModContent.Request<Texture2D>("gvmod/Assets/Menus/AbilityMenuBack");
            abilityMenuBack = new AbilityMenuPanel(backTexture);
            abilityMenuBack.SetPadding(0);
            abilityMenuBack.Left.Set((int)(Main.ScreenSize.X * 0.85), 0f);
            abilityMenuBack.Top.Set((int)(Main.ScreenSize.Y * 0.4), 0f);
            abilityMenuBack.Width.Set(150, 0f);
            abilityMenuBack.Height.Set(140, 0f);

            // Ability slots
            abilityDisplays = new AbilityDisplay[4];
            for (int i = 0; i < 4; i++)
            {
                Asset<Texture2D> display = ModContent.Request<Texture2D>("gvmod/Assets/Menus/AbilityEmpty" + (i + 1));
                int currentI = i;
                abilityDisplays[i] = new AbilityDisplay(display, i);
                abilityDisplays[i].Width.Set(58, 0f);
                abilityDisplays[i].Height.Set(48, 0f);
                abilityDisplays[i].OnClick += (evt, listener) => { OnSlotClick(evt, listener, currentI); };
            }
            abilityDisplays[0].Left.Set(12, 0);
            abilityDisplays[0].Top.Set(8, 0);
            abilityDisplays[1].Left.Set(76, 0);
            abilityDisplays[1].Top.Set(8, 0);
            abilityDisplays[2].Left.Set(8, 0);
            abilityDisplays[2].Top.Set(68, 0);
            abilityDisplays[3].Left.Set(70, 0);
            abilityDisplays[3].Top.Set(68, 0);

            // Selection mode back panel
            Asset<Texture2D> selectionBackTexture = ModContent.Request<Texture2D>("gvmod/Assets/Menus/SelectionMenuBack");
            selectionBack = new SelectionPanel(selectionBackTexture);
            selectionBack.Left.Set(0, 0f);
            selectionBack.Top.Set(40, 0f);
            selectionBack.Width.Set(150, 0f);
            selectionBack.Height.Set(60, 0f);

            // Right arrow for selecting
            Asset<Texture2D> rightArrow = ModContent.Request<Texture2D>("gvmod/Assets/Icons/ArrowRight");
            selectionRight = new UIImageButton(rightArrow);
            selectionRight.Left.Set(96, 0f);
            selectionRight.Top.Set(54, 0f);
            selectionRight.Width.Set(20, 0f);
            selectionRight.Height.Set(48, 0f);
            selectionRight.OnMouseDown += OnClickRightArrow;

            // Left arrow for selecting
            Asset<Texture2D> leftArrow = ModContent.Request<Texture2D>("gvmod/Assets/Icons/ArrowLeft");
            selectionLeft = new UIImageButton(leftArrow);
            selectionLeft.Left.Set(36, 0f);
            selectionLeft.Top.Set(54, 0f);
            selectionLeft.Width.Set(20, 0f);
            selectionLeft.Height.Set(48, 0f);
            selectionLeft.OnMouseDown += OnClickLeftArrow;

            // Selection option
            Asset<Texture2D> none = ModContent.Request<Texture2D>("gvmod/Assets/Icons/NoneIcon");
            specialOption = new SpecialOption(none, 0);
            specialOption.Left.Set(62, 0f);
            specialOption.Top.Set(59, 0f);
            specialOption.Width.Set(26, 0f);
            specialOption.Height.Set(22, 0f);
            specialOption.OnMouseDown += OnOptionClick;
            specialOption.OnMouseUp += OnOptionRelease;

            // Level text
            level = new UIText("1", 1.2f);
            level.Left.Set(38, 0f);
            level.Top.Set(116, 0f);
            level.Width.Set(12, 0f);
            level.Height.Set(16, 0f);

            // Append all to the background
            for (int i = 0; i < 4; i++)
            {
                abilityMenuBack.Append(abilityDisplays[i]);
            }
            abilityMenuBack.Append(level);
            abilityMenuBack.Append(selectionBack);
            abilityMenuBack.Append(specialOption);
            abilityMenuBack.Append(selectionLeft);
            abilityMenuBack.Append(selectionRight);
            Append(abilityMenuBack);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            var adept = Main.CurrentPlayer.GetModPlayer<AdeptPlayer>();
            if (adept.Septima.Name == "Human") Deactivate();
        }

        private void OnSlotClick(UIMouseEvent evt, UIElement listeningElement, int i)
        {
            canMove = false;
            if (!selecting)
            {
                selecting = true;
                editingSlot = i;
            }
        }

        private void OnOptionClick(UIMouseEvent evt, UIElement listeningElement)
        {
            canMove = false;
            if (selecting)
            {
                selecting = false;
                abilityDisplays[editingSlot].specialIndex = specialOption.specialIndex;
            }
            specialOption.specialIndex = 0;
        }

        private void OnOptionRelease(UIMouseEvent evt, UIElement listeningElement)
        {
            canMove = true;
        }

        private void OnClickRightArrow(UIMouseEvent evt, UIElement listeningElement)
        {
            var adept = Main.CurrentPlayer.GetModPlayer<AdeptPlayer>();
            List<Special> posibleList = adept.Septima.AvaliableSpecials();
            if (specialIndex < posibleList.Count - 1)
            {
                specialIndex++;
            }
            else
            {
                specialIndex = 0;
            }
        }

        private void OnClickLeftArrow(UIMouseEvent evt, UIElement listeningElement)
        {
            var adept = Main.CurrentPlayer.GetModPlayer<AdeptPlayer>();
            List<Special> posibleList = adept.Septima.AvaliableSpecials();
            if (specialIndex > 0)
            {
                specialIndex--;
            }
            else
            {
                specialIndex = posibleList.Count - 1;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var adept = Main.CurrentPlayer.GetModPlayer<AdeptPlayer>();
            if (!CheckPlayerAbilities(adept) || Main.dedServ)
            {
                specialOption.specialIndex = 0;
                specialIndex = 0;
                for (int i = 0; i < adept.ActiveSlots.Count; i++)
                {
                    adept.ActiveSlots[i] = 0;
                    abilityDisplays[i].specialIndex = 0;
                }
            }
            List<int> posibleList = adept.Septima.AvaliableSpecialsIndex(); // Gets the index of all abilities if the avalible level is less or equal than the player's level
            level.SetText(adept.Level.ToString());
            specialOption.specialIndex = posibleList[specialIndex];

            if (!selectionBack.IsMouseHovering && !selectionLeft.IsMouseHovering && !selectionRight.IsMouseHovering && Main.mouseLeft)
            {
                selecting = false;
            }

            abilityMenuBack.CanMove = canMove;

            if (!selecting)
            {
                // This gives the player's slot the selected special
                for (int i = 0; i < adept.ActiveSlots.Count; i++)
                {
                    adept.ActiveSlots[i] = abilityDisplays[i].specialIndex;
                }
                selectionBack.Remove();
                specialOption.Remove();
                selectionLeft.Remove();
                selectionRight.Remove();
            }
            else
            {
                abilityMenuBack.Append(selectionBack);
                abilityMenuBack.Append(specialOption);
                abilityMenuBack.Append(selectionLeft);
                abilityMenuBack.Append(selectionRight);
            }
        }

        // Return false if any of the slots have an ability the player shouldn't have
        private bool CheckPlayerAbilities(AdeptPlayer adept)
        {
            for (int i = 0; i < abilityDisplays.Length; i++)
            {
                bool containsValidSpecial = false;
                foreach (Special special in adept.Septima.Abilities)
                {
                    if (special == abilityDisplays[i].assignedSpecial && special.UnlockLevel <= adept.Level)
                    {
                        containsValidSpecial = true;
                    }
                }
                if (!containsValidSpecial)
                {
                    return false;
                }
            }
            return true;
        }

        public void ResetDisplays()
        {
            specialOption.specialIndex = 0;
            specialIndex = 0;
            for (int i = 0; i < abilityDisplays.Length; i++)
            {
                abilityDisplays[i].specialIndex = 0;
            }
        }
    }
}

