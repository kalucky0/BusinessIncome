using Il2CppScheduleOne.Property;
using MelonLoader;

[assembly: MelonInfo(typeof(BusinessIncome.Core), "BusinessIncome", "1.0.0", "kalucky0", null)]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace BusinessIncome
{
    public sealed class Core : MelonMod
    {
        private Action _onMinutePass;
        private Income _income;

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (sceneName != "Main" || !Managers.Get()) return;

            _income ??= new Income();

            _onMinutePass = OnMinutePass;
            Managers.Time.onMinutePass.Add(_onMinutePass);
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            if (!Managers.IsInitialized) return;

            Managers.Time.onMinutePass.Remove(_onMinutePass);
            _onMinutePass = null;
        }

        private void OnMinutePass()
        {
            if (Managers.Time.CurrentTime != 1759) return;

            LoggerInstance.Msg("Payout time!");
            _income.TriggerPayouts();
        }
    }
}