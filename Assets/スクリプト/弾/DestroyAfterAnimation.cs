using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    public float destroyTime = 0.5f;


    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}