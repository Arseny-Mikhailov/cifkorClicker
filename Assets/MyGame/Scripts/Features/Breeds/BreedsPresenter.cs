using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace MyGame.Scripts
{
  public class BreedsPresenter
  {
    private readonly BreedsView _view;
    private readonly RequestQueue _queue;
    private CancellationTokenSource _cts;
    private CancellationTokenSource _detailCts;
    private BreedData[] _breedsList;
    private BreedData _detailData;

    [Inject]
    public BreedsPresenter(BreedsView view, RequestQueue queue)
    {
      _view = view;
      _queue = queue;
    }

    public void Start()
    {
      _cts = new CancellationTokenSource();
      LoadBreedsList().Forget();
    }

    public void Stop()
    {
      _cts.Cancel();
      _detailCts?.Cancel();
      _view.ShowLoading(false);
      _view.HidePopup();
    }

    private async UniTaskVoid LoadBreedsList()
    {
      _view.ClearList();
      _view.ShowLoading(true);

      try
      {
        await _queue.Enqueue(() => FetchBreeds(_cts.Token));
        if (_cts.Token.IsCancellationRequested) return;

        _view.ShowLoading(false);
        
        for (var i = 0; i < _breedsList.Length; i++)
        {
          var b = _breedsList[i];
          _view.AddBreedItem(i, b.name, () =>
          {
            OnBreedSelected(b.id, b.name);
          });
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

    private async UniTask FetchBreeds(CancellationToken ct)
    {
      ct.ThrowIfCancellationRequested();
      
      var uwr = UnityWebRequest.Get("https://api.thedogapi.com/v1/breeds");
      
      var cancelRegistration = ct.Register(() => uwr.Abort());

      try
      {
        var op = uwr.SendWebRequest();   
        if (op == null)
          throw new Exception("Не удалось запустить SendWebRequest()");
        
        await op.ToUniTask(cancellationToken: ct);
        
        if (uwr.result != UnityWebRequest.Result.Success)
          throw new Exception($"Ошибка запроса: {uwr.error}");
        
        var all = JsonHelper.FromJson<BreedData>(uwr.downloadHandler.text);
        _breedsList = all.Take(10).ToArray();
      }
      finally
      {
        await cancelRegistration.DisposeAsync();
        uwr.Dispose();
      }
    }

    private void OnBreedSelected(int id, string name)
    {
      _detailCts?.Cancel();
      _detailCts = CancellationTokenSource
        .CreateLinkedTokenSource(_cts.Token);

      ShowBreedDetail(id, name, _detailCts.Token).Forget();
    }

    private async UniTaskVoid ShowBreedDetail(
      int id, string name, CancellationToken ct)
    {
      _view.ShowLoading(true);
      _view.HidePopup();

      try
      {
        await _queue.Enqueue(() => FetchBreedDetail(id, ct));
        if (ct.IsCancellationRequested) return;
        
        _view.ShowPopup(
          _detailData.name,
          _detailData.temperament
        );
      }
      catch (OperationCanceledException)
      {
      }
      finally
      {
        _view.ShowLoading(false);
      }
    }

    private async UniTask FetchBreedDetail(int id, CancellationToken ct)
    {
      ct.ThrowIfCancellationRequested();
      var uwr = UnityWebRequest.Get($"https://api.thedogapi.com/v1/breeds/{id}");
      var cancelRegistration = ct.Register(() => uwr.Abort());
      try
      {
        var op = uwr.SendWebRequest();
        if (op == null)
          throw new Exception("Не удалось запустить SendWebRequest()");

        await op.ToUniTask(cancellationToken: ct);

        if (uwr.result != UnityWebRequest.Result.Success)
          throw new Exception($"Ошибка: {uwr.error}");

        _detailData = JsonUtility.FromJson<BreedData>(uwr.downloadHandler.text);
      }
      finally
      {
        await cancelRegistration.DisposeAsync();
        uwr.Dispose();
      }
    }
  }
}