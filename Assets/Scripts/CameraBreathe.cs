using UnityEngine;

public class CameraBreathe : MonoBehaviour
{
    public float amplitude = 10f;
    public float period    = 5f;

    private Vector3 startPos;

    protected void Start()
    {
        this.startPos = this.transform.position;
    }

    protected void Update()
    {
        var theta    = Time.timeSinceLevelLoad / this.period;
        var distance = this.amplitude          * Mathf.Sin(theta);
        this.transform.position = this.startPos + Vector3.up * distance;
    }
}