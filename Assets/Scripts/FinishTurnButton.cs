using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class FinishTurnButton: MonoBehaviour
{
        public TurnManager turnManager;
        private Button _button;

        private void Start()
        {
                _button = GetComponent<Button>();
                _button.onClick.AddListener(TaskOnClick);
        }

        private void TaskOnClick()
        { 
                turnManager.ProceedTurn(true);
        }
}