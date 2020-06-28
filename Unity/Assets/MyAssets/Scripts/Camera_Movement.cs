using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    private readonly float xEdge = 10000;
    private readonly float yEdge = 10000;
    public int VerticalCameraOffsetFromPlayer;

    private Vector3 cameraStart;

    public Transform player;

    //public Transform background;
    //WorldManagement WM;
    // Use this for initialization
    private void Start()
    {
        //WM = GameObject.Find("WorldManager").GetComponent<WorldManagement>();
        cameraStart = transform.position;
        var playerPosition = player.position + new Vector3(0, VerticalCameraOffsetFromPlayer, 0);
        transform.position = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z + cameraStart.z);
        //background.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z + 1.0f);
        //background.parent = this.transform;
        //CalculateEdges();
    }

    // Update is called once per frame
    private void Update()
    {
        //fix this, so that camera won't look past scene edges, after worldmanager scene loading and features are fixed
        //z changing is removed to make perspective flooring
        var playerPosition = player.position + new Vector3(0, VerticalCameraOffsetFromPlayer, 0);
        var new_position = new Vector3();
        if (playerPosition.x <= xEdge && playerPosition.x >= -xEdge)
            new_position = new Vector3(playerPosition.x, new_position.y, new_position.z);
        else
            new_position = new Vector3(transform.position.x, new_position.y, new_position.z);
        if (playerPosition.y <= yEdge && playerPosition.y >= -yEdge)
            new_position = new Vector3(new_position.x, playerPosition.y, new_position.z);
        else
            new_position = new Vector3(new_position.x, transform.position.y, new_position.z);
        transform.position = new Vector3(new_position.x, new_position.y, playerPosition.z + cameraStart.z);
        //background.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z + 1.0f);
    }

    public void UpdateCoords()
    {
        //CalculateEdges();
        var playerPosition = player.position + new Vector3(0, VerticalCameraOffsetFromPlayer, 0);
        var new_position = new Vector3();
        if (playerPosition.x <= xEdge && playerPosition.x >= -xEdge)
            new_position = new Vector3(playerPosition.x, new_position.y, new_position.z);
        else
            new_position = new Vector3(Mathf.Sign(playerPosition.x) * xEdge, new_position.y, new_position.z);
        if (playerPosition.y <= yEdge && playerPosition.y >= -yEdge)
            new_position = new Vector3(new_position.x, playerPosition.y, new_position.z);
        else
            new_position = new Vector3(new_position.x, Mathf.Sign(playerPosition.y) * yEdge, new_position.z);
        transform.position = new Vector3(new_position.x, new_position.y, playerPosition.z + cameraStart.z);
        //background.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z + 1.0f);
    }

    /*void CalculateEdges()
    {
        xEdge = this.GetComponent<Camera>().pixelWidth;
        Vector3 ScreenToWorldPointSize = Camera.main.ScreenToViewportPoint(new Vector3((float)this.GetComponent<Camera>().pixelWidth, (float)this.GetComponent<Camera>().pixelHeight, 0.0f));
        //also use this, if there will ever be scaling
        try
        {
            //calculating edge position by x
            xEdge = WM.Scenes[WM.current_Scene].Size.x;
            if((WM.Scenes[WM.current_Scene].Size.x/2) < ScreenToWorldPointSize.x/2)
                xEdge = 0;
            else
                xEdge = (WM.Scenes[WM.current_Scene].Size.x - ScreenToWorldPointSize.x)/2;
            //calculating edge position by y
            if((WM.Scenes[WM.current_Scene].Size.y/2) < ScreenToWorldPointSize.y/2)
                yEdge = 0;
            else
            {
                yEdge = WM.Scenes[WM.current_Scene].Size.y;
                yEdge = (WM.Scenes[WM.current_Scene].Size.y - ScreenToWorldPointSize.y)/2;
            }
        }
        catch{}
    }

    public void UpdateScale()
    {
        //float a = this.gameObject.GetComponent<Camera>().aspect;
        //float o = this.gameObject.GetComponent<Camera>().orthographicSize;
        //o=o;
        this.gameObject.GetComponent<Camera>().aspect = (1.0f+1.0f/3.0f)*WM.Scenes[WM.current_Scene].Scale.x;
        this.gameObject.GetComponent<Camera>().orthographicSize = 5*WM.Scenes[WM.current_Scene].Scale.y;
    }*/
}
