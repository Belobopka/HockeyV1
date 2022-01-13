using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PuckManager: MonoBehaviour
    {
        public GameObject puckPrefab;
        private PuckObject _puckObject;
        private GridManager _gridManager;
        private UnitsManager _unitsManager;
        private bool _isPlacingPuck;
        private bool _isPlaced;
        private void Start()
        {
            _gridManager = GetComponent<GridManager>();
            _unitsManager = GetComponent<UnitsManager>();
            _puckObject = puckPrefab.GetComponent<PuckObject>();
        
        }
        
        public void ProceedPlacing()
        {
            var color = new Color(255, 255, 255, 0);
            _puckObject.ChangeColor(color);
            _isPlaced = false;
            _isPlacingPuck = true;
            
            _unitsManager.ClearSelectedUnit();
            ShowPuck();
        }
        
        public void FinishPlacing()
        {
            _isPlaced = true;
            _isPlacingPuck = false;
            var color = new Color(0, 0, 0, 1F);
            _puckObject.ChangeColor(color);
        }
        
        public void CancelPlacing()
        {
            _isPlaced = false;
            _isPlacingPuck = false;
            HidePuck();
        }

        private void HidePuck()
        {
            _puckObject.HideUnit(); 
        }
        
        private void ShowPuck()
        {
            _puckObject.ShowUnit();
            
        }


        private void HandlePlacingPuckUpdate()
        {
            var coords = Input.mousePosition;
            var mouseWorldClick = _gridManager.GetMouseWorldClick(coords);
            if (mouseWorldClick.HasValue && _gridManager.CheckHasCellOnClickPosition((Vector3) mouseWorldClick))
            {
                var mouseCoords = (Vector3) mouseWorldClick;
                var hasUnit = _unitsManager.getUnitOnTile(mouseCoords);
                if (hasUnit)
                {
                    // CancelPlacing();
                    return;
                }

                var cellPosition = _gridManager.GetMouseCellClick(mouseCoords);
                _puckObject.transform.position = _gridManager.tileMap.GetCellCenterWorld(cellPosition);
            }
        }

        private bool CheckIsClickOnUnit()
        {
            var coords = Input.mousePosition;
            var mouseWorldClick = _gridManager.GetMouseWorldClick(coords);
            if (mouseWorldClick.HasValue && _gridManager.CheckHasCellOnClickPosition((Vector3) mouseWorldClick))
            {
                var mouseCoords = (Vector3) mouseWorldClick;
                var hasUnit = _unitsManager.getUnitOnTile(mouseCoords);
                
                if (hasUnit)
                {
                    return true;
                }
            }

            return false;
        }

        private void UpdatePlacingState()
        {
            
            if (_isPlacingPuck && Input.GetButtonDown("Fire1"))
            {
                print(CheckIsClickOnUnit());
                if (CheckIsClickOnUnit())
                {
                    CancelPlacing();
                    return;
                }

                FinishPlacing();
            }
            if (_isPlacingPuck && Input.GetButtonDown("Fire2"))
            {
                CancelPlacing();
            }
            HandlePlacingPuckUpdate();
        }

        private void Update()
            {

                if (_isPlaced)
                {
                    return;
                }

                if (_isPlacingPuck)
                {
                    UpdatePlacingState();
                    return;
                }
            }
    }
}