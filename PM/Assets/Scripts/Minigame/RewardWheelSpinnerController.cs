using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PM.Common;

namespace PM.WheelMiniGame
{
    public class RewardWheelSpinnerController : Popup
    {
        [SerializeField]
        private TextMeshProUGUI initialPlayerBalanceText;
        [SerializeField]
        private TextMeshProUGUI balanceMultiplierText;
        [SerializeField]
        private TextMeshProUGUI totalWinningText;
        [SerializeField]
        private WheelSpinner wheelSpinner;
        [SerializeField]
        private Button spinButton;
        [SerializeField]
        private Button closeButton;

        private long currentBalance;
        private long currentWinnings;
        private int currentMultiplier;

        public override void Show()
        {
            base.Show();
            GetCurrentBalance();
            GetPlayerWinnings();
        }

        public override void Hide()
        {
            base.Hide();
            Reset();
        }

        private void GetPlayerWinnings()
        {
            WheelApiServer.GetInitialWinnings().Then((winnings) =>
            {
                currentWinnings = winnings;
                initialPlayerBalanceText.text = currentWinnings.ToString();
            }).Catch((exception) =>
            {
                // In the cases of this exception, we can probably show an error popup showing that something is wrong
                // with our servers. But since it's out of scope for this exam, let's just imagine there is :)
                Debug.LogErrorFormat("Error in getting player's initial winnings with error: {0}", exception);
            });
        }

        private void GetCurrentBalance()
        {
            WheelApiServer.GetPlayerBalance().Then((balance) =>
            {
                currentBalance = balance;
                Debug.Log("Current balance: " + currentBalance);
            }).Catch((exception) =>
            {
                Debug.LogErrorFormat("Error in getting player's current balance with error: {0}", exception);
            });
        }

        private void OnSpinClicked()
        {
            spinButton.interactable = false;
            WheelApiServer.GetMultiplier().Then((multiplier) =>
            {
                currentMultiplier = multiplier;
                wheelSpinner.SetWheelResult(currentMultiplier);
            }).Catch((exception) =>
            {
                Debug.LogErrorFormat("Error in running wheel spinner with error: {0}", exception);
                spinButton.interactable = true;
            });
        }

        private void OnSpinFinished()
        {
            balanceMultiplierText.text = currentMultiplier.ToString();
            var totalWinnings = currentWinnings * currentMultiplier;
            totalWinningText.text = totalWinnings.ToString();

            WheelApiServer.SetPlayerBalance(Convert.ToInt64(totalWinnings)).Then(() =>
            {
                currentBalance += totalWinnings;
                Debug.LogFormat("Player's new balance : {0}", currentBalance);
            }).Catch((exception) =>
            {
                Debug.LogErrorFormat("Error in setting player balance with error: {0}", exception);
            });
        }

        private void Reset()
        {
            initialPlayerBalanceText.text = "";
            balanceMultiplierText.text = "";
            totalWinningText.text = "";
            spinButton.interactable = true;
        }

        private void Awake()
        {
            spinButton.onClick.AddListener(OnSpinClicked);
            closeButton.onClick.AddListener(Hide);
            wheelSpinner.OnSpinFinished += OnSpinFinished;
        }

        private void OnDestroy()
        {
            spinButton.onClick.RemoveListener(OnSpinClicked);
            closeButton.onClick.RemoveListener(Hide);
            wheelSpinner.OnSpinFinished -= OnSpinFinished;
        }
    }
}
