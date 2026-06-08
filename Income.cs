using Il2CppScheduleOne.Money;
using Il2CppScheduleOne.Property;
using MelonLoader;
using UnityEngine;

namespace BusinessIncome
{
    internal sealed class Income(Preferences prefs)
    {
        private const string CashSpriteName = "cash_front";

        private readonly Preferences _prefs = prefs;
        private Sprite _cashFront;
        private bool _warnedInvertedIncomeRange;
        private bool _warnedMissingSprite;

        public void TriggerPayouts()
        {
            foreach (var business in Business.OwnedBusinesses)
            {
                int income = CalculateIncome(business);
                if (_prefs.EnableNotifications.Value)
                {
                    string money = MoneyManager.ApplyMoneyTextColor($"${income}");
                    Managers.Notifications.SendNotification(business.name, $"Made {money} today", GetCashSprite());
                }
                Managers.Money.CreateOnlineTransaction(business.propertyName, income, 1, "Income");
            }
        }

        private int CalculateIncome(Business business)
        {
            float minIncome = _prefs.MinBaseIncome.Value;
            float maxIncome = _prefs.MaxBaseIncome.Value;

            if (minIncome > maxIncome)
            {
                if (!_warnedInvertedIncomeRange)
                {
                    MelonLogger.Warning("MinBaseIncome is greater than MaxBaseIncome; values will be swapped");
                    _warnedInvertedIncomeRange = true;
                }

                (minIncome, maxIncome) = (maxIncome, minIncome);
            }

            float multiplier = _prefs.GetMultiplier(business.PropertyName);
            return Mathf.RoundToInt(UnityEngine.Random.Range(minIncome, maxIncome) * multiplier);
        }

        private Sprite GetCashSprite()
        {
            if (_cashFront != null)
                return _cashFront;

            foreach (Sprite sprite in Resources.FindObjectsOfTypeAll<Sprite>())
            {
                if (sprite.name == CashSpriteName)
                {
                    _cashFront = sprite;
                    return _cashFront;
                }
            }

            if (!_warnedMissingSprite)
            {
                MelonLogger.Warning($"Could not find notification sprite '{CashSpriteName}'");
                _warnedMissingSprite = true;
            }

            return null;
        }
    }
}
