using System;
using UnityEngine.Tilemaps;
using DefaultNamespace;
using UnityEngine;
using Utils;

namespace DefaultNamespace
{
    public class GridManager: MonoBehaviour
    {
        [SerializeField] private Tile tile;
        public Tilemap tilemap;
        public Camera globalCamera;
        private BoxCollider _tileMapCollider;
        private Vector3? _tileVector = null;
        public bool checkIsCoordinateInBounds()
        {
            return true;
        }

        private void Start()
        {
            _tileMapCollider = tilemap.GetComponent<BoxCollider>();
        }
        
        private void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                // var mouseCoordinates = Input.mousePosition;
                // var ray = globalCamera.ScreenPointToRay(mouseCoordinates);
                // if (!_tileMapCollider.Raycast(ray, out var hit, 300.0f)) return;
                // var cell = tilemap.WorldToCell(hit.point);
                // // if (_tileVector is null)
                // // {
                // //     _tileVector = tilemap.CellToWorld(cell);
                // // }
                // // else
                // // {
                // //     CubeLinedraw((Vector3) _tileVector, tilemap.CellToWorld(cell));
                // // }
                // _tileVector = tilemap.CellToWorld(cell);
                // tilemap.SetTile(new Vector3Int(cell.x, cell.y, cell.z), tile);
                //
                // var cube = GridCoordinates.UnityCellToCube(cell);
            }
        }
    }
}