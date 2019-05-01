using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridLevelBlock))]
public class GridLevelBlockEditor : Editor
{
    //private variables
    private GridLevelBlock levelBlock;
    private static string[] faceNames = { "Front", "Back", "Left", "Right", "Top", "Bottom" };

    //serialized variables
    private SerializedObject soLevelBlock;
    private SerializedProperty soSelectedLevelType;
    private SerializedProperty soSelectedBlockType;
    #region Front Face
    private SerializedProperty spFrontTopLeft;
    private SerializedProperty spFrontTop;
    private SerializedProperty spFrontTopRight;
    private SerializedProperty spFrontMidLeft;
    private SerializedProperty spFrontMid;
    private SerializedProperty spFrontMidRight;
    private SerializedProperty spFrontBottomLeft;
    private SerializedProperty spFrontBottom;
    private SerializedProperty spFrontBottomRight;
    #endregion
    #region Back Face
    private SerializedProperty spBackTopLeft;
    private SerializedProperty spBackTop;
    private SerializedProperty spBackTopRight;
    private SerializedProperty spBackMidLeft;
    private SerializedProperty spBackMid;
    private SerializedProperty spBackMidRight;
    private SerializedProperty spBackBottomLeft;
    private SerializedProperty spBackBottom;
    private SerializedProperty spBackBottomRight;
    #endregion
    #region Left Face
    private SerializedProperty spLeftTopLeft;
    private SerializedProperty spLeftTop;
    private SerializedProperty spLeftTopRight;
    private SerializedProperty spLeftMidLeft;
    private SerializedProperty spLeftMid;
    private SerializedProperty spLeftMidRight;
    private SerializedProperty spLeftBottomLeft;
    private SerializedProperty spLeftBottom;
    private SerializedProperty spLeftBottomRight;
    #endregion
    #region Right Face
    private SerializedProperty spRightTopLeft;
    private SerializedProperty spRightTop;
    private SerializedProperty spRightTopRight;
    private SerializedProperty spRightMidLeft;
    private SerializedProperty spRightMid;
    private SerializedProperty spRightMidRight;
    private SerializedProperty spRightBottomLeft;
    private SerializedProperty spRightBottom;
    private SerializedProperty spRightBottomRight;
    #endregion
    #region Top Face
    private SerializedProperty spTopTopLeft;
    private SerializedProperty spTopTop;
    private SerializedProperty spTopTopRight;
    private SerializedProperty spTopMidLeft;
    private SerializedProperty spTopMid;
    private SerializedProperty spTopMidRight;
    private SerializedProperty spTopBottomLeft;
    private SerializedProperty spTopBottom;
    private SerializedProperty spTopBottomRight;
    #endregion
    #region Bottom Face
    private SerializedProperty spBottomTopLeft;
    private SerializedProperty spBottomTop;
    private SerializedProperty spBottomTopRight;
    private SerializedProperty spBottomMidLeft;
    private SerializedProperty spBottomMid;
    private SerializedProperty spBottomMidRight;
    private SerializedProperty spBottomBottomLeft;
    private SerializedProperty spBottomBottom;
    private SerializedProperty spBottomBottomRight;
    #endregion   

    //public variables
    public string[] levelTypes = new string[] { "Dungeon", "NW_Dungeon" };
    public string[] blockTypes = new string[] { "Normal", "Spawn Point", "Level End"};

