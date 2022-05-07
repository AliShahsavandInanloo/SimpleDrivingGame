using UnityEngine;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
    [SerializeField] private float speed              = 3f;
    [SerializeField] private float turnSpeed          = 200f;
    [SerializeField] private float speedGainPerSecond = 0.2f;

    private int _steerValue;

    private void Update()
    {
        this.speed += this.speedGainPerSecond * Time.deltaTime;

        var turnValue = this._steerValue * (this.turnSpeed * Time.deltaTime);
        this.transform.Rotate(0f, turnValue, 0f);

        this.transform.Translate(Vector3.forward * (this.speed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.ObstacleTag)) SceneManager.LoadScene(TagManager.MainMenu);
    }

    public void Steer(int value)
    {
        this._steerValue = value;
    }
}