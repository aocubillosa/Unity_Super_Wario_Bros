using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public static LevelGenerator sharedInstance;
    public LevelBlock firstBlock;
    public List<LevelBlock> allLevelBlocks = new List<LevelBlock>(), currentBlocks = new List<LevelBlock>();
    public Transform levelStartPoint;

    void Awake() {
        sharedInstance = this;
    }

    void Start() {
        GenerateInitialBlocks();
    }

    public void GenerateInitialBlocks() {
        for (int i=0; i<2; i++) {
            AddLevelBlock();
        }
    }

    public void AddLevelBlock() {
        int randomIndex = Random.Range(0, allLevelBlocks.Count);
        LevelBlock currentBlock;
        Vector3 spawnPosition = Vector3.zero;
        if (currentBlocks.Count == 0) {
            currentBlock = (LevelBlock)Instantiate(firstBlock);
            currentBlock.transform.SetParent(this.transform, false);
            spawnPosition = levelStartPoint.position;
        }
        else {
            currentBlock = (LevelBlock)Instantiate(allLevelBlocks[randomIndex]);
            currentBlock.transform.SetParent(this.transform, false);
            spawnPosition = currentBlocks[currentBlocks.Count - 1].exitPoint.position;
        }
        Vector3 correction = new Vector3(spawnPosition.x - currentBlock.startPoint.position.x, spawnPosition.y - currentBlock.startPoint.position.y, 0);
        currentBlock.transform.position = correction;
        currentBlocks.Add(currentBlock);
    }

    public void RemoveOldLevelBlock() {
        LevelBlock oldBlock = currentBlocks[0];
        currentBlocks.Remove(oldBlock);
        Destroy(oldBlock.gameObject);
    }

    public void RemoveAllBlocks() {
        while (currentBlocks.Count > 0) {
            RemoveOldLevelBlock();
        }
    }
}