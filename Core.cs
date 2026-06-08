using MelonLoader;

[assembly: MelonInfo(typeof(BusinessIncome.Core), "BusinessIncome", "1.2.0", "kalucky0", null)]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace BusinessIncome
{
    public sealed class Core : MelonMod
    {
        private const string MainSceneName = "Main";

        private Action _onMinutePass;
        private bool _isSubscribed;
        private int _lastPayoutDay = -1;
        private Preferences _prefs;
        private Income _income;

        public override void OnInitializeMelon()
        {
            _prefs = new Preferences();
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (sceneName != MainSceneName || !Managers.Get()) return;

            _income ??= new Income(_prefs);

            SubscribeToMinutePass();
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            if (sceneName != MainSceneName) return;

            UnsubscribeFromMinutePass();

            Managers.Reset();
        }

        private void SubscribeToMinutePass()
        {
            if (_isSubscribed) return;

            _onMinutePass = OnMinutePass;
            Managers.Time.onMinutePass.Add(_onMinutePass);
            _isSubscribed = true;
        }

        private void UnsubscribeFromMinutePass()
        {
            if (!_isSubscribed || _onMinutePass == null) return;

            if (Managers.Time != null)
                Managers.Time.onMinutePass.Remove(_onMinutePass);

            _onMinutePass = null;
            _isSubscribed = false;
        }

        private void OnMinutePass()
        {
            if (!Managers.IsInitialized) return;
            if (Managers.Time.CurrentTime != _prefs.GetPayoutTime()) return;

            int currentDay = Managers.Time.ElapsedDays;
            if (currentDay == _lastPayoutDay) return;

            _lastPayoutDay = currentDay;
            LoggerInstance.Msg("Payout time!");
            _income.TriggerPayouts();
        }
    }
}
