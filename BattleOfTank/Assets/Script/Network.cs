using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class Network : MonoBehaviour
{
	public static Network instance;
	public Canvas canvas;
	public SocketIOComponent socket;
	public InputField playerNameInput;
	public GameObject player;
	public GameObject maincamera;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start()
	{
		//subscribe to all the various websocket events
		socket.On("enemies", OnEnemies);
		socket.On("other player connected", OnOtherPlayerConnected);
		socket.On("play", OnPlay);
		socket.On("player move", OnPlayerMove);
		socket.On("player turn", OnPlayerTurn);
		socket.On("barrel turn", OnBarrelTurn);
		socket.On("player shoot", OnPlayerShoot);
		socket.On("health", OnHealth);
		socket.On("other player disconnected", OnOtherPlayerDisconnect);
	}

	public void JoinGame()
	{
		StartCoroutine(ConnectToServer());
	}

	#region Commands

	IEnumerator ConnectToServer()
	{
		yield return new WaitForSeconds(0.5f);

		socket.Emit("player connect");

		yield return new WaitForSeconds(1f);

		string playerName = playerNameInput.text;
		List<SpawnPoint> playerSpawnPoints = GetComponent<PlayerSpawner>().playerSpawnPoints;
		List<SpawnPoint> enemySpawnPoints = GetComponent<EnemySpawner>().enemySpawnPoints;
		PlayerJSON playerJSON = new PlayerJSON(playerName, playerSpawnPoints, enemySpawnPoints);
		string data = JsonUtility.ToJson(playerJSON);
		socket.Emit("play", new JSONObject(data));
		canvas.transform.Find("Panel").Find("MultiplayerMenu").gameObject.SetActive(false);
	}

	public void CommandMove(Vector3 vec3)
	{
		string data = JsonUtility.ToJson(new PositionJSON(vec3));;
		socket.Emit("player move", new JSONObject(data));
	}

	public void CommandTurn(Quaternion quat)
	{
		string data = JsonUtility.ToJson(new RotationJSON(quat));
		socket.Emit("player turn", new JSONObject(data));
	}

	public void CommandBarrel(Quaternion quat)
	{
		string data = JsonUtility.ToJson(new BarrelRotationJSON(quat));
		socket.Emit("barrel turn", new JSONObject(data));
	}

	public void CommandShoot()
	{
		print("Shoot");
		socket.Emit("player shoot");
	}

	public void CommandHealthChange(GameObject playerFrom, GameObject playerTo, int healthChange, bool isEnemy)
	{
		print("health change cmd");
		HealthChangeJSON healthChangeJSON = new HealthChangeJSON(playerTo.name, healthChange, playerFrom.name, isEnemy);
		socket.Emit("health", new JSONObject(JsonUtility.ToJson(healthChangeJSON)));
	}

	#endregion

	#region Listening

	void OnEnemies(SocketIOEvent socketIOEvent)
	{
		EnemiesJSON enemiesJSON = EnemiesJSON.CreateFromJSON(socketIOEvent.data.ToString());
		EnemySpawner es = GetComponent<EnemySpawner>();
		es.SpawnEnemies(enemiesJSON);
	}

	void OnOtherPlayerConnected(SocketIOEvent socketIOEvent)
	{
		print("Someone else joined");
		string data = socketIOEvent.data.ToString();
		UserJSON userJSON = UserJSON.CreateFromJSON(data);
		Vector3 position = new Vector3(float.Parse(userJSON.position[0]), float.Parse(userJSON.position[1]), float.Parse(userJSON.position[2]));
		Quaternion rotation = Quaternion.Euler(float.Parse(userJSON.rotation[0]), float.Parse(userJSON.rotation[1]), float.Parse(userJSON.rotation[2]));
		GameObject o = GameObject.Find(userJSON.name) as GameObject;
		if (o != null)
		{
			return;
		}
		GameObject p = Instantiate(player, position, rotation) as GameObject;
		// here we are setting up their other fields name and if they are local
		TankMovement pc = p.GetComponent<TankMovement>();
		//Transform t = p.transform.Find("Healthbar Canvas");
		//Transform t1 = t.transform.Find("Player Name");
		//Text playerName = t1.GetComponent<Text>();
		//playerName.text = userJSON.name;
		pc.isLocalPlayer = false;
		p.name = userJSON.name;
		p.transform.Find("HealthCanvas").Find("NameText").GetComponent<Text>().text = userJSON.name;
		// we also need to set the health
		Health h = p.GetComponent<Health>();
		h.currentHealth = userJSON.health;
		h.OnChangeHealth();
	}
	void OnPlay(SocketIOEvent socketIOEvent)
	{
		print("you joined");
		string data = socketIOEvent.data.ToString();
		UserJSON currentUserJSON = UserJSON.CreateFromJSON(data);
		Vector3 position = new Vector3(float.Parse(currentUserJSON.position[0]), float.Parse(currentUserJSON.position[1]), float.Parse(currentUserJSON.position[2]));
		Quaternion rotation = Quaternion.Euler(float.Parse(currentUserJSON.rotation[0]), float.Parse(currentUserJSON.rotation[1]), float.Parse(currentUserJSON.rotation[2]));
		GameObject p = Instantiate(player, position, rotation) as GameObject;
		TankMovement pc = p.GetComponent<TankMovement>();
		//Transform t = p.transform.Find("Healthbar Canvas");
		//Transform t1 = t.transform.Find("Player Name");
		//Text playerName = t1.GetComponent<Text>();
		//playerName.text = currentUserJSON.name;
		pc.isLocalPlayer = true;
		p.name = currentUserJSON.name;
		p.transform.Find("HealthCanvas").Find("NameText").GetComponent<Text>().text = currentUserJSON.name;
		maincamera.SetActive(false);
	}
	void OnPlayerMove(SocketIOEvent socketIOEvent)
	{
		string data = socketIOEvent.data.ToString();
		//Debug.Log("on Player Move : " + data);
		UserJSON userJSON = UserJSON.CreateFromJSON(data);
		Vector3 position = new Vector3(float.Parse(userJSON.position[0]), float.Parse(userJSON.position[1]), float.Parse(userJSON.position[2]));
		// if it is the current player exit
		if (userJSON.name == playerNameInput.text)
		{
			return;
		}
		GameObject p = GameObject.Find(userJSON.name) as GameObject;

		if (p != null)
		{
			//Debug.Log(position);
			p.transform.position = position;
		}
	}
	void OnPlayerTurn(SocketIOEvent socketIOEvent)
	{
		string data = socketIOEvent.data.ToString();
		UserJSON userJSON = UserJSON.CreateFromJSON(data);
		Quaternion rotation = Quaternion.Euler(float.Parse(userJSON.rotation[0]), float.Parse(userJSON.rotation[1]), float.Parse(userJSON.rotation[2]));
		// if it is the current player exit
		if (userJSON.name == playerNameInput.text)
		{
			return;
		}
		GameObject p = GameObject.Find(userJSON.name) as GameObject;
		if (p != null)
		{
			p.transform.rotation = rotation;
		}
	}
	void OnBarrelTurn(SocketIOEvent socketIOEvent)
	{
		string data = socketIOEvent.data.ToString();
		UserJSON userJSON = UserJSON.CreateFromJSON(data);
		Quaternion rotation = Quaternion.Euler(float.Parse(userJSON.barrelrotation[0]),
			float.Parse(userJSON.barrelrotation[1]),
			float.Parse(userJSON.barrelrotation[2]));
		// if it is the current player exit
		if (userJSON.name == playerNameInput.text)
		{
			return;
		}
		GameObject p = GameObject.Find(userJSON.name).transform.Find("BarrelPivot").gameObject as GameObject;
		if (p != null)
		{
			p.transform.rotation = rotation;
		}
	}
	void OnPlayerShoot(SocketIOEvent socketIOEvent)
	{
		string data = socketIOEvent.data.ToString();
		ShootJSON shootJSON = ShootJSON.CreateFromJSON(data);
		//find the gameobject
		GameObject p = GameObject.Find(shootJSON.name);
		// instantiate the bullet etc from the player script
		TankMovement pc = p.GetComponent<TankMovement>();
		pc.CmdFire();
	}

	void OnHealth(SocketIOEvent socketIOEvent)
	{
		print("changing the health");
		// get the name of the player whose health changed
		string data = socketIOEvent.data.ToString();
		UserHealthJSON userHealthJSON = UserHealthJSON.CreateFromJSON(data);
		GameObject p = GameObject.Find(userHealthJSON.name);
		Health h = p.GetComponent<Health>();
		h.currentHealth = userHealthJSON.health;
		h.OnChangeHealth();
	}

	void OnOtherPlayerDisconnect(SocketIOEvent socketIOEvent)
	{
		print("user disconnected");
		string data = socketIOEvent.data.ToString();
		UserJSON userJSON = UserJSON.CreateFromJSON(data);
		Destroy(GameObject.Find(userJSON.name));
	}
    #endregion

    #region JSONMessageClasses

    [Serializable]
	public class PlayerJSON
	{
		public string name;
		public List<PointJSON> playerSpawnPoints;
		public List<PointJSON> enemySpawnPoints;

		public PlayerJSON(string _name, List<SpawnPoint> _playerSpawnPoints, List<SpawnPoint> _enemySpawnPoints)
		{
			playerSpawnPoints = new List<PointJSON>();
			enemySpawnPoints = new List<PointJSON>();
			name = _name;
			foreach (SpawnPoint playerSpawnPoint in _playerSpawnPoints)
			{
				PointJSON pointJSON = new PointJSON(playerSpawnPoint);
				playerSpawnPoints.Add(pointJSON);
			}
			foreach (SpawnPoint enemySpawnPoint in _enemySpawnPoints)
			{
				PointJSON pointJSON = new PointJSON(enemySpawnPoint);
				enemySpawnPoints.Add(pointJSON);
			}
		}
	}

	[Serializable]
	public class PointJSON
	{
		public string[] position;
		public string[] rotation;
		public PointJSON(SpawnPoint spawnPoint)
		{
			position = new string[] {
				spawnPoint.transform.position.x.ToString(),
				spawnPoint.transform.position.y.ToString(),
				spawnPoint.transform.position.z.ToString()
			};
			rotation = new string[] {
				spawnPoint.transform.eulerAngles.x.ToString(),
				spawnPoint.transform.eulerAngles.y.ToString(),
				spawnPoint.transform.eulerAngles.z.ToString()
			};
		}
	}


	[Serializable]
	public class PositionJSON
	{
		public string[] position;

		public PositionJSON(Vector3 _position)
		{
			position = new string[] { _position.x.ToString(), _position.y.ToString(), _position.z.ToString() };
		}
	}

	[Serializable]
	public class RotationJSON
	{
		public string[] rotation;

		public RotationJSON(Quaternion _rotation)
		{
			rotation = new string[] { _rotation.eulerAngles.x.ToString(),
				_rotation.eulerAngles.y.ToString(),
				_rotation.eulerAngles.z.ToString() };
		}
	}

	[Serializable]
	public class BarrelRotationJSON
	{
		public string[] rotation;

		public BarrelRotationJSON(Quaternion _rotation)
		{
			rotation = new string[] { _rotation.eulerAngles.x.ToString(),
				_rotation.eulerAngles.y.ToString(),
				_rotation.eulerAngles.z.ToString() };
		}
	}

	[Serializable]
	public class UserJSON
	{
		public string name;
		public string[] position;
		public string[] rotation;
		public string[] barrelrotation;
		public int health;

		public static UserJSON CreateFromJSON(string data)
		{
			return JsonUtility.FromJson<UserJSON>(data);
		}
	}

	[Serializable]
	public class HealthChangeJSON
	{
		public string name;
		public int healthChange;
		public string from;
		public bool isEnemy;

		public HealthChangeJSON(string _name, int _healthChange, string _from, bool _isEnemy)
		{
			name = _name;
			healthChange = _healthChange;
			from = _from;
			isEnemy = _isEnemy;
		}
	}

	[Serializable]
	public class EnemiesJSON
	{
		public List<UserJSON> enemies;

		public static EnemiesJSON CreateFromJSON(string data)
		{
			return JsonUtility.FromJson<EnemiesJSON>(data);
		}
	}

	[Serializable]
	public class ShootJSON
	{
		public string name;

		public static ShootJSON CreateFromJSON(string data)
		{
			return JsonUtility.FromJson<ShootJSON>(data);
		}
	}

	[Serializable]
	public class UserHealthJSON
	{
		public string name;
		public int health;

		public static UserHealthJSON CreateFromJSON(string data)
		{
			return JsonUtility.FromJson<UserHealthJSON>(data);
		}
	}


	#endregion
}
