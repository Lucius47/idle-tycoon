using TMPro;
using UnityEngine;

public class ItemsHolder : MonoBehaviour
{
    public Items.ItemType type;
    public int number;

    [SerializeField] private TextMeshProUGUI numberText;
    [SerializeField] private Transform itemsHold;
    [SerializeField] private float gap = 0.2f;

    private void Update()
    {
        numberText.text = number.ToString();
    }

    public void AddItem(Items.ItemType _type, int _number)
    {
        //if (type == Items.ItemType.None)
        //{
        //    this.type = _type;
        //    return;
        //}

        if (type != _type)
        {
            //Wrong type
            return;
        }

        Instantiate(Items.Instance.GetItem(_type), itemsHold.position, itemsHold.rotation, itemsHold);

        GameObject item = Instantiate(Items.Instance.GetItem(_type), itemsHold.position, itemsHold.rotation, itemsHold);
        item.transform.localPosition = new Vector3(0, (itemsHold.childCount - 1) * gap, 0);

        number += _number;
    }

    public bool RemoveItem(Items.ItemType _type, int _number)
    {
        if (_type == type) return false;
        if (number == 0) return false;

        number -= _number;
        Destroy(itemsHold.GetChild(itemsHold.childCount - 1).gameObject);
        return true;
    }
}
