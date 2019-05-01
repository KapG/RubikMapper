using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridMapper : EditorWindow
{
    private bool autoMapper;
    private string currentScene = null;
    private PathValidation pathValidator;

    //Arrays with tiles organized by type
    private GameObject[] levelTiles;
    private GameObject[] spawnTiles;
    private GameObject[] goalTiles;
    private GameObject[] cornerTiles;
    private GameObject[] wayUpTiles;
    private GameObject[] wayDownTiles;

    //Level size variables
    private int sizeX = 1;
    private int sizeY = 1;
    private int sizeZ = 1;
    private int blockSize = 1;

    //Level Type Dropdown Variables
    private int selectedLevelType = 0;
    public string[] levelTypes = new string[] { "Dungeon", "NW_Dungeon" };

    [MenuItem("Window/Grid Mapper")]
    public static void ShowWindow()
    {
        GetWindow(typeof(GridMapper));
    }

    private void OnEnable()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }

    void OnValidate()
    {
        //Validate negative size
        if (sizeX < 3) { sizeX = 3; }
        if (sizeX > 25) { sizeX = 25; }
        if (sizeY > 25) { sizeY = 25; }
        if (sizeZ < 2) { sizeZ = 2; }
        if (sizeZ > 25) { sizeZ = 25; }
        if (blockSize < 1) { blockSize = 1; }
    }

    void OnGUI()
    {
        GUILayout.Label("Grid Mapper Settings for " + currentScene, EditorStyles.boldLabel);

        //Level Type Dropdown
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Level Type: ");
        selectedLevelType = EditorGUILayout.Popup(selectedLevelType, levelTypes);
        EditorGUILayout.EndHorizontal();

        //Block Type Dropdown
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Level Size: ");
        GUILayout.Label("X", GUILayout.Width(12));
        sizeX = EditorGUILayout.IntField(sizeX);
        GUILayout.Label("Y", GUILayout.Width(12));
        sizeY = EditorGUILayout.IntField(sizeY);
        GUILayout.Label("Z", GUILayout.Width(12));
        sizeZ = EditorGUILayout.IntField(sizeZ);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Block Size: ");
        blockSize = EditorGUILayout.IntField(blockSize);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Generate Level"))
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            GUI.FocusControl(null);
            OnValidate();
            GenerateLevel();

            watch.Stop();
            Debug.Log("Generation of the level took " + watch.Elapsed.TotalSeconds + " seconds");
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        autoMapper = EditorGUILayout.BeginToggleGroup("Automatic Mapper", autoMapper);
        if (GUILayout.Button("Save Settings"))
        {
            GUI.FocusControl(null);
            OnValidate();
        }
        EditorGUILayout.EndToggleGroup();
    }

    private void GenerateLevel()
    {
        GameObject[,,] levelLayout = new GameObject[sizeX, sizeY, sizeZ];
        Vector3[,,] layoutPositions = new Vector3[sizeX, sizeY, sizeZ];
        bool validateSpawn = false;
        bool validateGoal = false;
        pathValidator = new PathValidation();

        levelTiles = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>()
            .Where(g => g.tag == "LevelBlock")
            .Where(g => g.GetComponentInChildren<GridLevelBlock>().selectedBlockType == 0)
            .Where(g => g.GetComponentInChildren<GridLevelBlock>().selectedLevelType == selectedLevelType)
            .ToArray();
        spawnTiles = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>()
            .Where(g => g.tag == "LevelBlock")
            .Where(g => g.GetComponentInChildren<GridLevelBlock>().selectedBlockType == 1)
            .Where(g => g.GetComponentInChildren<GridLevelBlock>().selectedLevelType == selectedLevelType)
            .ToArray();
        goalTiles = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>()
            .Where(g => g.tag == "LevelBlock")
            .Where(g => g.GetComponentInChildren<GridLevelBlock>().selectedBlockType == 2)
            .Where(g => g.GetComponentInChildren<GridLevelBlock>().selectedLevelType == selectedLevelType)
            .ToArray();

        cornerTiles = pathValidator.FindPossibleCorner(ref levelTiles);
        wayUpTiles = pathValidator.FindPossibleWayUp(ref levelTiles);
        wayDownTiles = pathValidator.FindPossibleWayDown(ref levelTiles);

        if (cornerTiles.Count() < 1)
        {
            Debug.LogError("No corner blocks found");
        }

        if (spawnTiles.Count() != 0)
        {
            validateSpawn = true;
        }

        if (goalTiles.Count() != 0)
        {
            validateGoal = true;
        }

        if (validateSpawn && validateGoal)
        {
            // Handles the generation of key tiles like spawn and goal
            GenerateKeyTiles(levelLayout, layoutPositions);
            // Handles tiles that will give access to upper areas
            GenerateWayUp(levelLayout, layoutPositions);
            // Handles tiles that allow for access to the left or right
            GenerateCorners(levelLayout, layoutPositions);
            // Handles the way forward between the rest of the tiles
            GenerateRows(levelLayout, layoutPositions);
            // Intantiates the object of the generated level
            InstantiateLevel(levelLayout, layoutPositions);
        }
        else if (!validateSpawn && validateGoal)
        {
            Debug.LogError("Missing Spawn block");
        }
        else if (!validateGoal && validateSpawn)
        {
            Debug.LogError("Missing End block");
        }
        else
        {
            Debug.LogError("Missing Spawn and End blocks");
        }
    }

    private void GenerateKeyTiles(GameObject[,,] levelLayout, Vector3[,,] layoutPositions)
    {
        GenerateSpawnTile(levelLayout, layoutPositions);
        GenerateGoalTile(levelLayout, layoutPositions);
    }

    private void GenerateSpawnTile(GameObject[,,] levelLayout, Vector3[,,] layoutPositions)
    {
        GameObject[] randomSpawnTiles = pathValidator.GetRandomSortedArray(spawnTiles);
        foreach (GameObject spawnTile in randomSpawnTiles)
        {
            GridLevelBlock spawn = spawnTile.GetComponentInChildren<GridLevelBlock>();
            if (!spawn.backTopLeft && !spawn.backTop && !spawn.backTopRight
            && !spawn.backMidLeft && !spawn.backMid && !spawn.backMidRight
            && !spawn.backBottomLeft && !spawn.backBottom && !spawn.backBottomRight)
            {
                continue;
            }
            else
            {
                levelLayout[0, 0, 0] = spawnTile;
                layoutPositions[0, 0, 0] = new Vector3(blockSize * 0, blockSize * 0, blockSize * 0);
                break;
            }
        }

        if (levelLayout[0, 0, 0] == null)
        {
            Debug.LogError("Spawn point must have an exit on 'Back' face with rotation '0'");
        }
    }

    private void GenerateGoalTile(GameObject[,,] levelLayout, Vector3[,,] layoutPositions)
    {
        //int randomEnd = rnd.Next(endBlocks.Count());
        //GridLevelTile endPoint = endBlocks[randomEnd].GetComponentInChildren<GridLevelTile>();
        int x = sizeX - 1;
        int y = sizeY - 1;
        int z = sizeZ - 1;

        if (sizeY == 0)
        {
            //No roof for end point
            if (z % 2 == 0)
            {
                //Even row for end point
                GameObject[] randomGoalTiles = pathValidator.GetRandomSortedArray(goalTiles);
                foreach (GameObject goalTile in randomGoalTiles)
                {
                    GridLevelBlock goal = goalTile.GetComponentInChildren<GridLevelBlock>();
                    if (!goal.frontTopLeft && !goal.frontTop && !goal.frontTopRight
                    && !goal.frontMidLeft && !goal.frontMid && !goal.frontMidRight
                    && !goal.frontBottomLeft && !goal.frontBottom && !goal.frontBottomRight)
                    {
                        continue;
                    }
                    else
                    {
                        levelLayout[x, y, z] = goalTile;
                        layoutPositions[x, y, z] = new Vector3(blockSize * x, blockSize * y, blockSize * z);
                    }
                }

                if (levelLayout[x, y, z] == null)
                {
                    Debug.LogError("Failed to find a valid goal tile");
                }
            }
            else
            {
                //Odd row for end point
                GameObject[] randomGoalTiles = pathValidator.GetRandomSortedArray(goalTiles);
                foreach (GameObject goalTile in randomGoalTiles)
                {
                    GridLevelBlock goal = goalTile.GetComponentInChildren<GridLevelBlock>();
                    if (!goal.backTopLeft && !goal.backTop && !goal.backTopRight
                    && !goal.backMidLeft && !goal.backMid && !goal.backMidRight
                    && !goal.backBottomLeft && !goal.backBottom && !goal.backBottomRight)
                    {
                        continue;
                    }
                    else
                    {
                        levelLayout[0, y, z] = goalTile;
                        layoutPositions[0, y, z] = new Vector3(0, blockSize * y, blockSize * z);
                        break;
                    }
                }

                if (levelLayout[0, y, z] == null)
                {
                    Debug.LogError("Failed to find a valid goal tile");
                }
            }
        }
        else if (sizeY % 2 == 0)
        {
            //Even roof for end point
            GameObject[] randomGoalTiles = pathValidator.GetRandomSortedArray(goalTiles);
            foreach (GameObject goalTile in randomGoalTiles)
            {
                GridLevelBlock goal = goalTile.GetComponentInChildren<GridLevelBlock>();
                if (!goal.backTopLeft && !goal.backTop && !goal.backTopRight
                && !goal.backMidLeft && !goal.backMid && !goal.backMidRight
                && !goal.backBottomLeft && !goal.backBottom && !goal.backBottomRight)
                {
                    continue;
                }
                else
                {
                    levelLayout[0, y, 0] = goalTile;
                    layoutPositions[0, y, 0] = new Vector3(0, blockSize * y, 0);
                    break;
                }
            }

            if (levelLayout[0, y, 0] == null)
            {
                Debug.LogError("Failed to find a valid goal tile");
            }
        }
        else
        {
            //Odd roof for end point
            if (z % 2 == 0)
            {
                //Even row for end point
                GameObject[] randomGoalTiles = pathValidator.GetRandomSortedArray(goalTiles);
                foreach (GameObject goalTile in randomGoalTiles)
                {
                    GridLevelBlock goal = goalTile.GetComponentInChildren<GridLevelBlock>();
                    if (!goal.frontTopLeft && !goal.frontTop && !goal.frontTopRight
                    && !goal.frontMidLeft && !goal.frontMid && !goal.frontMidRight
                    && !goal.frontBottomLeft && !goal.frontBottom && !goal.frontBottomRight)
                    {
                        continue;
                    }
                    else
                    {
                        levelLayout[x, y, z] = goalTile;
                        layoutPositions[x, y, z] = new Vector3(blockSize * x, blockSize * y, blockSize * z);
                        break;
                    }
                }
            }
            else
            {
                //Odd row for end point
                GameObject[] randomGoalTiles = pathValidator.GetRandomSortedArray(goalTiles);
                foreach (GameObject goalTile in randomGoalTiles)
                {
                    GridLevelBlock goal = goalTile.GetComponentInChildren<GridLevelBlock>();
                    if (!goal.backTopLeft && !goal.backTop && !goal.backTopRight
                    && !goal.backMidLeft && !goal.backMid && !goal.backMidRight
                    && !goal.backBottomLeft && !goal.backBottom && !goal.backBottomRight)
                    {
                        GenerateGoalTile(levelLayout, layoutPositions);
                    }
                    else
                    {
                        levelLayout[0, y, z] = goalTile;
                        layoutPositions[0, y, z] = new Vector3(0, blockSize * y, blockSize * z);
                    }
                }
            }
        }
    }

    private void GenerateWayUp(GameObject[,,] levelLayout, Vector3[,,] layoutPositions)
    {
        int x = sizeX - 1;
        int z = sizeZ - 1;

        for (int i = 0; i < sizeY; i++)
        {
            if (i % 2 == 0)
            {
                if (z % 2 == 0 && levelLayout[x, i, z] == null)
                {
                    GameObject[] randomWayUpTiles = pathValidator.GetRandomSortedArray(wayUpTiles);
                    GameObject[] randomWayDownTiles = pathValidator.GetRandomSortedArray(wayDownTiles);

                    foreach (GameObject wayUpTile in randomWayUpTiles)
                    {
                        if (levelLayout[x, i, z] != null && levelLayout[x, i + 1, z] != null) { break; }

                        GridLevelBlock bottomTile = wayUpTile.GetComponentInChildren<GridLevelBlock>();

                        if (bottomTile.frontTopLeft || bottomTile.frontTop || bottomTile.frontTopRight
                        || bottomTile.frontMidLeft || bottomTile.frontMid || bottomTile.frontMidRight
                        || bottomTile.frontBottomLeft || bottomTile.frontBottom || bottomTile.frontBottomRight)
                        {
                            foreach (GameObject wayDownTiles in randomWayDownTiles)
                            {
                                GridLevelBlock upperTile = wayDownTiles.GetComponentInChildren<GridLevelBlock>();

                                if (upperTile.frontTopLeft || upperTile.frontTop || upperTile.frontTopRight
                                || upperTile.frontMidLeft || upperTile.frontMid || upperTile.frontMidRight
                                || upperTile.frontBottomLeft || upperTile.frontBottom || upperTile.frontBottomRight)
                                {
                                    if (pathValidator.ValidatePathUpwards(bottomTile, upperTile))
                                    {
                                        levelLayout[x, i, z] = wayUpTile;
                                        layoutPositions[x, i, z] = new Vector3(blockSize * x, blockSize * i, blockSize * z);

                                        levelLayout[x, i + 1, z] = wayDownTiles;
                                        layoutPositions[x, i + 1, z] = new Vector3(blockSize * x, blockSize * (i + 1), blockSize * z);
                                    }
                                }
                            }
                        }
                    }

                    if (levelLayout[x, i, z] == null || levelLayout[x, i + 1, z] == null)
                    {
                        Debug.LogError("Failed to find a way upwards for position X:" + x + " Y:" + i + " Z:" + z);
                    }
                }
                else if (z % 2 != 0 && levelLayout[0, i, z] == null)
                {
                    GameObject[] randomWayUpBlocks = pathValidator.GetRandomSortedArray(wayUpTiles);
                    GameObject[] randomWayDownBlocks = pathValidator.GetRandomSortedArray(wayDownTiles);

                    foreach (GameObject wayUpBlock in randomWayUpBlocks)
                    {
                        if (levelLayout[0, i, z] != null && levelLayout[0, i + 1, z] != null) { break; }

                        GridLevelBlock bottomTile = wayUpBlock.GetComponentInChildren<GridLevelBlock>();

                        if (bottomTile.backTopLeft || bottomTile.backTop || bottomTile.backTopRight
                        || bottomTile.backMidLeft || bottomTile.backMid || bottomTile.backMidRight
                        || bottomTile.backBottomLeft || bottomTile.backBottom || bottomTile.backBottomRight)
                        {
                            foreach (GameObject wayDownBlock in randomWayDownBlocks)
                            {
                                GridLevelBlock upperTile = wayDownBlock.GetComponentInChildren<GridLevelBlock>();

                                if (upperTile.backTopLeft || upperTile.backTop || upperTile.backTopRight
                                || upperTile.backMidLeft || upperTile.backMid || upperTile.backMidRight
                                || upperTile.backBottomLeft || upperTile.backBottom || upperTile.backBottomRight)
                                {
                                    if (pathValidator.ValidatePathUpwards(bottomTile, upperTile))
                                    {
                                        levelLayout[0, i, z] = wayUpBlock;
                                        layoutPositions[0, i, z] = new Vector3(0, blockSize * i, blockSize * z);

                                        levelLayout[0, i + 1, z] = wayDownBlock;
                                        layoutPositions[0, i + 1, z] = new Vector3(0, blockSize * (i + 1), blockSize * z);
                                    }
                                }
                            }
                        }
                    }

                    if (levelLayout[0, i, z] == null || levelLayout[0, i + 1, z] == null)
                    {
                        Debug.LogError("Failed to find a way upwards for position X:" + x + " Y:" + i + " Z:" + z);
                    }
                }
            }
            else
            {
                if (levelLayout[0, i, 0] == null)
                {
                    GameObject[] randomWayUpBlocks = pathValidator.GetRandomSortedArray(wayUpTiles);
                    GameObject[] randomWayDownBlocks = pathValidator.GetRandomSortedArray(wayDownTiles);

                    foreach (GameObject wayUpBlock in randomWayUpBlocks)
                    {
                        if (levelLayout[0, i, 0] != null && levelLayout[0, i + 1, 0] != null) { break; }

                        GridLevelBlock bottomTile = wayUpBlock.GetComponentInChildren<GridLevelBlock>();

                        if (bottomTile.backTopLeft || bottomTile.backTop || bottomTile.backTopRight
                        || bottomTile.backMidLeft || bottomTile.backMid || bottomTile.backMidRight
                        || bottomTile.backBottomLeft || bottomTile.backBottom || bottomTile.backBottomRight)
                        {
                            foreach (GameObject wayDownBlock in randomWayDownBlocks)
                            {
                                GridLevelBlock upperTile = wayDownBlock.GetComponentInChildren<GridLevelBlock>();

                                if (upperTile.backTopLeft || upperTile.backTop || upperTile.backTopRight
                                || upperTile.backMidLeft || upperTile.backMid || upperTile.backMidRight
                                || upperTile.backBottomLeft || upperTile.backBottom || upperTile.backBottomRight)
                                {
                                    if (pathValidator.ValidatePathUpwards(bottomTile, upperTile))
                                    {
                                        levelLayout[0, i, 0] = wayUpBlock;
                                        layoutPositions[0, i, 0] = new Vector3(0, blockSize * i, 0);

                                        levelLayout[0, i + 1, 0] = wayDownBlock;
                                        layoutPositions[0, i + 1, 0] = new Vector3(0, blockSize * (i + 1), 0);
                                    }
                                }
                            }
                        }
                    }

                    if (levelLayout[0, i, 0] == null || levelLayout[0, i + 1, 0] == null)
                    {
                        Debug.LogError("Failed to find a way upwards for position X:" + x + " Y:" + i + " Z:" + z);
                    }
                }
            }
        }
    }

    private void GenerateCorners(GameObject[,,] levelLayout, Vector3[,,] layoutPositions)
    {
        int x = sizeX - 1;
        for (int y = 0; y < sizeY; y++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                if (z % 2 == 0)
                {
                    if (levelLayout[0, y, z] == null)
                    {
                        GameObject[] randomCornerTiles = pathValidator.GetRandomSortedArray(cornerTiles);
                        foreach (GameObject cornerTile in randomCornerTiles)
                        {
                            GridLevelBlock tile = cornerTile.GetComponentInChildren<GridLevelBlock>();
                            if (!tile.backTopLeft && !tile.backTop && !tile.backTopRight
                            && !tile.backMidLeft && !tile.backMid && !tile.backMidRight
                            && !tile.backBottomLeft && !tile.backBottom && !tile.backBottomRight)
                            {
                                continue;
                            }
                            else
                            {
                                if (!tile.rightTopLeft && !tile.rightTop && !tile.rightTopRight
                                && !tile.rightMidLeft && !tile.rightMid && !tile.rightMidRight
                                && !tile.rightBottomLeft && !tile.rightBottom && !tile.rightBottomRight)
                                {
                                    continue;
                                }
                                else
                                {
                                    GridLevelBlock previousTile = levelLayout[0, y, z - 1].GetComponentInChildren<GridLevelBlock>();

                                    if (pathValidator.ValidatePathSideways(previousTile, tile))
                                    {
                                        levelLayout[0, y, z] = cornerTile;
                                        layoutPositions[0, y, z] = new Vector3(0, blockSize * y, blockSize * z);
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (levelLayout[x, y, z] == null)
                    {
                        GameObject[] randomCornerTiles = pathValidator.GetRandomSortedArray(cornerTiles);
                        foreach (GameObject cornerTile in randomCornerTiles)
                        {
                            GridLevelBlock tile = cornerTile.GetComponentInChildren<GridLevelBlock>();
                            if (!tile.frontTopLeft && !tile.frontTop && !tile.frontTopRight
                            && !tile.frontMidLeft && !tile.frontMid && !tile.frontMidRight
                            && !tile.frontBottomLeft && !tile.frontBottom && !tile.frontBottomRight)
                            {
                                continue;
                            }
                            else
                            {
                                if (!tile.leftTopLeft && !tile.leftTop && !tile.leftTopRight
                                && !tile.leftMidLeft && !tile.leftMid && !tile.leftMidRight
                                && !tile.leftBottomLeft && !tile.leftBottom && !tile.leftBottomRight)
                                {
                                    continue;
                                }
                                else
                                {
                                    levelLayout[x, y, z] = cornerTile;
                                    layoutPositions[x, y, z] = new Vector3(blockSize * x, blockSize * y, blockSize * z);
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (levelLayout[0, y, z] == null)
                    {
                        GameObject[] randomCornerTiles = pathValidator.GetRandomSortedArray(cornerTiles);
                        foreach (GameObject cornerTile in randomCornerTiles)
                        {
                            GridLevelBlock tile = cornerTile.GetComponentInChildren<GridLevelBlock>();
                            if (!tile.backTopLeft && !tile.backTop && !tile.backTopRight
                            && !tile.backMidLeft && !tile.backMid && !tile.backMidRight
                            && !tile.backBottomLeft && !tile.backBottom && !tile.backBottomRight)
                            {
                                continue;
                            }
                            else
                            {
                                if (!tile.leftTopLeft && !tile.leftTop && !tile.leftTopRight
                                && !tile.leftMidLeft && !tile.leftMid && !tile.leftMidRight
                                && !tile.leftBottomLeft && !tile.leftBottom && !tile.leftBottomRight)
                                {
                                    continue;
                                }
                                else
                                {
                                    levelLayout[0, y, z] = cornerTile;
                                    layoutPositions[0, y, z] = new Vector3(0, blockSize * y, blockSize * z);
                                    break;
                                }
                            }
                        }
                    }

                    if (levelLayout[x, y, z] == null)
                    {
                        GameObject[] randomCornerTiles = pathValidator.GetRandomSortedArray(cornerTiles);
                        foreach (GameObject cornerTile in randomCornerTiles)
                        {
                            GridLevelBlock cornerBlock = cornerTile.GetComponentInChildren<GridLevelBlock>();
                            if (!cornerBlock.frontTopLeft && !cornerBlock.frontTop && !cornerBlock.frontTopRight
                            && !cornerBlock.frontMidLeft && !cornerBlock.frontMid && !cornerBlock.frontMidRight
                            && !cornerBlock.frontBottomLeft && !cornerBlock.frontBottom && !cornerBlock.frontBottomRight)
                            {
                                continue;
                            }
                            else
                            {
                                if (!cornerBlock.rightTopLeft && !cornerBlock.rightTop && !cornerBlock.rightTopRight
                                && !cornerBlock.rightMidLeft && !cornerBlock.rightMid && !cornerBlock.rightMidRight
                                && !cornerBlock.rightBottomLeft && !cornerBlock.rightBottom && !cornerBlock.rightBottomRight)
                                {
                                    continue;
                                }
                                else
                                {
                                    GridLevelBlock previousTile = levelLayout[x, y, z - 1].GetComponentInChildren<GridLevelBlock>();

                                    if (pathValidator.ValidatePathSideways(previousTile, cornerBlock))
                                    {
                                        levelLayout[x, y, z] = cornerTile;
                                        layoutPositions[x, y, z] = new Vector3(blockSize * x, blockSize * y, blockSize * z);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void GenerateRows(GameObject[,,] levelLayout, Vector3[,,] layoutPositions)
    {
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int z = 0; z < sizeZ; z++)
                {

                    if (levelLayout[x, y, z] == null)
                    {
                        GameObject previousObject = levelLayout[x - 1, y, z];
                        GridLevelBlock previousBlock = previousObject.GetComponentInChildren<GridLevelBlock>();
                        GameObject nextObj = levelLayout[x + 1, y, z];
                        GridLevelBlock nextBlock = null;
                        if (nextObj != null)
                        {
                            nextBlock = nextObj.GetComponentInChildren<GridLevelBlock>();
                        }

                        GameObject[] randomLevelTiles = pathValidator.GetRandomSortedArray(levelTiles);
                        foreach (GameObject levelTile in randomLevelTiles)
                        {
                            GridLevelBlock levelBlock = levelTile.GetComponentInChildren<GridLevelBlock>();

                            if (pathValidator.ValidateRowPath(previousBlock, levelBlock, nextBlock))
                            {
                                levelLayout[x, y, z] = levelTile;
                                layoutPositions[x, y, z] = new Vector3(blockSize * x, blockSize * y, blockSize * z);
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        if (levelLayout[x, y, z] == null)
                        {
                            if (nextBlock != null)
                            {
                                Debug.LogError("Could not find a valid path forward between " + previousObject.name + " and " + nextObj.name);
                            }
                            else
                            {
                                Debug.LogError("Could not find a valid path forward after " + previousObject.name);
                            }

                        }
                    }

                }
            }
        }
    }

    private void InstantiateLevel(GameObject[,,] levelLayout, Vector3[,,] layoutPositions)
    {
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    GameObject block = levelLayout[x, y, z];
                    Vector3 position = layoutPositions[x, y, z];

                    if (block != null && position != null)
                    {
                        Instantiate(block, position, Quaternion.identity);
                    }
                }
            }
        }
    }
}