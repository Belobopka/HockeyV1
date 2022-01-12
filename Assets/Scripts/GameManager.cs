using DefaultNamespace;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;		
	
	public class GameManager : MonoBehaviour
	{ 
		public static GameManager instance = null;
		public UnitsManager unitsManager;

		
		private BoardManager boardManager;

		private bool doingSetup = true;
		
		void Awake()
		{
			boardManager = GetComponent<BoardManager>();
			if (instance == null) instance = this;
            else if (instance != this) Destroy(gameObject);	
            
			DontDestroyOnLoad(gameObject);
			InitGame();
		}
		
        //Initializes the game for each level.
		void InitGame()
		{
			doingSetup = true;
			boardManager.SetupScene();
			unitsManager.InitUnits();
			doingSetup = false;
		}
		
		public void ProceedTurn() {}
		

		//Update is called every frame.
		void Update()
		{
			// if (Input.GetMouseButtonDown(0))
			// {
			// 	var mousePosition = Input.mousePosition;
			// 	print("mousePosition");
			// 	print(mousePosition);
			//
			// 	var worldCameraPointRay = globalCamera.ScreenPointToRay(mousePosition);
			// 	var tileCoordinates = tilemap.WorldToCell(worldCameraPointRay.direction);
			// 	var cellCenter = tilemap.GetCellCenterWorld(tileCoordinates);
			// 	tilemap.SetTile(tileCoordinates, tileBase);
			// 	var tile = tilemap.GetTile(tileCoordinates);
			// 	print(cellCenter);
			// 	print(tile);
			// 	print(tileCoordinates);
			// }
		}
	}


