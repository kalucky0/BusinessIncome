using Il2CppScheduleOne.Money;
using Il2CppScheduleOne.Property;
using UnityEngine;

namespace BusinessIncome
{
    internal sealed class Income
    {
        private readonly Sprite _cashFront;

        public Income()
        {
            _cashFront = FindCashSprite();
        }

        public void TriggerPayouts()
        {
            foreach (var business in Business.OwnedBusinesses)
            {
                int income = CalculateIncome(business);
                string money = MoneyManager.ApplyMoneyTextColor($"${income}");
                Managers.Notifications.SendNotification(business.name, $"Made {money} today", _cashFront);
                Managers.Money.CreateOnlineTransaction(business.propertyName, income, 1, "Income");
            }
        }

        private int CalculateIncome(Business business)
        {
            float multiplier = GetIncomeMultiplier(business.PropertyName);
            return Mathf.RoundToInt(UnityEngine.Random.Range(100f, 300f) * multiplier);
        }

        private float GetIncomeMultiplier(string businessName)
        {
            return businessName switch
            {
                "Laundromat" => 1f,
                "Post Office" => 1.5f,
                "Car Wash" => 2f,
                "Taco Ticklers" => 3f,
                _ => 1f
            };
        }

        private Sprite FindCashSprite()
        {
            Sprite[] sprites = Resources.FindObjectsOfTypeAll<Sprite>();

            foreach (Sprite sprite in sprites)
            {
                if (sprite.name == "cash_front")
                    return sprite;
            }

            return null;
        }
    }
}
