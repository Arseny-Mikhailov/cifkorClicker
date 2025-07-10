using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MyGame.Scripts.UI;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace MyGame.Scripts.Features.Weather
{
    public class WeatherPresenter : IInitializable, IDisposable
    {
        private readonly WeatherView _view;
        private readonly RequestQueue _queue;

        private CancellationTokenSource _loopCts;
        private CancellationTokenSource _requestCts;

        private const string ApiUrl = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";
        private const float IntervalSec = 5f;

        public WeatherPresenter(WeatherView view, RequestQueue queue)
        {
            _view = view;
            _queue = queue;
        }

        public void Initialize()
        {
            _view.Showed += OnShow;
            _view.Hidden += OnHide;
        }

        public void Dispose()
        {
            _view.Showed -= OnShow;
            _view.Hidden -= OnHide;
            CancelAll();
        }

        private void OnShow() => StartLoop().Forget();

        private void OnHide() => CancelAll();

        private void CancelAll()
        {
            _loopCts?.Cancel();
            _requestCts?.Cancel();
        }

        private async UniTaskVoid StartLoop()
        {
            CancelAll();
            _loopCts = new CancellationTokenSource();
            var loopToken = _loopCts.Token;

            await EnqueueRequest(loopToken);

            while (!loopToken.IsCancellationRequested)
            {
                try
                {
                    await UniTask.Delay(
                        TimeSpan.FromSeconds(IntervalSec),
                        cancellationToken: loopToken
                    );
                }
                catch (OperationCanceledException) { break; }

                if (loopToken.IsCancellationRequested) break;
                await EnqueueRequest(loopToken);
            }
        }

        private async UniTask EnqueueRequest(CancellationToken loopToken)
        {
            _requestCts?.Cancel();
            _requestCts = CancellationTokenSource.CreateLinkedTokenSource(loopToken);
            var reqToken = _requestCts.Token;

            await _queue.Enqueue(async () =>
            {
                await FetchAndDisplay(reqToken);
            });
        }

        private async UniTask FetchAndDisplay(CancellationToken token)
        {
            using var req = UnityWebRequest.Get(ApiUrl);
            req.timeout = 10;
            await req.SendWebRequest().WithCancellation(token);

            if (token.IsCancellationRequested
                || req.result is UnityWebRequest.Result.ConnectionError
                    or UnityWebRequest.Result.ProtocolError)
                return;

            var data = JsonUtility.FromJson<ForecastResponse>(req.downloadHandler.text);
            var p = data?.properties?.periods?[0];
            if (p == null) return;

            var label = $"{p.name} - {p.temperature}{p.temperatureUnit}";
            Sprite icon = null;

            using (var iconReq = UnityWebRequestTexture.GetTexture(p.icon))
            {
                await iconReq.SendWebRequest().WithCancellation(token);
                if (!(token.IsCancellationRequested
                      || iconReq.result is UnityWebRequest.Result.ConnectionError
                          or UnityWebRequest.Result.ProtocolError))
                {
                    var tex = DownloadHandlerTexture.GetContent(iconReq);
                    icon = Sprite.Create(
                        tex,
                        new Rect(0, 0, tex.width, tex.height),
                        new Vector2(0.5f, 0.5f)
                    );
                }
            }

            if (!token.IsCancellationRequested)
                _view.SetWeather(label, icon);
        }

        [Serializable]
        private class ForecastResponse { public Properties properties; }
        [Serializable]
        private class Properties { public Period[] periods; }
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