using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewObjectController : MonoBehaviour
{
    [SerializeField] private GetImageListController getImageListController;
    [SerializeField] private List<Texture> AfterpaintList;
    [SerializeField] private List<Texture> BeforepaintList;
    [SerializeField] List<GameObject> AfterobjList;
    [SerializeField] List<GameObject> BeforeobjList;
    [SerializeField] private Material material;
    [SerializeField] private List<GameObject> CanvasObj;
    public bool active = false;
    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            BeforeobjList.Clear();
            AfterobjList.Clear();
            foreach(var obj in CanvasObj)
            {
                foreach(Transform canvas in obj.transform)
                {
                    var beforeCanvas = canvas.transform.GetChild(0).gameObject;
                    var afterCanvas = canvas.transform.GetChild(1).gameObject;

                    BeforeobjList.Add(beforeCanvas);
                    AfterobjList.Add(afterCanvas);
                }
            }
            
            AfterpaintList = getImageListController.AfterpaintList;
            BeforepaintList = getImageListController.BeforepaintList;
            //Debug.Log(BeforepaintList.Count);
            for (int i = 0; AfterobjList.Count > i; i++)
            {
                var AfterpaintMaterial = new Material(material);
                AfterpaintMaterial.mainTexture = AfterpaintList[i];
                AfterobjList[i].GetComponent<MeshRenderer>().material = AfterpaintMaterial;

                var BeforepaintMaterial = new Material(material);
                BeforepaintMaterial.mainTexture = BeforepaintList[i];
                BeforeobjList[i].GetComponent<MeshRenderer>().material = BeforepaintMaterial;

            }
            
        }

    }
}
