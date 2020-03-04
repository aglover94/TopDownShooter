using UnityEngine;
using UnityEngine.UI;

public class MakeMapObject : MonoBehaviour
{
    //Public Variable
    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        MiniMapController.RegisterMapObject(this.gameObject, image);
    }

    void OnDestroy()
    {
        MiniMapController.RemoveMapObject(this.gameObject);
    }
}
