using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;
using DefaultNamespace;
using UnityEngine;
using Utils;

public class UnitsManager: MonoBehaviour
{
    public List<UnitManager> units;
    public GameObject[] unitPrefabs;
    public UnitManager selectedUnit;
    public Camera globalCamera;
    public Tilemap tileMap;
    public int teamPlayersCount = 6;
    private Collider _tileMapCollider;
    public bool IsUnitMoving => CheckIsUnitMoving();
    public GameObject unitObject;
    private void Start()
    {
        _tileMapCollider = tileMap.GetComponent<Collider>();
    }

    // private UnitManager GetUnitFromPosition()
    // {
    //     
    // }

    public void ProceedTurn()
    {
        foreach (var unit in units)
        {
            selectedUnit = null;
            unit.ProceedToNextTurn();
        }
    }

    private void InitTeam(Vector3Int startCellPosition)
    {
        for (int i = 0; i < teamPlayersCount; i++)
        {
            InitUnitObject(startCellPosition, i);
        } 
    }
    
    public bool CheckIsAllUnitsMoved()
    {
        return units.All(unit => unit.moved);
    }
    
    public bool CheckIsUnitMoving()
    {
        return units.Any(unit => unit.isMoving);
    }

    private void InitUnitObject(Vector3Int startCellPosition, int i)
    {
        var pos = new Vector3Int(startCellPosition.x, startCellPosition.y, 0) + new Vector3Int(0, i, 0);
        var inst = Instantiate(unitPrefabs[0], tileMap.CellToWorld(pos), Quaternion.identity);
        var unitManager = inst.GetComponent<UnitManager>();
        unitManager.cellPosition = pos;
        units.Add(unitManager);
    }

    public void InitUnits()
    {
        var cellBounds = tileMap.cellBounds;
        var team1StartTile = cellBounds.min;
        var team2StartTile = new Vector3Int( -1  +cellBounds.max.x, -cellBounds.max.y, cellBounds.max.z);
        InitTeam(team1StartTile);
        InitTeam(team2StartTile);
    }

    private Vector3? GetMouseWorldClick(Vector3 mouseCoordinates)
    {
        var ray = globalCamera.ScreenPointToRay(mouseCoordinates);
        if (!_tileMapCollider.Raycast(ray, out var hit, 300.0f)) return null;
        return hit.point; 
    }
    private Vector3Int? GetMouseCellClick(Vector3 mouseCoordinates)
    {
        return tileMap.WorldToCell(mouseCoordinates); 
    }

    private UnitManager getUnitOnTile(Vector3 mouseCoordinates)
    {
        var cell = GetMouseCellClick(mouseCoordinates);
        foreach (var unit in units)
        {
            if (unit.cellPosition.Equals(cell))
            {
                return unit;
                
            }
         
        }
        return null;
    }

    private bool CheckIsTheSameUnit(UnitManager unit)
    {
        if (selectedUnit && unit)
        {
            return unit.cellPosition.Equals(selectedUnit.cellPosition);
        }

        return false;
    }
    
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            var coords = Input.mousePosition;
            var mouseWorldClick = GetMouseWorldClick(coords);
            if (!mouseWorldClick.HasValue)
            {
                return;
                
            }
            var tileUnit = getUnitOnTile( (Vector3)mouseWorldClick);
            var cell = GetMouseCellClick((Vector3)mouseWorldClick);
            print(cell);
            if (selectedUnit &&  selectedUnit.isMoving)
            {
                return;
            }
            if (tileUnit && CheckIsTheSameUnit(tileUnit))
            {
                selectedUnit.HandleUnitClick();
                selectedUnit = null;
                return;
            }

            if (!tileUnit && selectedUnit)
            {
                var mouseCellClick = GetMouseCellClick((Vector3)mouseWorldClick);
                if (mouseCellClick.HasValue)
                {
                    var moveTo = tileMap.CellToWorld((Vector3Int )mouseCellClick);
                    selectedUnit.Move((Vector3Int) mouseCellClick, (Vector3) moveTo, tileMap);
                }

                return;
            }
            if (selectedUnit && tileUnit)
            {
                selectedUnit.HandleUnitClick();
                selectedUnit = tileUnit;
                tileUnit.HandleUnitClick();
                return;
            }
            if (tileUnit && !selectedUnit)
            {
                selectedUnit = tileUnit;
                selectedUnit.HandleUnitClick();
                return;
            }

            // var selectedUnit = GetUnitFromPosition();
        }
    }
}
