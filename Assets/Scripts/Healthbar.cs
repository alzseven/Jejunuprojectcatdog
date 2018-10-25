using UnityEngine;
using System.Collections;

public class Healthbar : MonoBehaviour
{
    GUIStyle healthStyle;
    GUIStyle backStyle;
    UnitLifeCycle UC;

    void Awake()
    {
        UC = GetComponent<UnitLifeCycle>();
    }

    void OnGUI()
    {
        InitStyles();

        // Draw a Health Bar

        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);

        // draw health bar background
        GUI.color = Color.grey;
        GUI.backgroundColor = Color.grey;
        GUI.Box(new Rect(pos.x - 46, Screen.height - pos.y - 200, UC. maxHealth / 1, 4), ".", backStyle);

        // draw health bar amount
        GUI.color = Color.green;
        GUI.backgroundColor = Color.green;
        GUI.Box(new Rect(pos.x - 45, Screen.height - pos.y - 201, UC. Health / 1, 2), ".", healthStyle);
    }

    void InitStyles()
    {
        if (healthStyle == null)
        {
            healthStyle = new GUIStyle(GUI.skin.box);
            healthStyle.normal.background = MakeTex(2, 2, new Color(0f, 1f, 0f, 1.0f));
        }

        if (backStyle == null)
        {
            backStyle = new GUIStyle(GUI.skin.box);
            backStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 1.0f));
        }
    }

    Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}


