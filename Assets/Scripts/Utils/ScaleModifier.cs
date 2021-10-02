using UnityEngine;

public class ScaleModifier : MonoBehaviour
{
    const float LERP_THRESHOLD = 0.01f;
    bool isChanging;
    [SerializeField]
    float speed;
    Vector3 desiredScale;
    Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isChanging)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, speed * Time.deltaTime);
            if(Vector3.Distance(transform.localScale,desiredScale) <= LERP_THRESHOLD)
            {
                transform.localScale = desiredScale;
                isChanging = false;
            }
        }        
    }

    public void Modify(Vector3 desiredScale)
    {
        isChanging = true;
        this.desiredScale = desiredScale;
    }
    public void Modify(float desiredScale)
    {
        isChanging = true;
        this.desiredScale = Vector3.one * desiredScale;
    }
    public void ModifyToOriginal()
    {
        isChanging = true;
        this.desiredScale = originalScale;
    }
}
