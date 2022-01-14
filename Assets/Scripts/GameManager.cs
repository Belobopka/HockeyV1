using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;		
	
	public class GameManager : MonoBehaviour
	{ 
		public static GameManager instance = null;
		private UnitsManager _unitsManager;
		private PuckManager _puckManager;
		
		private BoardManager _boardManager;
		
		void Awake()
		{
			_puckManager = GetComponent<PuckManager>();
			_unitsManager = GetComponent<UnitsManager>();
			_boardManager = GetComponent<BoardManager>();
			if (instance == null) instance = this;
            else if (instance != this) Destroy(gameObject);	
            
			DontDestroyOnLoad(gameObject);
			InitGame();
		}
		
        //Initializes the game for each level.
		void InitGame()
		{
			_boardManager.SetupScene();
			_unitsManager.InitUnits();
		}
		
		public void ProceedTurn() {}
		

		//Update is called every frame.
		void Update()
		{
			_puckManager.HandleUpdate();
			_unitsManager.HandleUpdate();
		}
	}


