using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathValidation
{
    /// <summary>
    /// Validates if the current block can be accessed through the previous block
    /// </summary>
    /// <param name="previousBlock">The block in the previous position of the grid</param>
    /// <param name="currentBlock">The block we are currently validating</param>
    /// <returns></returns>
    public bool ValidateRowPath(GridLevelBlock previousBlock, GridLevelBlock currentBlock, GridLevelBlock nextBlock)
    {
        bool pathBackwards;
        bool pathForward;

        if (ValidatePathForward(previousBlock, currentBlock)) { pathBackwards = true; }
        else { pathBackwards = false; }

        if (nextBlock == null) { pathForward = true; }
        else
        {
            if (ValidatePathForward(currentBlock, nextBlock)) { pathForward = true; }
            else { pathForward = false; }
        }

        return pathBackwards && pathForward;
    }

    /// <summary>
    /// Validates if a corner block has path to the right connecting to the previous block
    /// </summary>
    /// <param name="previousBlock">The block in the previous position of the grid</param>
    /// <param name="currentBlock">The block we are currently validating</param>
    /// <param name="oddRow">Defines whether we should go right or left</param>
    /// <returns></returns>
    public bool ValidatePathSideways(GridLevelBlock previousBlock, GridLevelBlock currentBlock)
    {
        if ((previousBlock.leftTopLeft && currentBlock.rightTopRight) || (previousBlock.leftTop && currentBlock.rightTop) || (previousBlock.leftTopRight && currentBlock.rightTopLeft) ||
            (previousBlock.leftMidLeft && currentBlock.rightMidRight) || (previousBlock.leftMid && currentBlock.rightMid) || (previousBlock.leftMidRight && currentBlock.rightMidLeft) ||
            (previousBlock.leftBottomLeft && currentBlock.rightBottomRight) || (previousBlock.leftBottom && currentBlock.rightBottom) || (previousBlock.leftBottomRight && currentBlock.rightBottomLeft))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Validates if there is a way forward between 2 tiles
    /// </summary>
    /// <param name="bottomBlock"></param>
    /// <param name="upperBlock"></param>
    /// <returns></returns>
    public bool ValidatePathUpwards(GridLevelBlock bottomBlock, GridLevelBlock upperBlock)
    {
        if ((bottomBlock.topTopLeft && upperBlock.bottomTopRight) || (bottomBlock.topTop && upperBlock.bottomTop) || (bottomBlock.topTopRight && upperBlock.bottomTopLeft) ||
            (bottomBlock.topMidLeft && upperBlock.bottomMidRight) || (bottomBlock.topMid && upperBlock.bottomMid) || (bottomBlock.topMidRight && upperBlock.bottomMidLeft) ||
            (bottomBlock.topBottomLeft && upperBlock.bottomBottomRight) || (bottomBlock.topBottom && upperBlock.bottomBottom) || (bottomBlock.topBottomRight && upperBlock.bottomBottomLeft))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Return an array with tiles that have a path down
    /// </summary>
    /// <param name="levelTiles">Array containing all tiles</param>
    /// <returns></returns>
    public GameObject[] FindPossibleWayDown(ref GameObject[] levelTiles)
    {
        List<GameObject> downBlocksList = new List<GameObject>();
        List<GameObject> levelBlocksList = new List<GameObject>();

        for (int i = 0; i < levelTiles.Count(); i++)
        {
            GridLevelBlock b = levelTiles[i].GetComponentInChildren<GridLevelBlock>();
            if (b.bottomTopLeft || b.bottomTop || b.bottomTopRight
                || b.bottomMidLeft || b.bottomMid || b.bottomMidRight
                || b.bottomBottomLeft || b.bottomBottom || b.bottomBottomRight)
            {
                downBlocksList.Add(levelTiles[i]);
            }
            else
            {
                levelBlocksList.Add(levelTiles[i]);
            }
        }

        levelTiles = levelBlocksList.ToArray();
        return downBlocksList.ToArray();
    }

    /// <summary>
    /// Return an array with tiles that have a path down
    /// </summary>
    /// <param name="levelTiles">Array containing all tiles</param>
    /// <returns></returns>
    public GameObject[] FindPossibleWayUp(ref GameObject[] levelTiles)
    {
        List<GameObject> upBlocksList = new List<GameObject>();
        List<GameObject> levelBlocksList = new List<GameObject>();

        for (int i = 0; i < levelTiles.Count(); i++)
        {
            GridLevelBlock b = levelTiles[i].GetComponentInChildren<GridLevelBlock>();
            if (b.topTopLeft || b.topTop || b.topTopRight
                || b.topMidLeft || b.topMid || b.topMidRight
                || b.topBottomLeft || b.topBottom || b.topBottomRight)
            {
                upBlocksList.Add(levelTiles[i]);
            }
            else
            {
                levelBlocksList.Add(levelTiles[i]);
            }
        }

        levelTiles = levelBlocksList.ToArray();
        return upBlocksList.ToArray();
    }

    /// <summary>
    /// Return an array with tiles that connect sideways
    /// </summary>
    /// <param name="levelTiles"></param>
    /// <returns></returns>
    public GameObject[] FindPossibleCorner(ref GameObject[] levelTiles)
    {
        List<GameObject> cornerBlocksList = new List<GameObject>();
        List<GameObject> levelBlocksList = new List<GameObject>();

        for (int i = 0; i < levelTiles.Count(); i++)
        {
            GridLevelBlock b = levelTiles[i].GetComponentInChildren<GridLevelBlock>();
            if (b.frontTopLeft || b.frontTop || b.frontTopRight || b.frontMidLeft || b.frontMid || b.frontMidRight
                || b.frontBottomLeft || b.frontBottom || b.frontBottomRight || b.backTopLeft || b.backTop || b.backTopRight
                || b.backMidLeft || b.backMid || b.backMidRight || b.backBottomLeft || b.backBottom || b.backBottomRight)
            {
                if (b.leftTopLeft || b.leftTop || b.leftTopRight || b.leftMidLeft || b.leftMid || b.leftMidRight
                || b.leftBottomLeft || b.leftBottom || b.leftBottomRight || b.rightTopLeft || b.rightTop || b.rightTopRight
                || b.rightMidLeft || b.rightMid || b.rightMidRight || b.rightBottomLeft || b.rightBottom || b.rightBottomRight)
                {
                    cornerBlocksList.Add(levelTiles[i]);
                }
                else
                {
                    levelBlocksList.Add(levelTiles[i]);
                }
            }
            else
            {
                levelBlocksList.Add(levelTiles[i]);
            }
        }

        levelTiles = levelBlocksList.ToArray();
        return cornerBlocksList.ToArray();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tilesArray"></param>
    /// <returns></returns>
    public GameObject[] GetRandomSortedArray(GameObject[] tilesArray)
    {
        System.Random rnd = new System.Random();
        return tilesArray.OrderBy(x => rnd.Next()).ToArray();
    }

    #region Private methods

    /// <summary>
    /// Validates if there is a foward connection between two tiles
    /// </summary>
    /// <param name="currentTile"></param>
    /// <param name="nextTile"></param>
    /// <returns></returns>
    private bool ValidatePathForward(GridLevelBlock currentTile, GridLevelBlock nextTile)
    {
        if ((currentTile.backTopLeft && nextTile.frontTopRight) || (currentTile.backTop && nextTile.frontTop) || (currentTile.backTopRight && nextTile.frontTopLeft) ||
            (currentTile.backMidLeft && nextTile.frontMidRight) || (currentTile.backMid && nextTile.frontMid) || (currentTile.backMidRight && nextTile.frontMidLeft) ||
            (currentTile.backBottomLeft && nextTile.frontBottomRight) || (currentTile.backBottom && nextTile.frontBottom) || (currentTile.backBottomRight && nextTile.frontBottomLeft))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion 
}