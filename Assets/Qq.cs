using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

public class Qq : MonoBehaviour
{
  [SerializeField] private Tile tile;

  private Tilemap _tilemap;
  private Collider _collider;

  private void Awake()
  {
    _tilemap = GetComponentInChildren<Tilemap>();
    _collider = GetComponentInChildren<Collider>();
  }

  private void Start()
  {
    Debug.DrawLine(Vector3.zero, new Vector3(5, 0, 0), Color.white, 2.5f);
    var bounds = _tilemap.cellBounds;
    var allPositionsWithin = bounds.allPositionsWithin;
    foreach (var within in allPositionsWithin)
    {
      CreateCordsText(within);
    }
  }

  private Vector3? _tileVector = null;

  private void Update()
  {
    if (Input.GetButtonDown("Fire1"))
    {
      var tilemap = GetComponentInChildren<Tilemap>();

      var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
      Debug.DrawRay(ray.origin, ray.direction * 15, Color.yellow);
      if (!_collider.Raycast(ray, out var hit, 300.0f)) return;
      var cell = tilemap.WorldToCell(hit.point);
      if (_tileVector is null)
      {
        _tileVector = tilemap.CellToWorld(cell);
      }
      else
      {
        CubeLinedraw((Vector3) _tileVector, tilemap.CellToWorld(cell));
      }
      tilemap.SetTile(new Vector3Int(cell.x, cell.y, cell.z), tile);

      var cube = GridCoordinates.UnityCellToCube(cell);
      // tilemap.SetTile(GridCoordinates.CubeToUnityCell(cube + GridCoordinates.VecDl), tile);
      // tilemap.SetTile(GridCoordinates.CubeToUnityCell(cube + GridCoordinates.VecDr), tile);
      // tilemap.SetTile(GridCoordinates.CubeToUnityCell(cube + GridCoordinates.VecR), tile);
      // tilemap.SetTile(GridCoordinates.CubeToUnityCell(cube + GridCoordinates.VecUr), tile);
      // tilemap.SetTile(GridCoordinates.CubeToUnityCell(cube + GridCoordinates.VecUl), tile);
      // tilemap.SetTile(GridCoordinates.CubeToUnityCell(cube + GridCoordinates.VecL), tile);
    }
  }
  
  private void CubeLinedraw(Vector3 a, Vector3 b)
  {
    Debug.DrawLine(a, b, Color.red, 100f);
    var N = GridCoordinates.CubeDistance(a, b);

    for (int i = 0; i < N * 2; i++)
    {
      _tilemap.SetTile(_tilemap.WorldToCell(GridCoordinates.CubeLerp(a, b, 0.5f / N * i)), tile);
    }
  }



  private void CreateCordsText(Vector3Int cord)
  {
    var text = new GameObject();
    text.transform.position = _tilemap.CellToWorld(new Vector3Int(cord.x, cord.y, 0));
    text.transform.Rotate(90f, 0, 0);
    text.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    var w = text.AddComponent<TextMesh>();
    var cube2 = GridCoordinates.UnityCellToCube(new Vector3Int(cord.x, cord.y, 0));

    w.text = "x:" + cord.x + " y:" + cord.y + $"\n x:{cube2.x} y:{cube2.y} z:{cube2.z}";
  }
}