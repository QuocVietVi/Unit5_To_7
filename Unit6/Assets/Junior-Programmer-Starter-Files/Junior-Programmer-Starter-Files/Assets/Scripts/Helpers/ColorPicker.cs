using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public Color[] AvailableColors;
    public Button ColorButtonPrefab;
    public TransporterColor[] transColorArr;
    
    public Color SelectedColor { get; private set; }
    public TransporterColor transColor { get; private set; }
    public System.Action<Color> onColorChanged;

    List<Button> m_ColorButtons = new List<Button>();
    
    // Start is called before the first frame update
    public void Init()
    {
        foreach (var color in AvailableColors)
        {
            var newButton = Instantiate(ColorButtonPrefab, transform);
            newButton.GetComponent<Image>().color = color;
            
            newButton.onClick.AddListener(() =>
            {
                SelectedColor = color;
                
                foreach (var button in m_ColorButtons)
                {
                    button.interactable = true;
                }

                newButton.interactable = false;
                
                onColorChanged.Invoke(SelectedColor);
            });
            
            m_ColorButtons.Add(newButton);
        }
        for (int i = 0; i < m_ColorButtons.Count; i++)
        {
            int n = i;
            m_ColorButtons[i].onClick.AddListener(() =>
            {
                MainManager.Instance.transColor = transColorArr[n];
            });
        }
    }

    public void SelectColor(Color color)
    {
        for (int i = 0; i < AvailableColors.Length; ++i)
        {
            if (AvailableColors[i] == color)
            {
                m_ColorButtons[i].onClick.Invoke();
            }
        }
    }
}
