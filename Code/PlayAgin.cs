using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayAgin : MonoBehaviour
{
    // Start is called before the first frame update
   public void Againlever2()
    {
        SceneManager.LoadScene("TopDownCarlever2");
    }
    public void Againlever1()
    {
        SceneManager.LoadScene("TopDownCarlever1");
    }
    public void Againlever3()
    {
        SceneManager.LoadScene("TopDownCarlever3");
    }
    public void AgainPlayer()
    {
        SceneManager.LoadScene("TopDownCarPlayer");
    }
   public void ExitGame()
    {   

        SceneManager.LoadScene("Menu");
    }
}
