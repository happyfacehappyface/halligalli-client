using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SplashSceneController : MonoBehaviour
{
    protected void Start()
    {
        StartCoroutine(WaitForReady());

    }

    private bool IsAllReady()
    {

        if ((SoundManager.Instance == null) || (!SoundManager.Instance.IsReady()))
        {
            return false;
        }

        if ((NetworkManager.Instance == null) || (!NetworkManager.Instance.IsReady()))
        {
            return false;
        }


        if ((AssetHolder.Instance == null) || (!AssetHolder.Instance.IsReady()))
        {
            return false;
        }

        return true;

    }


    IEnumerator WaitForReady()
    {
        while (true)
        {
            if (IsAllReady())
            {
                SceneManager.LoadScene("OutGameScene");
            }

            yield return new WaitForSeconds(0.1f);
        }

    }
}
