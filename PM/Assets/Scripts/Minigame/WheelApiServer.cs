using UnityEngine;
using Server.API;
using Promises;

namespace PM.WheelMiniGame
{
    public static class WheelApiServer
    {
        private static GameplayApi gameplayApi;

        static WheelApiServer()
        {
            gameplayApi = new GameplayApi();
            var promise = gameplayApi.Initialise();
            promise.Then(() =>
            {
                Debug.Log("GameplayApi Initialized");
            }).Catch(exception =>
            {
                Debug.LogErrorFormat("GameplayApi failed to initialize with error: {0}", exception);
            });
        }

        public static IPromise<long> GetPlayerBalance()
        {
            return gameplayApi.GetPlayerBalance();
        }

        public static IPromise<int> GetInitialWinnings()
        {
            return gameplayApi.GetInitialWin();
        }

        public static IPromise<int> GetMultiplier()
        {
            return gameplayApi.GetMultiplier();
        }

        public static IPromise SetPlayerBalance(long balance)
        {
            return gameplayApi.SetPlayerBalance(balance);
        }
    }
}
