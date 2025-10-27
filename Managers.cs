using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.GameTime;
using Il2CppScheduleOne.Money;
using Il2CppScheduleOne.UI;
using MelonLoader;

namespace BusinessIncome
{
    internal sealed class Managers
    {
        public static bool IsInitialized => Notifications != null && Time != null && Money != null;
        public static NotificationsManager Notifications { private set; get; }
        public static TimeManager Time { private set; get; }
        public static MoneyManager Money { private set; get; }

        public static bool Get()
        {
            Notifications = GetSingleton<NotificationsManager>();
            Time = GetNetSingleton<TimeManager>();
            Money = GetNetSingleton<MoneyManager>();

            return IsInitialized;
        }

        private static T GetSingleton<T>() where T : Singleton<T>
        {
            var instance = Singleton<T>.Instance;
            LogFound(instance);
            return instance;
        }

        private static T GetNetSingleton<T>() where T : NetworkSingleton<T>
        {
            var instance = NetworkSingleton<T>.Instance;
            LogFound(instance);
            return instance;
        }

        private static void LogFound<T>(T obj)
        {
            if (obj == null)
                MelonLogger.Warning($"Failed to find {typeof(T).Name}");
            else
                MelonLogger.Msg($"Found {typeof(T).Name}");
        }
    }
}