    private void OnEnable()
    {
        levelBlock = target as GridLevelBlock;
        soLevelBlock = new SerializedObject(levelBlock);
        soSelectedLevelType = soLevelBlock.FindProperty("selectedLevelType");
        soSelectedBlockType = soLevelBlock.FindProperty("selectedBlockType");
        #region Front Face
        spFrontTopLeft = soLevelBlock.FindProperty("frontTopLeft");
        spFrontTop = soLevelBlock.FindProperty("frontTop");
        spFrontTopRight = soLevelBlock.FindProperty("frontTopRight");
        spFrontMidLeft = soLevelBlock.FindProperty("frontMidLeft");
        spFrontMid = soLevelBlock.FindProperty("frontMid");
        spFrontMidRight = soLevelBlock.FindProperty("frontMidRight");
        spFrontBottomLeft = soLevelBlock.FindProperty("frontBottomLeft");
        spFrontBottom = soLevelBlock.FindProperty("frontBottom");
        spFrontBottomRight = soLevelBlock.FindProperty("frontBottomRight");
        #endregion
        #region Back Face
        spBackTopLeft = soLevelBlock.FindProperty("backTopLeft");
        spBackTop = soLevelBlock.FindProperty("backTop");
        spBackTopRight = soLevelBlock.FindProperty("backTopRight");
        spBackMidLeft = soLevelBlock.FindProperty("backMidLeft");
        spBackMid = soLevelBlock.FindProperty("backMid");
        spBackMidRight = soLevelBlock.FindProperty("backMidRight");
        spBackBottomLeft = soLevelBlock.FindProperty("backBottomLeft");
        spBackBottom = soLevelBlock.FindProperty("backBottom");
        spBackBottomRight = soLevelBlock.FindProperty("backBottomRight");
        #endregion
        #region Left Face
        spLeftTopLeft = soLevelBlock.FindProperty("leftTopLeft");
        spLeftTop = soLevelBlock.FindProperty("leftTop");
        spLeftTopRight = soLevelBlock.FindProperty("leftTopRight");
        spLeftMidLeft = soLevelBlock.FindProperty("leftMidLeft");
        spLeftMid = soLevelBlock.FindProperty("leftMid");
        spLeftMidRight = soLevelBlock.FindProperty("leftMidRight");
        spLeftBottomLeft = soLevelBlock.FindProperty("leftBottomLeft");
        spLeftBottom = soLevelBlock.FindProperty("leftBottom");
        spLeftBottomRight = soLevelBlock.FindProperty("leftBottomRight");
        #endregion
        #region Right Face
        spRightTopLeft = soLevelBlock.FindProperty("rightTopLeft");
        spRightTop = soLevelBlock.FindProperty("rightTop");
        spRightTopRight = soLevelBlock.FindProperty("rightTopRight");
        spRightMidLeft = soLevelBlock.FindProperty("rightMidLeft");
        spRightMid = soLevelBlock.FindProperty("rightMid");
        spRightMidRight = soLevelBlock.FindProperty("rightMidRight");
        spRightBottomLeft = soLevelBlock.FindProperty("rightBottomLeft");
        spRightBottom = soLevelBlock.FindProperty("rightBottom");
        spRightBottomRight = soLevelBlock.FindProperty("rightBottomRight");
        #endregion
        #region Top Face
        spTopTopLeft = soLevelBlock.FindProperty("topTopLeft");
        spTopTop = soLevelBlock.FindProperty("topTop");
        spTopTopRight = soLevelBlock.FindProperty("topTopRight");
        spTopMidLeft = soLevelBlock.FindProperty("topMidLeft");
        spTopMid = soLevelBlock.FindProperty("topMid");
        spTopMidRight = soLevelBlock.FindProperty("topMidRight");
        spTopBottomLeft = soLevelBlock.FindProperty("topBottomLeft");
        spTopBottom = soLevelBlock.FindProperty("topBottom");
        spTopBottomRight = soLevelBlock.FindProperty("topBottomRight");
        #endregion
        #region Bottom Face
        spBottomTopLeft = soLevelBlock.FindProperty("bottomTopLeft");
        spBottomTop = soLevelBlock.FindProperty("bottomTop");
        spBottomTopRight = soLevelBlock.FindProperty("bottomTopRight");
        spBottomMidLeft = soLevelBlock.FindProperty("bottomMidLeft");
        spBottomMid = soLevelBlock.FindProperty("bottomMid");
        spBottomMidRight = soLevelBlock.FindProperty("bottomMidRight");
        spBottomBottomLeft = soLevelBlock.FindProperty("bottomBottomLeft");
        spBottomBottom = soLevelBlock.FindProperty("bottomBottom");
        spBottomBottomRight = soLevelBlock.FindProperty("bottomBottomRight");
        #endregion
    }

    private void OnDisable()
    {
        levelBlock.ClearSelection();
    }

