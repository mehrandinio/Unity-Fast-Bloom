using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTextEffect : MonoBehaviour
{
    struct TextData
    {
        public float offset;
        public float hue;
        public float value;
        public Vector2 position;
        public float rotation;
    }

    [SerializeField] Text textPrefab = null;
    [SerializeField] Transform grid = null;

    List<Text> texts = new List<Text>();
    List<TextData> textsData = new List<TextData>();

    float timer;

    public void Awake ()
    {
        DestroyChildrens(grid);
        int length = 5;
        for (int i = 0; i < length; i++)
        {
            var text = Instantiate(textPrefab, grid, false);
            var scale = (i + 1) * 0.5f;
            text.transform.localScale = Vector2.one * scale;
            texts.Add(text);

            var data = new TextData();
            data.offset = (float)i / (float)length;
            textsData.Add(data);
        }
    }

    void Update ()
    {
        var dt = Time.deltaTime;
        UpdateData(dt);
        UpdateView();
    }

    void UpdateData (float dt)
    {
        float loopDuration = 3;
        float time = Time.realtimeSinceStartup % loopDuration;
        float t = time / loopDuration;
        float TAU = Mathf.PI * 2;

        for (int i = 0; i < textsData.Count; i++)
        {
            var data = textsData[i];

            float id = i;
            data.hue = t;
            data.value = (t + data.offset) % 0.5f < 0.3f ? 1 : 0;
            data.rotation = t * TAU / 1 + 0.25f * Mathf.Sin(t * TAU) * id;

            textsData[i] = data;
        }
    }

    void UpdateView ()
    {
        for (int i = 0; i < texts.Count; i++)
        {
            var text = texts[i];
            var data = textsData[i];
            var hue = Mathf.Repeat(data.hue + data.offset, 1);
            text.color = Color.HSVToRGB(hue, 1, data.value);
            text.transform.localRotation = Quaternion.Euler(0, 0, data.rotation * Mathf.Rad2Deg);
        }
    }

    void DestroyChildrens (Transform transform)
    {
        int count = transform.childCount;
        for (int i = 0; i < count; i++)
        {
            var child = transform.GetChild(i).gameObject;
            Object.Destroy(child);
        }
    }
}
