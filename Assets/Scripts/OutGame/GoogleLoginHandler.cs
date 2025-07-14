using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class GoogleLoginHandler : MonoBehaviour
{
    // ─── 싱글톤 ───────────────────────────────────────────────
    public  static GoogleLoginHandler Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ─── 설정값 ───────────────────────────────────────────────
    //private const string LoginUrl  = "http://localhost:8081/google/auth/login";
    //private const string StatusUrl = "http://localhost:8081/auth/status";

    private const string LoginUrl = "/api/google/auth/login";
    private const string StatusUrl = "/api/auth/status";
    private const float  PollDelay = 1f;

    // ─── 서버 응답 모델 ──────────────────────────────────────
    [System.Serializable]
    private class StatusResponse
    {
        public bool loggedIn;
        public User user;

        [System.Serializable]
        public class User
        {
            public string id;
            public string email;
            public string name;
        }
    }

    // ─── 로그인 상태 보관 ────────────────────────────────────
    private StatusResponse.User currentUser;
    public  bool   IsLoggedIn => currentUser != null;
    public  string Id         => currentUser?.id;
    public  string Email      => currentUser?.email;
    public  string Name       => currentUser?.name;

    // ─── UI 버튼에서 호출 ────────────────────────────────────
    public void TryLogin()
    {
    #if UNITY_WEBGL && !UNITY_EDITOR
        Application.ExternalEval($"window.open('{LoginUrl}', '_blank')");
    #else
        Application.OpenURL(LoginUrl);
    #endif
        StartCoroutine(PollLoginStatus());
    }

    // ─── 로그인 상태 폴링 ────────────────────────────────────
    private IEnumerator PollLoginStatus()
    {
        while (true)
        {
            using (UnityWebRequest req = UnityWebRequest.Get(StatusUrl))
            {
                yield return req.SendWebRequest();

                if (req.result == UnityWebRequest.Result.Success)
                {
                    var data = JsonUtility.FromJson<StatusResponse>(req.downloadHandler.text);
                    if (data != null && data.loggedIn)
                    {
                        OnLoginSuccess(data.user);
                        yield break;          // 종료
                    }
                }
            }
            yield return new WaitForSeconds(PollDelay);
        }
    }

    // ─── 로그인 완료 처리 ───────────────────────────────────
    private void OnLoginSuccess(StatusResponse.User user)
    {
        currentUser = user;
        Debug.Log($"✅ 로그인 성공: {user.email}, {user.name}");

        // TODO: UI 갱신·씬 전환 등
    }
}