    public override void OnInspectorGUI()
    {
        soLevelBlock.Update();
        GUILayout.Label("Grid Mapper Options for " + levelBlock.name);
        levelBlock.currentTab = GUILayout.Toolbar(levelBlock.currentTab, faceNames);

        switch (levelBlock.currentTab)
        {
            case 0:
                levelBlock.HighlightSelectedFace(faceNames[0]);
                #region Draw front face toggles
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Top Left:");
                spFrontTopLeft.boolValue = EditorGUILayout.Toggle(spFrontTopLeft.boolValue);
                GUILayout.Label("Top:");
                spFrontTop.boolValue = EditorGUILayout.Toggle(spFrontTop.boolValue);
                GUILayout.Label("Top Right:");
                spFrontTopRight.boolValue = EditorGUILayout.Toggle(spFrontTopRight.boolValue);
                EditorGUILayout.EndToggleGroup();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Mid Left:");
                spFrontMidLeft.boolValue = EditorGUILayout.Toggle(spFrontMidLeft.boolValue);
                GUILayout.Label("Mid:");
                spFrontMid.boolValue = EditorGUILayout.Toggle(spFrontMid.boolValue);
                GUILayout.Label("Mid Right:");
                spFrontMidRight.boolValue = EditorGUILayout.Toggle(spFrontMidRight.boolValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Bottom Left:");
                spFrontBottomLeft.boolValue = EditorGUILayout.Toggle(spFrontBottomLeft.boolValue);
                GUILayout.Label("Bottom:");
                spFrontBottom.boolValue = EditorGUILayout.Toggle(spFrontBottom.boolValue);
                GUILayout.Label("Bottom Right:");
                spFrontBottomRight.boolValue = EditorGUILayout.Toggle(spFrontBottomRight.boolValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                #endregion
                break;
            case 1:
                levelBlock.HighlightSelectedFace(faceNames[1]);
                #region Draw back face toggles
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Top Left:");
                spBackTopLeft.boolValue = EditorGUILayout.Toggle(spBackTopLeft.boolValue);
                GUILayout.Label("Top:");
                spBackTop.boolValue = EditorGUILayout.Toggle(spBackTop.boolValue);
                GUILayout.Label("Top Right:");
                spBackTopRight.boolValue = EditorGUILayout.Toggle(spBackTopRight.boolValue);
                EditorGUILayout.EndToggleGroup();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Mid Left:");
                spBackMidLeft.boolValue = EditorGUILayout.Toggle(spBackMidLeft.boolValue);
                GUILayout.Label("Mid:");
                spBackMid.boolValue = EditorGUILayout.Toggle(spBackMid.boolValue);
                GUILayout.Label("Mid Right:");
                spBackMidRight.boolValue = EditorGUILayout.Toggle(spBackMidRight.boolValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Bottom Left:");
                spBackBottomLeft.boolValue = EditorGUILayout.Toggle(spBackBottomLeft.boolValue);
                GUILayout.Label("Bottom:");
                spBackBottom.boolValue = EditorGUILayout.Toggle(spBackBottom.boolValue);
                GUILayout.Label("Bottom Right:");
                spBackBottomRight.boolValue = EditorGUILayout.Toggle(spBackBottomRight.boolValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                #endregion
                break;
            case 2:
                levelBlock.HighlightSelectedFace(faceNames[2]);
                #region Draw left face toggles
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Top Left:");
                spLeftTopLeft.boolValue = EditorGUILayout.Toggle(spLeftTopLeft.boolValue);
                GUILayout.Label("Top:");
                spLeftTop.boolValue = EditorGUILayout.Toggle(spLeftTop.boolValue);
                GUILayout.Label("Top Right:");
                spLeftTopRight.boolValue = EditorGUILayout.Toggle(spLeftTopRight.boolValue);
                EditorGUILayout.EndToggleGroup();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Mid Left:");
                spLeftMidLeft.boolValue = EditorGUILayout.Toggle(spLeftMidLeft.boolValue);
                GUILayout.Label("Mid:");
                spLeftMid.boolValue = EditorGUILayout.Toggle(spLeftMid.boolValue);
                GUILayout.Label("Mid Right:");
                spLeftMidRight.boolValue = EditorGUILayout.Toggle(spLeftMidRight.boolValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Bottom Left:");
                spLeftBottomLeft.boolValue = EditorGUILayout.Toggle(spLeftBottomLeft.boolValue);
                GUILayout.Label("Bottom:");
                spLeftBottom.boolValue = EditorGUILayout.Toggle(spLeftBottom.boolValue);
                GUILayout.Label("Bottom Right:");
                spLeftBottomRight.boolValue = EditorGUILayout.Toggle(spLeftBottomRight.boolValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                #endregion
                break;
            case 3:
                levelBlock.HighlightSelectedFace(faceNames[3]);
                #region Draw right face toggles
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Top Left:");
                spRightTopLeft.boolValue = EditorGUILayout.Toggle(spRightTopLeft.boolValue);
                GUILayout.Label("Top:");
                spRightTop.boolValue = EditorGUILayout.Toggle(spRightTop.boolValue);
                GUILayout.Label("Top Right:");
                spRightTopRight.boolValue = EditorGUILayout.Toggle(spRightTopRight.boolValue);
                EditorGUILayout.EndToggleGroup();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Mid Left:");
                spRightMidLeft.boolValue = EditorGUILayout.Toggle(spRightMidLeft.boolValue);
                GUILayout.Label("Mid:");
                spRightMid.boolValue = EditorGUILayout.Toggle(spRightMid.boolValue);
                GUILayout.Label("Mid Right:");
                spRightMidRight.boolValue = EditorGUILayout.Toggle(spRightMidRight.boolValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Bottom Left:");
                spRightBottomLeft.boolValue = EditorGUILayout.Toggle(spRightBottomLeft.boolValue);
                GUILayout.Label("Bottom:");
                spRightBottom.boolValue = EditorGUILayout.Toggle(spRightBottom.boolValue);
                GUILayout.Label("Bottom Right:");
                spRightBottomRight.boolValue = EditorGUILayout.Toggle(spRightBottomRight.boolValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                #endregion
                break;
            case 4:
                levelBlock.HighlightSelectedFace(faceNames[4]);
                #region Draw top face toggles
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Top Left:");
                spTopTopLeft.boolValue = EditorGUILayout.Toggle(spTopTopLeft.boolValue);
                GUILayout.Label("Top:");
                spTopTop.boolValue = EditorGUILayout.Toggle(spTopTop.boolValue);
                GUILayout.Label("Top Right:");
                spTopTopRight.boolValue = EditorGUILayout.Toggle(spTopTopRight.boolValue);
                EditorGUILayout.EndToggleGroup();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Mid Left:");
                spTopMidLeft.boolValue = EditorGUILayout.Toggle(spTopMidLeft.boolValue);
                GUILayout.Label("Mid:");
                spTopMid.boolValue = EditorGUILayout.Toggle(spTopMid.boolValue);
                GUILayout.Label("Mid Right:");
                spTopMidRight.boolValue = EditorGUILayout.Toggle(spTopMidRight.boolValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Bottom Left:");
                spTopBottomLeft.boolValue = EditorGUILayout.Toggle(spTopBottomLeft.boolValue);
                GUILayout.Label("Bottom:");
                spTopBottom.boolValue = EditorGUILayout.Toggle(spTopBottom.boolValue);
                GUILayout.Label("Bottom Right:");
                spTopBottomRight.boolValue = EditorGUILayout.Toggle(spTopBottomRight.boolValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                #endregion
                break;
            case 5:
                levelBlock.HighlightSelectedFace(faceNames[5]);
                #region Draw bottom face toggles
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Top Left:");
                spBottomTopLeft.boolValue = EditorGUILayout.Toggle(spBottomTopLeft.boolValue);
                GUILayout.Label("Top:");
                spBottomTop.boolValue = EditorGUILayout.Toggle(spBottomTop.boolValue);
                GUILayout.Label("Top Right:");
                spBottomTopRight.boolValue = EditorGUILayout.Toggle(spBottomTopRight.boolValue);
                EditorGUILayout.EndToggleGroup();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Mid Left:");
                spBottomMidLeft.boolValue = EditorGUILayout.Toggle(spBottomMidLeft.boolValue);
                GUILayout.Label("Mid:");
                spBottomMid.boolValue = EditorGUILayout.Toggle(spBottomMid.boolValue);
                GUILayout.Label("Mid Right:");
                spBottomMidRight.boolValue = EditorGUILayout.Toggle(spBottomMidRight.boolValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Bottom Left:");
                spBottomBottomLeft.boolValue = EditorGUILayout.Toggle(spBottomBottomLeft.boolValue);
                GUILayout.Label("Bottom:");
                spBottomBottom.boolValue = EditorGUILayout.Toggle(spBottomBottom.boolValue);
                GUILayout.Label("Bottom Right:");
                spBottomBottomRight.boolValue = EditorGUILayout.Toggle(spBottomBottomRight.boolValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                #endregion
                break;
        }

        //Level Type Dropdown
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Level Type: ");
        soSelectedLevelType.intValue = EditorGUILayout.Popup(soSelectedLevelType.intValue, levelTypes);
        EditorGUILayout.EndHorizontal();

        //Block Type Dropdown
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Block Type: ");
        soSelectedBlockType.intValue = EditorGUILayout.Popup(soSelectedBlockType.intValue, blockTypes);
        EditorGUILayout.EndHorizontal();

        soLevelBlock.ApplyModifiedProperties();
    }
}
