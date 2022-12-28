using System.Collections.Generic;
using Terraria;
using gvmod.Common.Players.Septimas.Abilities;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace gvmod.Common.Players
{
    public abstract class Septima
    {
        public Player Player { get; }
        public AdeptPlayer Adept { get; }

        public abstract string Name { get; }

        public int SecondaryTimer { get; set; }
        public int SecondaryCooldownTime { get; set; }

        public List<Special> Abilities { get; set; } = new List<Special>();
        public float ApBaseRegen { get; set; }

        public float SpBaseRegen { get; set; }
        public float SpBaseOverheatRegen { get; set; }
        public float SpBaseUsage { get; set; }

        public Vector2 VelocityMultiplier { get; set; }

        public abstract bool CanRecharge { get; }

        public abstract Color ClearColor { get; }
        public abstract Color MainColor { get; }
        public abstract Color DarkColor { get; }

        protected Septima(AdeptPlayer adept, Player player)
        {
            Adept = adept;
            Player = player;
            VelocityMultiplier = new Vector2(1, 1);
            InitializeAbilitiesList();
        }

        public abstract void InitializeAbilitiesList();

        public abstract void OnOverheat();

        public abstract void OnRecovery();

        public abstract void FirstAbilityEffects();

        public abstract void FirstAbility();

        public abstract void SecondAbilityEffects();

        public abstract void SecondAbility();

        public abstract bool OnPrevasion(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter);

        public abstract void MiscEffects();

        public abstract void Updates();

        public List<Special> AvaliableSpecials()
        {
            List<Special> specials = new List<Special>();
            for (int i = 0; i < Abilities.Count; i++)
            {
                if (Abilities[i] == null)
                {
                    specials.Add(new None(Player, Adept));
                }
                else
                {
                    if (Adept.Level >= Abilities[i].UnlockLevel)
                    {
                        specials.Add(Abilities[i]);
                    }
                }
            }
            return specials;
        }

        public List<int> AvaliableSpecialsIndex()
        {
            List<int> specials = new List<int>();
            for (int i = 0; i < Abilities.Count; i++)
            {
                if (Abilities[i] == null)
                {
                    specials.Add(0);
                } else
                {
                    if (Adept.Level >= Abilities[i].UnlockLevel)
                    {
                        specials.Add(i);
                    }
                }
            }
            if (specials.Count == 0)
            {
                specials.Add(0);
            }
            return specials;
        }
    }
}
