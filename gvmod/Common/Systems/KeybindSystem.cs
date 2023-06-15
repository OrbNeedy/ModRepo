using Terraria.ModLoader;

namespace gvmod.Common.Systems
{
	public class KeybindSystem : ModSystem
	{
		public static ModKeybind primaryAbility { get; private set; }
		public static ModKeybind secondaryAbility { get; private set; }
		public static ModKeybind special1 { get; private set; }
		public static ModKeybind special2 { get; private set; }
		public static ModKeybind special3 { get; private set; }
		public static ModKeybind special4 { get; private set; }
		public static ModKeybind abilityMenu { get; private set; }

		public override void Load()
		{
			primaryAbility = KeybindLoader.RegisterKeybind(Mod, "Primary septimal ability", "F");
			secondaryAbility = KeybindLoader.RegisterKeybind(Mod, "Secondary septimal ability/cooldown time", "Q");
			special1 = KeybindLoader.RegisterKeybind(Mod, "First special ability", "I");
			special2 = KeybindLoader.RegisterKeybind(Mod, "Second special ability", "J");
			special3 = KeybindLoader.RegisterKeybind(Mod, "Third special ability", "K");
			special4 = KeybindLoader.RegisterKeybind(Mod, "Fourth special ability", "L");
			abilityMenu = KeybindLoader.RegisterKeybind(Mod, "Ability menu", "P");
		}

		public override void Unload()
		{
			primaryAbility = null;
			secondaryAbility = null;
			abilityMenu = null;
		}
	}
}
