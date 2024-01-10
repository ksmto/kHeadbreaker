using ThunderRoad;

namespace kHeadbreaker {
    internal class ModOptions {
        [ModOption(name = "Enabled",
            order = 0,
            defaultValueIndex = 0,
            interactionType = ModOption.InteractionType.ButtonList,
            valueSourceName = nameof(boolOptions),
            saveValue = true)]
        public static bool enabled;

        internal static ModOptionBool[] boolOptions = new ModOptionBool[] {
            new ModOptionBool("Enabled", true),
            new ModOptionBool("Disabled", false)
        };
    }
}