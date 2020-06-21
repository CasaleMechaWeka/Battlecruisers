using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class InvokeTestGod : MonoBehaviour
    {
        private float _previousGameSpeed = 1;

        public async void Invoke5s()
        {
            Logging.LogMethod(Tags.ALWAYS);

            Invoke(nameof(InvokeDone), 5);

            StartCoroutine(Coroutines(CoroutineDone, 5));
            await Task.Delay(5000);
            Logging.Log(Tags.ALWAYS, "await Task.Delay() done :)");
        }

        private IEnumerator Coroutines(Action action, float Delay)
        {
            yield return new WaitForSecondsRealtime(Delay);
            action();
        }

        private void CoroutineDone()
        {
            Logging.LogMethod(Tags.ALWAYS);
        }

        private void InvokeDone()
        {
            Logging.Log(Tags.ALWAYS, "Invoke done :D");
        }

        public void PauseGame()
        {
            _previousGameSpeed = Time.timeScale;
            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            Time.timeScale = _previousGameSpeed;
        }
    }
}