using MelonLoader;

namespace BusinessIncome
{
    internal sealed class Preferences
    {
        private readonly MelonPreferences_Category _settings;
        private readonly MelonPreferences_Category _multipliers;
        public MelonPreferences_Entry<bool> EnableNotifications { private set; get; }
        public MelonPreferences_Entry<float> MinBaseIncome { private set; get; }
        public MelonPreferences_Entry<float> MaxBaseIncome { private set; get; }
        public MelonPreferences_Entry<float> LaundromatMultiplier { private set; get; }
        public MelonPreferences_Entry<float> PostOfficeMultiplier { private set; get; }
        public MelonPreferences_Entry<float> CarWashMultiplier { private set; get; }
        public MelonPreferences_Entry<float> TacoTicklersMultiplier { private set; get; }

        public Preferences()
        {
            _settings = MelonPreferences.CreateCategory("BusinessIncome_Settings", "Settings");
            _multipliers = MelonPreferences.CreateCategory("BusinessIncome_Multipliers", "Income Multipliers");

            EnableNotifications = _settings.CreateEntry("EnableNotifications", true, "Enable Income Notifications");
            MinBaseIncome = _settings.CreateEntry("MinBaseIncome", 100.0f, "Minimum Base Income");
            MaxBaseIncome = _settings.CreateEntry("MaxBaseIncome", 300.0f, "Maximum Base Income");
            LaundromatMultiplier = _multipliers.CreateEntry("LaundromatMultiplier", 1.0f, "Laundromat Income Multiplier");
            PostOfficeMultiplier = _multipliers.CreateEntry("PostOfficeMultiplier", 1.5f, "Post Office Income Multiplier");
            CarWashMultiplier = _multipliers.CreateEntry("CarWashMultiplier", 2.0f, "Car Wash Income Multiplier");
            TacoTicklersMultiplier = _multipliers.CreateEntry("TacoTicklersMultiplier", 3.0f, "Taco Ticklers Income Multiplier");
        }
    }
}
