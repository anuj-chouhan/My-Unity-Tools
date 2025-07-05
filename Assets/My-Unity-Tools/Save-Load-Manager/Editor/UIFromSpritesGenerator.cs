using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIFromSpritesGenerator : EditorWindow
{
    GameObject parentObject;
    List<Sprite> spriteList = new List<Sprite>();
    Vector2 scrollPos;

    [MenuItem("Tools/UI Sprite Importer")]
    public static void ShowWindow()
    {
        GetWindow<UIFromSpritesGenerator>("UI Sprite Importer");
    }

    private void OnGUI()
    {
        GUILayout.Label("UI Generator from Sprites", EditorStyles.boldLabel);

        parentObject = (GameObject)EditorGUILayout.ObjectField("Parent GameObject", parentObject, typeof(GameObject), true);

        GUILayout.Space(10);
        GUILayout.Label("Drag & Drop Sprites Below", EditorStyles.boldLabel);
        Rect dropArea = GUILayoutUtility.GetRect(0.0f, 100.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drop Sprites Here");

        HandleDragAndDrop(dropArea);

        GUILayout.Space(10);
        GUILayout.Label("Sprites in Queue", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(150));
        foreach (var sprite in spriteList)
        {
            EditorGUILayout.ObjectField(sprite, typeof(Sprite), false);
        }
        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);

        if (GUILayout.Button("Generate UI Elements"))
        {
            GenerateUIElements();
        }

        if (GUILayout.Button("Reset Tool"))
        {
            ResetTool();
        }
    }

    private void HandleDragAndDrop(Rect dropArea)
    {
        Event evt = Event.current;
        if (evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform)
        {
            if (!dropArea.Contains(evt.mousePosition))
                return;

            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (evt.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();

                foreach (Object dragged in DragAndDrop.objectReferences)
                {
                    if (dragged is Sprite sprite && !spriteList.Contains(sprite))
                    {
                        spriteList.Add(sprite);
                    }
                }

                evt.Use();
            }
        }
    }

    private void GenerateUIElements()
    {
        if (parentObject == null)
        {
            Debug.LogError("Parent GameObject is not assigned.");
            return;
        }

        foreach (Sprite sprite in spriteList)
        {
            if (sprite == null) continue;

            GameObject go = new GameObject(sprite.name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            go.transform.SetParent(parentObject.transform, false);

            Image img = go.GetComponent<Image>();
            img.sprite = sprite;
        }

        Debug.Log($"Created {spriteList.Count} UI elements.");
    }

    private void ResetTool()
    {
        parentObject = null;
        spriteList.Clear();
        Debug.Log("Tool has been reset.");
    }
}