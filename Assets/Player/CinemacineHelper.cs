using UnityEngine;
using Unity.Cinemachine;

public class CinemacineHelper : MonoBehaviour
{
    void Start()
    {
        GameObject Captain = GameObject.Find("Captain");
        CinemachineTargetGroup Group = GetComponent<CinemachineTargetGroup>();

        if (Captain != null && Group != null)
        {
            Group.Targets[0] = new CinemachineTargetGroup.Target
            {
                Object = Captain.transform,
                Radius = 0.5f,
                Weight = 1f
            };
        }
    }
}