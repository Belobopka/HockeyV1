using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;
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
    private GridManager _gridManager;
    public List<Vector3Int> availableTiles;
    // public List<Vector3Int> allYCells;
    public List<Vector3Int> newList;
    public bool IsUnitMoving => CheckIsUnitMoving();
    
    private void Start()
    {
        _tileMapCollider = tileMap.GetComponent<Collider>();
        _gridManager = GetComponent<GridManager>();
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
    
    public bool CheckIsAllUnitsMoved()
    {
        return units.All(unit => unit.moved);
    }
    
    public bool CheckIsUnitMoving()
    {
        return units.Any(unit => unit.isMoving);
    }

    private void InitUnitObject(Vector3Int cellPosition)
    {
        var inst = Instantiate(unitPrefabs[0], tileMap.CellToWorld(cellPosition), Quaternion.identity);
        var unitManager = inst.GetComponent<UnitManager>();
        unitManager.cellPosition = cellPosition;
        units.Add(unitManager);
    }

    private List<Vector3Int> GetAllAvailableCells()
    {
        var tileCellLocations = new List<Vector3Int>();

        foreach (var pos in tileMap.cellBounds.allPositionsWithin)
        {   
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            if (tileMap.HasTile(localPlace))
            {
                tileCellLocations.Add(pos);
            }
        }

        return tileCellLocations;
    }
    
    private void InitTeam(List<Vector3Int> cells)
    {
        for (var i = 0; i < cells.Count; i++)
        {
            InitUnitObject(cells[i]);
        } 
    }

    private List<Vector3Int> PrepareTeam1Tiles( List<Vector3Int> allAvailableCellPositions)
    {
        var tileCellLocations = new List<Vector3Int>();

        var allYCells = new List<Vector3Int>(GetYCells(allAvailableCellPositions));
        
        var yCount = allYCells.Count;
        
        while (yCount < teamPlayersCount)
        { 
           RemoveCellsFromList(allAvailableCellPositions, allYCells[allYCells.Count - 1].x );
           var newListok = GetYCells(allAvailableCellPositions);
            foreach (var item in newListok)
            {
                allYCells.Add(item);
            }

            yCount = allYCells.Count;
        }
        for (var i = 0; i < teamPlayersCount; i++)
        {
            tileCellLocations.Add(allYCells[i]);
        }
        
        return tileCellLocations;
    }
    
    private List<Vector3Int> GetYCells(List<Vector3Int> list)
    {
        int? y = null;
        var newYCells = new List<Vector3Int>();
        foreach (var t in list)
        {
            if (y != t.y)
            {
                newYCells.Add(t);
                y = t.y;
            }
            
        }

        return newYCells;
    }

    private void RemoveCellsFromList(List<Vector3Int> list, int x)
    {
        list.RemoveAll(delegate(Vector3Int i) { return i.x == x; });
    }



    public void InitUnits()
    {
        var allAvailableCellPositions = GetAllAvailableCells();
        var team1Tiles = PrepareTeam1Tiles(allAvailableCellPositions);
        availableTiles = allAvailableCellPositions;
        var team2Cells = new List<Vector3Int>();
        foreach (var cell in allAvailableCellPositions)
        {
            team2Cells.Add(cell);
        }
        team2Cells.Reverse();
        var team2Tiles = PrepareTeam1Tiles(team2Cells);
        InitTeam(team1Tiles);
        InitTeam(team2Tiles);
    }

    public void ClearSelectedUnit()
    {
        if (selectedUnit)
        {
            selectedUnit.HandleUnitClick();
            selectedUnit = null;     
        }

    }

    public UnitManager getUnitOnTile(Vector3 mouseCoordinates)
    {
        var cell = _gridManager.GetMouseCellClick(mouseCoordinates);
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
            var mouseWorldClick = _gridManager.GetMouseWorldClick(coords);
            
            if (!mouseWorldClick.HasValue)
            {
                return;
                
            }
            
            var tileUnit = getUnitOnTile((Vector3)mouseWorldClick);
            
            var cellPosition = _gridManager.GetMouseCellClick((Vector3)mouseWorldClick);
  
            var hasCell = tileMap.GetTile((Vector3Int)cellPosition);
            if (!hasCell)
            {
                return;
            }
            if (selectedUnit && selectedUnit.isMoving)
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
                var mouseCellClick = _gridManager.GetMouseCellClick((Vector3)mouseWorldClick);
                var moveTo = tileMap.CellToWorld((Vector3Int )mouseCellClick);
                selectedUnit.Move((Vector3Int) mouseCellClick, (Vector3) moveTo, tileMap);
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
