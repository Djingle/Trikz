using UnityEngine;

public class Bottle : MonoBehaviour
{
    //[field: SerializeField] public float MinTorque { get; private set; }
    //[field: SerializeField] public float MaxTorque { get; private set; }
    //[field: SerializeField] public float DampingFactor { get; private set; }
    //[field: SerializeField] public float ComY { get; private set; }
    [field: SerializeField] public CenterOfMass CoMPrefab { get; private set; }
    public CenterOfMass CenterOfMass { get; private set; }
    public BottleData BottleData { get; private set; }

    public virtual void Init(BottleData bottleData)
    {
        // Init BottleData
        BottleData = bottleData;
        // Init CoM
        Vector3 comPos = transform.position;
        comPos += transform.up * BottleData.ComY;
        // Move CoM if already existing, instantiate it otherwise
        if (CenterOfMass == null) {
            CenterOfMass = Instantiate(CoMPrefab, comPos, Quaternion.identity, gameObject.transform);
        }
        else {
            CenterOfMass.transform.position = comPos;
        }
        // Init Model
        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = BottleData.Model;
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.SetMaterials(BottleData.Materials);
    }
}