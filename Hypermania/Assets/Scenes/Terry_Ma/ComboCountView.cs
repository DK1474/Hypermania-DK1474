using UnityEngine;
using TMPro;
public class ComboCountView: MonoBehaviour
{
    [SerializeField] private TMP_Text ComboText;
    public void SetComboCount(int Combo)
    {
        ComboText.text = Combo.ToString();
    }
    public void SetVisibility(bool Visible)
    {
        gameObject.SetActive(Visible);
    }
}