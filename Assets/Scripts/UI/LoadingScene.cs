using Cysharp.Threading.Tasks;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class LoadingScene : MonoBehaviour
    {
        private async void Start()
        {
            //FB.Init(() => SetFBAdvertiserTracking(true));
            await SceneManager.LoadSceneAsync(1);
        }

        private void SetFBAdvertiserTracking(bool value)
        {
            FB.Mobile.SetAdvertiserTrackingEnabled(value);
            FB.Mobile.SetAdvertiserIDCollectionEnabled(value);
        }
    }
}