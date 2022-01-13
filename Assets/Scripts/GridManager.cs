using System;
using UnityEngine.Tilemaps;
using DefaultNamespace;
using UnityEngine;
using Utils;

namespace DefaultNamespace
{
    public class GridManager: MonoBehaviour
    {
        public Tilemap tileMap;
        public Camera globalCamera;
        private BoxCollider _tileMapCollider;

        private void Start()
        {
            _tileMapCollider = tileMap.GetComponent<BoxCollider>();
        }
        
        public Vector3? GetMouseWorldClick(Vector3 mouseCoordinates)
        {
            var ray = globalCamera.ScreenPointToRay(mouseCoordinates);
            if (!_tileMapCollider.Raycast(ray, out var hit, 300.0f)) return null;
            return hit.point; 
        }
        public Vector3Int GetMouseCellClick(Vector3 mouseCoordinates)
        {
            return tileMap.WorldToCell(mouseCoordinates); 
        }

        public bool CheckHasCellOnClickPosition(Vector3 mouseWorldClick)
        {
            var cellPosition = GetMouseCellClick(mouseWorldClick);
            return tileMap.GetTile(cellPosition);
        }
    }
}