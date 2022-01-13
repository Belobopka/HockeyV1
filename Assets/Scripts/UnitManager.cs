using System;
using UnityEngine.Tilemaps;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class UnitManager: MovingObject
{
    public Vector3Int cellPosition;
    private Vector3 worldPosition;
    public bool isSelected;
    public bool moved;
    private Renderer _renderer;
    protected override void Start()
    {
        base.Start();
        _renderer = GetComponent<Renderer>();
    }

    private void Select()
    {
        isSelected = true;
        if (moved)
        {
            _renderer.material.color = Color.red;  
        }
        else
        {
            _renderer.material.color = Color.white;  
        }
        
        transform.localScale =new Vector3(0.3f, 0.3f, 0.3f);
    }
    
    private void UnSelect()
    {
        isSelected = false;
        if (moved)
        {
            _renderer.material.color = Color.red;  
        }
        else
        {
            _renderer.material.color = Color.white;  
        }
        transform.localScale =new Vector3(0.1f, 0.1f, 0.1f);
    }

    public void HandleUnitClick()
    {
        if (isSelected)
        {
            UnSelect();
            return;
        }
        Select();
    }

    public void ProceedToNextTurn()
    {
        moved = false;
        UnSelect();
    }
    
    public void Move(Vector3Int newCellPosition, Vector3 moveTo, Tilemap tilemap)
    {
        
        // if (moved)
        // {
        //     return;
        // }
        var cellCenter = tilemap.GetCellCenterWorld(newCellPosition);
        cellPosition = newCellPosition;
        moved = true;
        _renderer.material.color = Color.red;
        base.Move(moveTo);
    }
    
    private void Update()
    {
    }
}
