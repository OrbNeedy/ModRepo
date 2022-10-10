using System.Collections.Generic;
using Terraria;
using gvmod.Common.Players.Septimas.Abilities;
using Microsoft.Xna.Framework;

namespace gvmod.Common.Players
{
    public abstract class Septima
    {
        private AdeptPlayer adept;
        private Player player;
        private List<Special> abilities = new List<Special>();
        private float spUsage;
        private int secondaryCooldown;
        private Vector2 velocityMultiplier;

        public int SecondaryTimer { get; set; }

        public Player Player { get => player; }

        public AdeptPlayer Adept { get => adept; }

        public List<Special> Abilities { get => abilities; set => abilities = value; }
        public float SpUsage { get => spUsage; set => spUsage = value; }
        public abstract string Name { get; }
        public int SecondaryCooldownTime { get => secondaryCooldown; set => secondaryCooldown = value; }

        public Vector2 VelocityMultiplier { get => velocityMultiplier; set => velocityMultiplier = value; }

        protected Septima(AdeptPlayer adept, Player player)
        {
            this.adept = adept;
            this.player = player;
            velocityMultiplier = new Vector2(1, 1);
            InitializeAbilitiesList();
        }

        public abstract void InitializeAbilitiesList();

        public abstract void FirstAbilityEffects();

        public abstract void FirstAbility();

        public abstract void SecondAbilityEffects();

        public abstract void SecondAbility();

        public abstract void MiscEffects();

        public abstract void Updates();

        public List<Special> AvaliableSpecials()
        {
            List<Special> specials = new List<Special>();
            for (int i = 0; i < abilities.Count; i++)
            {
                if (adept.level >= abilities[i].UnlockLevel)
                {
                    specials.Add(abilities[i]);
                }
            }
            return specials;
        }
    }
}
