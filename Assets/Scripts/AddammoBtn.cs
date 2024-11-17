using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AddammoBtn : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Bazooka bazooka;
    [SerializeField] private int ammoAmount = 1;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (bazooka != null)
        {
            bazooka.AddAmmo();
        }
    }
}
