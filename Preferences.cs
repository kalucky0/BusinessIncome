using MelonLoader;

namespace BusinessIncome
{
    internal sealed class Preferences
    {
        private readonly MelonPreferences_Category _settings;
        private readonly MelonPreferences_Category _multipliers;
        private readonly Dictionary<string, MelonPreferences_Entry<float>> _multiplierByProperty;

        public MelonPreferences_Entry<bool> EnableNotifications { private set; get; }
        public MelonPreferences_Entry<int> PayoutTime { private set; get; }
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
            PayoutTime = _settings.CreateEntry("PayoutTime", 1800, "Daily Payout Time (HHmm, e.g. 1800 = 6:00 PM)");
            MinBaseIncome = _settings.CreateEntry("MinBaseIncome", 100.0f, "Minimum Base Income");
            MaxBaseIncome = _settings.CreateEntry("MaxBaseIncome", 300.0f, "Maximum Base Income");
            LaundromatMultiplier = _multipliers.CreateEntry("LaundromatMultiplier", 1.0f, "Laundromat Income Multiplier");
            PostOfficeMultiplier = _multipliers.CreateEntry("PostOfficeMultiplier", 1.5f, "Post Office Income Multiplier");
            CarWashMultiplier = _multipliers.CreateEntry("CarWashMultiplier", 2.0f, "Car Wash Income Multiplier");
            TacoTicklersMultiplier = _multipliers.CreateEntry("TacoTicklersMultiplier", 3.0f, "Taco Ticklers Income Multiplier");

            _multiplierByProperty = new Dictionary<string, MelonPreferences_Entry<float>>
            {
                ["Laundromat"] = LaundromatMultiplier,
                ["Post Office"] = PostOfficeMultiplier,
                ["Car Wash"] = CarWashMultiplier,
                ["Taco Ticklers"] = TacoTicklersMultiplier
            };
        }

        public int GetPayoutTime()
        {
            int time = PayoutTime.Value;
            if (IsValidPayoutTime(time))
                return time;

            MelonLogger.Warning($"Invalid PayoutTime {time}; using default 1800");
            return 1800;
        }

        public float GetMultiplier(string propertyName)
        {
            return _multiplierByProperty.TryGetValue(propertyName, out var entry)
                ? entry.Value
                : 1f;
        }

        private static bool IsValidPayoutTime(int time)
        {
            if (time < 0 || time > 2359)
                return false;

            int minutes = time % 100;
            return minutes <= 59;
        }
    }
}
