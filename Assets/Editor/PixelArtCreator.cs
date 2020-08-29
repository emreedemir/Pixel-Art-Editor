using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.Linq;


public class PixelArtCreator : EditorWindow
{
    public Sprite defaultSprite;

    public Texture defaultTexture;

    static EditorWindow editorWindow;

    int selectedPanelOption = 0;

    string[] paletteOptions = { "8x8", "16x16", "32x32", "64x64" };

    public List<Vector2> filledPositions;

    public int Dimension = 0;

    public Dictionary<Rect, Texture2D> texturesDataPair = new Dictionary<Rect, Texture2D>();

    public List<Texture2D> textures = new List<Texture2D>();

    public List<Rect> rects = new List<Rect>();

    public Color currentColor;

    public bool colorPicker;


    public readonly KeyValuePair<int, int>[] paletElements =
    {
        new KeyValuePair<int, int>(0, 8),
        new KeyValuePair<int, int>(1, 16) ,
        new KeyValuePair<int, int>(2, 32),
        new KeyValuePair<int,int>(4,64)
    };

    public float tileSize = 12;

    Vector2 mousePosition;

    Rect paletteRect = new Rect(10, 10, 500, 500);

    private void OnEnable()
    {
        ReCreatePalette();
    }

    [MenuItem("Pixel Art Creator/Open Editor")]
    public static void OpenWindow()
    {
        editorWindow = EditorWindow.GetWindow(typeof(PixelArtCreator));

        editorWindow.minSize = new Vector2(800, 600);

        editorWindow.maxSize = new Vector2(800, 600);
    }



    private void OnGUI()
    {
        DrawSection();

        DrawMenu();
    }

    public void DrawSection()
    {
        GUILayout.BeginArea(paletteRect);

        int order = 0;

        if (textures.Count > 0)
        {
            for (int i = 0; i < Dimension; i++)
            {
                for (int j = 0; j < Dimension; j++)
                {

                    EditorGUI.DrawPreviewTexture(rects[order], textures[order]);

                    order++;
                }
            }
        }

        GUILayout.EndArea();
    }

    private void OnInspectorUpdate()
    {
        mousePosition = Event.current.mousePosition;

        EditorGUILayout.LabelField("Mouse Position: ", Event.current.mousePosition.ToString());
    }

    public void DrawMenu()
    {

        EditorGUI.BeginChangeCheck();


        selectedPanelOption = EditorGUILayout.Popup("Select Palet", selectedPanelOption, paletteOptions);

        currentColor = EditorGUILayout.ColorField(currentColor);

        if (GUILayout.Button("Color Picker"))
        {
            colorPicker = !colorPicker;
        }

        Event mouseEvent = Event.current;

        if (mouseEvent.isMouse)
        {
            if (colorPicker)
            {
                mousePosition = Event.current.mousePosition;

                Debug.Log("mouse Position" + mousePosition);

                Debug.Log("1");

                Rect rect = rects.Find(x => x.Contains(mousePosition));

                if (rect != null)
                {
                    Debug.Log(rect.position);

                    Debug.Log("3");

                    Texture2D texture;

                    if (texturesDataPair.TryGetValue(rect, out texture))
                    {
                        Debug.Log("4");
                        currentColor = texture.GetPixel(1, 1);

                        colorPicker = false;
                    }
                }
            }
        }


        if (EditorGUI.EndChangeCheck())
        {
            ReCreatePalette();
        }
    }

    public void ReCreatePalette()
    {
        int dimension = paletElements[selectedPanelOption].Value;

        texturesDataPair.Clear();

        for (int j = 0; j < dimension; j++)
        {
            for (int i = 0; i < dimension; i++)
            {
                Texture2D texture = GetDefaultTexture();

                texturesDataPair.Add(new Rect((Dimension - j) * (tileSize + 1), i * (tileSize + 1), tileSize, tileSize), texture);
            }
        }

        textures = texturesDataPair.Values.ToList();

        rects = texturesDataPair.Keys.ToList();

        Dimension = dimension;
    }

    public Texture2D GetDefaultTexture()
    {
        Texture2D texture2D = new Texture2D(2, 2, TextureFormat.ARGB32, false);

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                texture2D.SetPixel(2, 2, Color.gray);
            }
        }

        texture2D.Apply();

        return texture2D;
    }
}

