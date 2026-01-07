using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

// Esta clase se encarga de generar el mapa, inluyendo: instanciar habitaciones, buscar conexiones y enviarle a room que derribe tapones
public class MapGenerator : MonoBehaviour
{
    [Header("Room Prefab")]
    [SerializeField] private GameObject[] roomPrefabs;

    [Header("Ajustes del Mapa")]
    [SerializeField] public int difficulty;                 // Controla el número de salas
    [SerializeField] private float roomSize = 20f;          // Tamaño real de cada sala (escala 2,1,2)
    [SerializeField] private NavMeshSurface navMeshSurface;

    [Header ("Prefabs")]
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private GameObject orbPrefab;
    [SerializeField] private GameObject playerPrefab;

    [Header("Spawn")]
    [SerializeField] private GameObject playerSpawnRoomPrefab;  // sala especial donde empieza el jugador
    [SerializeField] private Transform playerTransform;         // referencia al jugador para moverlo al spawn

    // Número final de salas (derivado de dificultad)
    private int roomsToGenerate;

    // Diccionario que guarda todas las salas creadas
    // Key   -> posición en el grid
    // Value -> referencia a la Room
    private Dictionary<Vector2Int, Room> spawnedRooms = new Dictionary<Vector2Int, Room>();

    public static MapGenerator Instance { get; private set; }

    // Direcciones posibles de conexión
    private Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.right,
        Vector2Int.left
    };

    // Singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        GenerateMap();

        // Una vez instanciadas todas las salas, bakear el NavMesh
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }

        SpawnContents();
    }

    void GenerateMap()
    {
        Debug.Log($"Dificultad recibida: {difficulty}");

        // Cuántas salas habrá según dificultad (mínimo 4, dificultad añade)
        roomsToGenerate = 4 + difficulty * 3;

        Debug.Log($"Salas a generar al final: {roomsToGenerate}");

        spawnedRooms.Clear();

        // Crea sala inicial en el centro (modificado para que ahora sea sala de spawn)
        CreateRoom(Vector2Int.zero, playerSpawnRoomPrefab);

        // Hasta llegar al total...
        while (spawnedRooms.Count < roomsToGenerate)
        {
            // Elige una sala existente al azar
            List<Vector2Int> existingPositions = new List<Vector2Int>(spawnedRooms.Keys);
            Vector2Int basePos = existingPositions[Random.Range(0, existingPositions.Count)];

            // Elige una dirección aleatoria
            Vector2Int dir = directions[Random.Range(0, directions.Length)];
            Vector2Int newPos = basePos + dir;

            // Si ya existe una sala ahí, ignorar y reintentar
            if (spawnedRooms.ContainsKey(newPos))
                continue;

            // ¿Todo bien? Nueva sala
            CreateRoom(newPos);
        }

        // Una vez se hayan creado, pasa a abrir conexiones
        UpdateRoomSides();

        // Todo listo: spawnear jugador
        if (playerPrefab != null)
        {
            Room spawnRoom = spawnedRooms[Vector2Int.zero];

            //¿ Hay punto de spawn asignado? Ahí. ¿No? Lo calcula automáticamente
            Vector3 spawnPos = spawnRoom.playerSpawnPoint != null
                ? spawnRoom.playerSpawnPoint.position
                : spawnRoom.transform.position + Vector3.up * 1.5f;

            GameObject playerGO = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        }
    }

    // null para parametro opcional de habitacion (aka, para pasarle la de spawn sin liar lo de generar randoms)
    void CreateRoom(Vector2Int gridPos, GameObject prefab = null)
    {
        // Elegir prefab aleatorio de la lista
        if(prefab == null)
        prefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];

        // Convertir la posición de cuadrícula a coordenadas del mundo
        Vector3 worldPos = new Vector3(
            gridPos.x * roomSize,
            0f,
            gridPos.y * roomSize
        );

        // Instanciar la sala y obtener el script Room
        GameObject roomGO = Instantiate(prefab, worldPos, Quaternion.identity, transform);
        Room room = roomGO.GetComponent<Room>();

        // [!] Por favor que se me deje de olvidar de una vez...
        if (room == null)
        {
            Debug.LogWarning($"El prefab {prefab.name} no tiene el script Room.cs");
            return;
        }

        // Guardar la posición de cuadrícula en la sala
        room.gridPosition = gridPos;

        // Registrar sala en el diccionario
        spawnedRooms.Add(gridPos, room);
    }

    void UpdateRoomSides()
    {
        foreach (var roomEntry in spawnedRooms)
        {
            // roomEntry = Entrada en el diccionario
            Vector2Int roomGridPos = roomEntry.Key;
            Room room = roomEntry.Value;

            foreach (Vector2Int dir in directions)
            {
                Vector2Int neighborPos = roomGridPos + dir;

                bool hasNeighbor = spawnedRooms.ContainsKey(neighborPos);

                // Si NO hay vecino -> activamos pared
                // Si SÍ hay vecino -> desactivamos pared
                room.SetSideActive(dir, !hasNeighbor);
            }
        }
    }

    void SpawnContents()
    {
        Debug.Log("Adelante gacheto-SpawnContents.");
        foreach (Room room in spawnedRooms.Values)
        {
            // ENEMIGOS
            foreach (Transform spawn in room.enemySpawnPoints)
            {
                GameObject npcGO = Instantiate(npcPrefab, spawn.position, spawn.rotation);
                NPCPatrol npc = npcGO.GetComponent<NPCPatrol>();

                Debug.Log("Enemigo spawneado.");

                if (npc != null && room.patrolPoints.Length > 0)
                {
                    npc.SetPatrolPoints(room.patrolPoints);
                }
            }

            // ORBES
            foreach (Transform spawn in room.orbSpawnPoints)
            {
                Instantiate(orbPrefab, spawn.position, spawn.rotation);
                Debug.Log("Orbe spawneado.");
            }
        }
    }
}
