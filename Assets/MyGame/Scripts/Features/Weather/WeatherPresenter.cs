using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MyGame.Scripts.Core;
using MyGame.Scripts.UI;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace MyGame.Scripts.Features.Weather
{
    public class WeatherPresenter : IInitializable, IDisposable
    {
        private const string ApiUrl = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

        private const float IntervalSec = 5f;

        private readonly RequestQueue _queue;
        private readonly WeatherView _view;
        private CancellationTokenSource _cts;

        [Inject]
        public WeatherPresenter(WeatherView view, RequestQueue queue)
        {
            _view = view;
            _queue = queue;
        }

        public void Dispose()
        {
            _view.Showed -= OnShow;
            _view.Hidden -= OnHide;
            _cts?.Cancel();
            _cts?.Dispose();
        }

        public void Initialize()
        {
            _view.Showed += OnShow;
            _view.Hidden += OnHide;
        }

        private void OnShow()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            UpdateWeatherAsync(_cts.Token).Forget();
        }

        private void OnHide()
        {
            _cts?.Cancel();
        }

        private async UniTaskVoid UpdateWeatherAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await _queue.Enqueue(() => FetchAndDisplayAsync(token));

                    await UniTask.Delay(TimeSpan.FromSeconds(IntervalSec), cancellationToken: token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        private async UniTask FetchAndDisplayAsync(CancellationToken token)
        {
            using var req = UnityWebRequest.Get(ApiUrl);
            var op = req.SendWebRequest();
            await op.ToUniTask(cancellationToken: token);

            if (req.result != UnityWebRequest.Result.Success)
            { 
                return;
            }

            var p = JsonUtility.FromJson<ForecastResponse>(req.downloadHandler.text)?.properties?.periods?[0];
            if (p == null)
            { 
                return;
            }

            var label = $"{p.name} – {p.temperature}{p.temperatureUnit}";
            
            Sprite icon = null;
            using var ireq = UnityWebRequestTexture.GetTexture(p.icon);
            var iop = ireq.SendWebRequest();
            await iop.ToUniTask(cancellationToken: token);

            if (ireq.result == UnityWebRequest.Result.Success)
            {
                var tex = DownloadHandlerTexture.GetContent(ireq);
                icon = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
            }

            _view.SetWeather(label, icon);
        }

        [Serializable]
        private class ForecastResponse
        {
            public Properties properties;
        }

        [Serializable]
        private class Properties
        {
            public Period[] periods;
        }

        [Serializable]
        private class Period
        {
            public string name;
            public int temperature;
            public string temperatureUnit;
            public string icon;
        }
    }
}