using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class AddPuckButton: MonoBehaviour
    {
        private Button _button;
        public PuckManager puckManager;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(TaskOnClick);
        }

        private void TaskOnClick()
        {
            puckManager.ProceedPlacing();
        }

        private void Update()
        {
        }
    }
}