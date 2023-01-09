using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewObjectController : MonoBehaviour
{
    [SerializeField] private GetImageListController getImageListController;
    [SerializeField] private List<Texture> AfterpaintList;
    [SerializeField] List<GameObject> AfterobjList;
    [SerializeField] private Material material;
    public bool active = false;
    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            AfterpaintList = getImageListController.AfterpaintList;

            for (int i = 0; AfterobjList.Count > i; i++)
            {
                var paintMaterial = new Material(material);
                paintMaterial.mainTexture = AfterpaintList[i];
                AfterobjList[i].GetComponent<MeshRenderer>().material = paintMaterial;
            }
        }

    }
}
