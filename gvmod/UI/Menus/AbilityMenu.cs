

using gvmod.Common.Players;
using gvmod.Common.Players.Septimas.Abilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace gvmod.UI.Menus
{
    internal class AbilityMenu : UIState
    {
        private UIElement area;
        private AbilityDisplay[] abilityDisplays;
        private UIImage abilityMenuBack;
        private UIImage selectionBack;
        private UIImageButton selectionRight;
        private UIImageButton selectionLeft;
        private SpecialOption specialOption;
        private UIText level;
        private int editingSlot = 0;
        private bool selecting = false;
        private int specialIndex = 0;
        private Vector2 offset;
        private bool dragging;

        public override void OnInitialize()
        {
            area = new UIElement();
            area.Left.Set(-area.Width.Pixels - 200, 1f);
            area.Top.Set(-area.Height.Pixels - 300, 1f);
            area.Width.Set(150, 0f);
            area.Height.Set(140, 0f);

            // Background
            Asset<Texture2D> backTexture = ModContent.Request<Texture2D>("gvmod/Assets/Menus/AbilityMenuBack");
            abilityMenuBack = new AbilityMenuPanel(backTexture);
            abilityMenuBack.Left.Set(0, 0f);
            abilityMenuBack.Top.Set(0, 0f);
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

            // Level text
            level = new UIText("1", 1.2f);
            level.Left.Set(38, 0f);
            level.Top.Set(116, 0f);
            level.Width.Set(12, 0f);
            level.Height.Set(16, 0f);

            // Append all to the background
            area.Append(abilityMenuBack);
            for (int i = 0; i < 4; i++)
            {
                abilityMenuBack.Append(abilityDisplays[i]);
            }
            abilityMenuBack.Append(level);
            abilityMenuBack.Append(selectionBack);
            abilityMenuBack.Append(specialOption);
            abilityMenuBack.Append(selectionLeft);
            abilityMenuBack.Append(selectionRight);
            Append(area);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            var adept = Main.LocalPlayer.GetModPlayer<AdeptPlayer>();
            if (adept.Septima.Name == "Human") Deactivate();
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
                abilityDisplays[editingSlot].specialIndex = specialOption.specialIndex;
            }
            specialOption.specialIndex = 0;
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
                for (int i = 0; i < adept.activeSlot.Count; i++)
                {
                    adept.activeSlot[i] = 0;
                    abilityDisplays[i].specialIndex = 0;
                }
            }
            List<int> posibleList = adept.Septima.AvaliableSpecialsIndex(); // Gets the index of all abilities if the avalible level is less or equal than the player's level
            level.SetText(adept.level.ToString());
            specialOption.specialIndex = posibleList[specialIndex];

            UpdatePositions();
            if (!selectionBack.IsMouseHovering && !selectionLeft.IsMouseHovering && !selectionRight.IsMouseHovering && Main.mouseLeft)
            {
                selecting = false;
            }

            if (!selecting)
            {
                // This gives the player's slot the selected special
                for (int i = 0; i < adept.activeSlot.Count; i++)
                {
                    adept.activeSlot[i] = abilityDisplays[i].specialIndex;
                }
                selectionBack.Remove();
                specialOption.Remove();
                selectionLeft.Remove();
                selectionRight.Remove();
            }
            else
            {
                dragging = false;
                abilityMenuBack.Append(selectionBack);
                abilityMenuBack.Append(specialOption);
                abilityMenuBack.Append(selectionLeft);
                abilityMenuBack.Append(selectionRight);
            }
        }

        public override void MouseDown(UIMouseEvent evt)
        {
            base.MouseDown(evt);
            if (abilityMenuBack.ContainsPoint(evt.MousePosition))
            {
                DragStart(evt);
            }
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            base.MouseUp(evt);
            if (abilityMenuBack.ContainsPoint(evt.MousePosition))
            {
                DragEnd(evt);
            }
        }

        public void UpdatePositions()
        {
            if (abilityMenuBack.ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            if (dragging)
            {
                Left.Set(Main.mouseX - offset.X, 0f); // Main.MouseScreen.X and Main.mouseX are the same
                Top.Set(Main.mouseY - offset.Y, 0f);
                Recalculate();
            }
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

        // Return false if any of the slots have an ability the player shouldn't have
        private bool CheckPlayerAbilities(AdeptPlayer adept)
        {
            for (int i = 0; i < abilityDisplays.Length; i++)
            {
                bool containsValidSpecial = false;
                foreach (Special special in adept.Septima.Abilities)
                {
                    if (special == abilityDisplays[i].assignedSpecial && special.UnlockLevel <= adept.level)
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

