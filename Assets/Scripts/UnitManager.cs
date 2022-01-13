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
    private PuckManager _puckManager;
    protected override void Start()
    {
        base.Start();
        _renderer = GetComponent<Renderer>();
        _puckManager = GetComponent<PuckManager>();
    }

    protected virtual void Select()
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
    
    protected virtual void UnSelect()
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

    public void ShowUnit()
    {
        _renderer.enabled = true;
        gameObject.SetActive(true);
    }
    
    public void HideUnit()
    {
        _renderer.enabled = false;
        gameObject.SetActive(false);
    }

    public virtual void HandleUnitClick()
    {
        if (isSelected)
        {
            UnSelect();
            return;
        }
        Select();
    }

    public void ChangeColor(Color a)
    {
        _renderer.material.color = a;
    }

    public virtual void ProceedToNextTurn()
    {
        moved = false;
        UnSelect();
    }
    
    public virtual void Move(Vector3Int newCellPosition, Vector3 moveTo, Tilemap tilemap)
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
