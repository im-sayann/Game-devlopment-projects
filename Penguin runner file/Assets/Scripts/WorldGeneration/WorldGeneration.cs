using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    //Gameplay
    private float ChunkSpawnZ;
    private Queue<Chunk> activeChunks = new Queue<Chunk>();
    private List<Chunk> chunkPool = new List<Chunk>();

    //Configurable fields
    [SerializeField] private int firstChunkSpawnPosition = -10;
    [SerializeField] private int chunkOnScreen = 5;
    [SerializeField] private float deSpawnDistance = 5.0f;

    [SerializeField] private List<GameObject> chunkPrefab;
    [SerializeField] private Transform cameraTransform;
        
    private void Awake()
    {
        ResetWorld();
    }
    
    public void Start()
    {
        if (chunkPrefab.Count == 0)
        {
            Debug.LogError("No chunk prefab found on the world generator, please assign some chunks!");
            return;
        }

        if (!cameraTransform)
        {
            cameraTransform = Camera.main.transform;
            Debug.Log("Wr've assign cameraTransform automatically to the Camera.main");
        }


    }
    public  void ScanPosition()
    {
        float cameraZ = cameraTransform.position.z;
        Chunk lastChunk = activeChunks.Peek();

        if (cameraZ >= lastChunk.transform.position.z + lastChunk.chunkLength + deSpawnDistance)
        {
            SpawnNewChunk();
            DeleteLastChunk();
        }
    }
    private void SpawnNewChunk()
    {
        int randomIndex = Random.Range(0, chunkPrefab.Count);

        Chunk chunk = chunkPool.Find(x => !x.gameObject.activeSelf && x.name == (chunkPrefab[randomIndex].name + "(Clone)"));

        if (!chunk)
        {
            GameObject go = Instantiate(chunkPrefab[randomIndex], transform);
            chunk = go.GetComponent<Chunk>();
        }

        chunk.transform.position = new Vector3(0, 0, ChunkSpawnZ);
        ChunkSpawnZ += chunk.chunkLength;

        activeChunks.Enqueue(chunk);
        chunk.ShowChunk();
    }
    private void DeleteLastChunk()
    {
        Chunk chunk = activeChunks.Dequeue();
        chunk.HideChunk();
        chunkPool.Add(chunk);
    }
    public void ResetWorld()
    {
        ChunkSpawnZ = firstChunkSpawnPosition;

        for (int i = activeChunks.Count; i != 0; i--)
            DeleteLastChunk();

        for (int i = 0; i < chunkOnScreen; i++)
            SpawnNewChunk();
    }
}

