using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public void OnClickPlay()
    {
        //ScenerManager.LoadScene("GameLevel");
        LoadingScreenManager.Instance.SwitchToScene(2);  
    }
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
