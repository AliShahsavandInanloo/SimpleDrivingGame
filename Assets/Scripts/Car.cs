using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private float speed              = 3f;
    [SerializeField] private float turnSpeed          = 200f;
    [SerializeField] private float speedGainPerSecond = 0.2f;

    private int _steerValue;

    private void Update()
    {
        this.speed += this.speedGainPerSecond * Time.deltaTime;

        this.transform.Rotate(0f, this._steerValue * (this.turnSpeed * Time.deltaTime), 0f);

        this.transform.Translate(Vector3.forward * (this.speed * Time.deltaTime));
    }

    public void Steer(int value)
    {
        this._steerValue = value;
    }
}