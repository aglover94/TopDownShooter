using UnityEngine;

public class MiniMapCam : MonoBehaviour
{
    //Public Variable
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(player.position.x, this.transform.position.y, player.position.z);
    }
}
