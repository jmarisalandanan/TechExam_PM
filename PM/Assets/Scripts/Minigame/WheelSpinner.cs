using System.Collections.Generic;
using UnityEngine;

namespace PM.WheelMiniGame
{
    public class WheelSpinner : MonoBehaviour
    {
        private const float MAX_SPIN_SPEED = 4;
        private const float MIN_SPIN_SPEED = 1f;
        private const float MIN_STOP_SPEED = 0.3f;
        private const float ACCELERATION_FACTOR = 0.5f;
        private const float DECELERATION_FACTOR = 0.7f;
        private const float MIN_SPIN_TIME = 5f;

        private WheelItem targetWheelItem;
        private List<WheelItem> wheelItems = new List<WheelItem>();
        private bool isSpinning;
        private float currentSpinTime;
        private float spinVelocity;
        private float distanceToTarget;
        private float wheelDecelerationTime;

        public event System.Action OnSpinFinished;

        public bool IsSpinning { get { return isSpinning; } }

        public void SetWheelResult(int expectedResult)
        {
            var possibleTargets = wheelItems.FindAll(item => item.WheelValue == expectedResult);
            if (possibleTargets.Count <= 0)
            {
                throw new System.Exception(string.Format("Expected result: {0}x not found in wheel items", expectedResult));
            }
            targetWheelItem = possibleTargets[Random.Range(0, possibleTargets.Count)];
            SpinTheWheel();
        }

        private void SpinTheWheel()
        {
            spinVelocity = 0;
            currentSpinTime = 0;
            distanceToTarget = 0;
            isSpinning = true;
            wheelDecelerationTime = MIN_SPIN_TIME;
        }

        private void UpdateWheel()
        {
            currentSpinTime += Time.deltaTime;
            if (currentSpinTime >= MIN_SPIN_TIME)
            {
                wheelDecelerationTime -= Time.deltaTime;
                spinVelocity = Mathf.Max(spinVelocity - (DECELERATION_FACTOR * Time.deltaTime), MIN_SPIN_SPEED);

                if (wheelDecelerationTime <= 0)
                {
                    if (distanceToTarget == 0)
                    {
                        distanceToTarget = Vector3.Distance(Vector3.zero, targetWheelItem.transform.eulerAngles);
                    }

                    var distance = Vector3.Distance(Vector3.zero, targetWheelItem.transform.eulerAngles);
                    spinVelocity = Mathf.Max(spinVelocity * (distance / distanceToTarget), MIN_STOP_SPEED);
                    if (distance < 1f)
                    {
                        isSpinning = false;
                        OnSpinFinished();
                        return;
                    }
                }
            }
            else
            {
                spinVelocity = Mathf.Min(spinVelocity + (ACCELERATION_FACTOR * Time.deltaTime), MAX_SPIN_SPEED);
            }
            transform.localEulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z - spinVelocity);
        }

        private void Awake()
        {
            transform.GetComponentsInChildren<WheelItem>(wheelItems);
        }

        private void Update()
        {
            if (!isSpinning)
            {
                return;
            }
            UpdateWheel();
        }
    }
}
