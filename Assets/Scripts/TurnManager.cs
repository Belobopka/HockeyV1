using UnityEngine;

namespace DefaultNamespace
{
    public class TurnManager: MonoBehaviour
    {
        private UnitsManager _unitsManager;
        private GameManager _gameManager;
    
        public bool TurnFinished => CheckIsTurnFinished();
        private void Start()
        {
            _unitsManager = GetComponent<UnitsManager>();
            _gameManager = GetComponent<GameManager>();
        }

        private bool CheckIsTurnFinished()
        {
            var allUnitsMoved = _unitsManager.CheckIsAllUnitsMoved();
            return allUnitsMoved;
        }

        public void ProceedTurn(bool forceProceed)
        {
            var unitIsMoving = _unitsManager.IsUnitMoving;
            if (unitIsMoving)
            {
                return;
            }
            if (!TurnFinished && !forceProceed)
            {
                return;
            }
            _unitsManager.ProceedTurn();
            _gameManager.ProceedTurn();
        } 
    }
}