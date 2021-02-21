using UnityEngine;
public class WeaponBobbing : MonoBehaviour
{
    OptSystem optSystem = new OptSystem();
    private float timer = 0.0f;
    public float bobbingSpeed = 0.2f;
    public float bobbingAmount = 3f;
    float midpoint = 0.0f;
    void Update()
    {
        

        if (CharacterSystem.isCarrying && !CharacterSystem.isJumping)
        {
            float waveslice = 0.0f;
            float horizontal = optSystem.Input.GetAxis("Horizontal");
            float vertical = optSystem.Input.GetAxis("Vertical");

            Vector3 cSharpConversion = transform.localPosition;
            if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
            {
                timer = 0.0f;
            }
            else
            {
                waveslice = Mathf.Sin(timer);
                timer = timer + bobbingSpeed;
                if (timer > Mathf.PI * 2)
                {
                    timer = timer - (Mathf.PI * 2);
                }
            }
            if (waveslice != 0)
            {
                float translateChange = waveslice * bobbingAmount;
                float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
                totalAxes = Mathf.Clamp(totalAxes, 0.1f, 0.1f);
                translateChange = totalAxes * translateChange;
                cSharpConversion.y = midpoint + translateChange;
            }
            else
            {
                cSharpConversion.y = midpoint;
            }
            transform.localPosition = cSharpConversion;
        } 
    }
}