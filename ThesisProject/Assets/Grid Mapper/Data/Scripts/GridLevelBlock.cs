using UnityEngine;

public class GridLevelBlock : MonoBehaviour
{
    //private variables
    private Renderer[] renderers;

    //public variables
    public int currentTab;
    public int selectedLevelType = 0;
    public int selectedBlockType = 0;
    #region Front Face
    public bool frontTopLeft;
    public bool frontTop;
    public bool frontTopRight;
    public bool frontMidLeft;
    public bool frontMid;
    public bool frontMidRight;
    public bool frontBottomLeft;
    public bool frontBottom;
    public bool frontBottomRight;
    #endregion
    #region Back Face
    public bool backTopLeft;
    public bool backTop;
    public bool backTopRight;
    public bool backMidLeft;
    public bool backMid;
    public bool backMidRight;
    public bool backBottomLeft;
    public bool backBottom;
    public bool backBottomRight;
    #endregion
    #region Left Face
    public bool leftTopLeft;
    public bool leftTop;
    public bool leftTopRight;
    public bool leftMidLeft;
    public bool leftMid;
    public bool leftMidRight;
    public bool leftBottomLeft;
    public bool leftBottom;
    public bool leftBottomRight;
    #endregion
    #region Right Face
    public bool rightTopLeft;
    public bool rightTop;
    public bool rightTopRight;
    public bool rightMidLeft;
    public bool rightMid;
    public bool rightMidRight;
    public bool rightBottomLeft;
    public bool rightBottom;
    public bool rightBottomRight;
    #endregion
    #region Top Face
    public bool topTopLeft;
    public bool topTop;
    public bool topTopRight;
    public bool topMidLeft;
    public bool topMid;
    public bool topMidRight;
    public bool topBottomLeft;
    public bool topBottom;
    public bool topBottomRight;
    #endregion
    #region Bottom Face
    public bool bottomTopLeft;
    public bool bottomTop;
    public bool bottomTopRight;
    public bool bottomMidLeft;
    public bool bottomMid;
    public bool bottomMidRight;
    public bool bottomBottomLeft;
    public bool bottomBottom;
    public bool bottomBottomRight;
    #endregion Front Face

    public Material unselected;
    public Material selected;

    /// <summary>
    /// Highlights the selected face
    /// </summary>
    /// <param name="faceName"></param>
    public void HighlightSelectedFace(string faceName)
    {
        UpdateMaterials();
        renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.sharedMaterial = unselected;

            if (faceName.Equals("Front") && (renderer.name.Equals("Outside_Front") || renderer.name.Equals("Inside_Front")))
            {
                renderer.sharedMaterial = selected;
            }
            else if (faceName.Equals("Back") && (renderer.name.Equals("Outside_Back") || renderer.name.Equals("Inside_Back")))
            {
                renderer.sharedMaterial = selected;
            }
            else if (faceName.Equals("Left") && (renderer.name.Equals("Outside_Left") || renderer.name.Equals("Inside_Left")))
            {
                renderer.sharedMaterial = selected;
            }
            else if (faceName.Equals("Right") && (renderer.name.Equals("Outside_Right") || renderer.name.Equals("Inside_Right")))
            {
                renderer.sharedMaterial = selected;
            }
            else if (faceName.Equals("Top") && (renderer.name.Equals("Outside_Up") || renderer.name.Equals("Inside_Up")))
            {
                renderer.sharedMaterial = selected;
            }
            else if (faceName.Equals("Bottom") && (renderer.name.Equals("Outside_Bottom") || renderer.name.Equals("Inside_Bottom")))
            {
                renderer.sharedMaterial = selected;
            }
        }
    }

    /// <summary>
    /// Update the material when the selection changes
    /// </summary>
    private void UpdateMaterials()
    {
        if (unselected == null)
        {
            unselected = (Material)Resources.Load("grid_cube", typeof(Material));
        }

        if (selected == null)
        {
            selected = (Material)Resources.Load("grid_cube_selected", typeof(Material));
        }

    }

    /// <summary>
    /// Clear all the information on the level block
    /// </summary>
    public void Clear()
    {
        selectedLevelType = 0;
        selectedBlockType = 0;
        #region Front Face
        frontTopLeft = false;
        frontTop = false;
        frontTopRight = false;
        frontMidLeft = false;
        frontMid = false;
        frontMidRight = false;
        frontBottomLeft = false;
        frontBottom = false;
        frontBottomRight = false;
        #endregion
        #region Back Face
        backTopLeft = false;
        backTop = false;
        backTopRight = false;
        backMidLeft = false;
        backMid = false;
        backMidRight = false;
        backBottomLeft = false;
        backBottom = false;
        backBottomRight = false;
        #endregion
        #region Left Face
        leftTopLeft = false;
        leftTop = false;
        leftTopRight = false;
        leftMidLeft = false;
        leftMid = false;
        leftMidRight = false;
        leftBottomLeft = false;
        leftBottom = false;
        leftBottomRight = false;
        #endregion
        #region Right Face
        rightTopLeft = false;
        rightTop = false;
        rightTopRight = false;
        rightMidLeft = false;
        rightMid = false;
        rightMidRight = false;
        rightBottomLeft = false;
        rightBottom = false;
        rightBottomRight = false;
        #endregion
        #region Top Face
        topTopLeft = false;
        topTop = false;
        topTopRight = false;
        topMidLeft = false;
        topMid = false;
        topMidRight = false;
        topBottomLeft = false;
        topBottom = false;
        topBottomRight = false;
        #endregion
        #region Bottom Face
        bottomTopLeft = false;
        bottomTop = false;
        bottomTopRight = false;
        bottomMidLeft = false;
        bottomMid = false;
        bottomMidRight = false;
        bottomBottomLeft = false;
        bottomBottom = false;
        bottomBottomRight = false;
        #endregion Front Face
    }

    /// <summary>
    /// Clears all selections and highlights
    /// </summary>
    public void ClearSelection()
    {
        renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.sharedMaterial = unselected;
        }
    }
}