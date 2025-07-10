using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using MyGame.Scripts.Core;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace MyGame.Scripts.Features
{
    public class BreedsPresenter
    {
        private readonly RequestQueue _queue;
        private readonly BreedsView _view;
        private BreedData[] _breeds;
        private CancellationTokenSource _cts;
        private BreedData _detail;

        [Inject]
        public BreedsPresenter(BreedsView view, RequestQueue queue)
        {
            _view = view;
            _queue = queue;
        }
        
        public void Start()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            LoadListAsync(_cts.Token).Forget();
        }

        public void Stop()
        {
            _cts?.Cancel();
            _view.ShowLoading(false);
            _view.HidePopup();
        }

        private async UniTaskVoid LoadListAsync(CancellationToken token)
        {
            _view.ClearList();
            _view.ShowLoading(true);

            try
            {
                await _queue.Enqueue(() => FetchBreedsAsync(token));

                foreach (var b in _breeds.Take(10))
                { 
                    _view.AddBreedItem(b.id, b.name, 
                        () => ShowDetailAsync(b.id, token).Forget());
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                _view.ShowLoading(false);
            }
        }

        private async UniTask FetchBreedsAsync(CancellationToken token)
        {
            using var req = UnityWebRequest.Get("https://api.thedogapi.com/v1/breeds");
            var op = req.SendWebRequest();
            await op.ToUniTask(cancellationToken: token);

            if (req.result != UnityWebRequest.Result.Success)
            { 
                throw new Exception(req.error);
            }

            _breeds = JsonHelper.FromJson<BreedData>(req.downloadHandler.text);
        }

        private async UniTaskVoid ShowDetailAsync(int id, CancellationToken token)
        {
            _view.ShowLoading(true);
            _view.HidePopup();

            try
            {
                await _queue.Enqueue(() => FetchDetailAsync(id, token));

                _view.ShowPopup(_detail.name, _detail.temperament);
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                _view.ShowLoading(false);
            }
        }

        private async UniTask FetchDetailAsync(int id, CancellationToken token)
        {
            using var req = UnityWebRequest.Get($"https://api.thedogapi.com/v1/breeds/{id}");
            var op = req.SendWebRequest();
            await op.ToUniTask(cancellationToken: token);

            if (req.result != UnityWebRequest.Result.Success)
            { 
                throw new Exception(req.error);
            }

            _detail = JsonUtility.FromJson<BreedData>(req.downloadHandler.text);
        }
    }
}