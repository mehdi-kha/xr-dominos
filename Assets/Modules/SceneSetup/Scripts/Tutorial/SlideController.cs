using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlideController : MonoBehaviour
{
    [SerializeField] private GameObject _visuals;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _description;
    public void Show(SlideData slideData)
    {
        _title.text = slideData.Title;
        _description.text = slideData.Text;
        _image.material.mainTexture = slideData.Image;
        _visuals.SetActive(true);
    }

    public void Hide()
    {
        _visuals.SetActive(false);
    }
}
