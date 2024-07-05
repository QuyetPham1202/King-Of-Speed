using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CarUIHandler : MonoBehaviour
{

    [Header("Car details")]
    public Image carImage;
    Animator animator = null;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetupCar(CarData carData)
    {
        carImage.sprite = carData.CarUISprite;
    }
    public void StartCarEntraceAnimation(bool isAppearingOnRightSide)
    {
        if (isAppearingOnRightSide)
            animator.Play("Car UI Appear From Right ");
        else animator.Play("Car UI Appear From Left");
    }
    public void StartCarExitAnimation(bool isExitingOnRightSide)
    {
        if (isExitingOnRightSide)
            animator.Play("Car UI Disappear To Right ");
        else animator.Play("Car UI Disappear To Left");
    }

    // Event 
    public void OnCarExitAnimationCompleted()
    {
        Destroy(gameObject);
    }
}
