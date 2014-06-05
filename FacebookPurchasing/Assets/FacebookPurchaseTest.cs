using UnityEngine;
using System.Collections;

public class FacebookPurchaseTest : MonoBehaviour {

    public GUIText debugText;

    public string productID = "";
    public int quantity = 1;

    private string lastResponse = "";
    private bool isInit = false;
    bool loggedIn = false;

	// Use this for initialization
	void Start () {
        FB.Init(OnInitComplete, OnHideUnity);
	}

    void Update()
    {
        debugText.text = lastResponse;
    }

    private void OnInitComplete()
    {
        lastResponse = "FB.Init completed: Is user logged in? " + FB.IsLoggedIn;
        isInit = true;

        FB.Login("email,publish_actions", LoginCallback);
    }

    void LoginCallback(FBResult result)
    {
        if (result.Error != null)
            lastResponse = "Error Response:\n" + result.Error;
        else if (!FB.IsLoggedIn)
        {
            lastResponse = "Login cancelled by Player";
        }
        else
        {
            lastResponse = "Login was successful!";
            loggedIn = true;
        }
    }


    private void OnHideUnity(bool isGameShown)
    {
        Debug.Log("Is game showing? " + isGameShown);
    }

    void OnGUI()
    {
        if (loggedIn == true)
        {
            if (GUI.Button(new Rect((Screen.width / 2) - 50, (Screen.height / 2) - 100, 100, 30), "Purchase"))
            {
                FB.Canvas.Pay(
                  product: "https://socialplay-staging.azurewebsites.net/CreditBundleDataFacebook?BundleID=5",
                  quantity: 1,
                  callback: delegate(FBResult response)
                  {
                      if (response.Error == null)
                          lastResponse = response.Text;
                      else
                          lastResponse = response.Error;

                      Application.ExternalEval("console.log('" + lastResponse + "');");
                  }
                        );
            }
        }
    }
}
