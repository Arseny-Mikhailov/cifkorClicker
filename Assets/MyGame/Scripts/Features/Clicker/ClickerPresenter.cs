using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace MyGame.Scripts
{
    public class ClickerPresenter
    {
        private readonly ClickerModel _model;
        private readonly ClickerView _view;
        private CancellationTokenSource _cts;

        public ClickerPresenter(ClickerModel model, ClickerView view)
        {
            _model = model;
            _view = view;

            _view.OnClick += HandleClick;
        }

        public void Start()
        {
            _cts = new CancellationTokenSource();
            StartAutoClick(_cts.Token).Forget();
            StartEnergyRestore(_cts.Token).Forget();
            UpdateUI();
        }

        public void Stop()
        {
            _cts?.Cancel();
        }

        private void HandleClick()
        {
            if (!_model.TryClick()) return;

            _view.PlayClickVFX();
            UpdateUI();
        }

        private async UniTaskVoid StartAutoClick(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_model.Config.AutoClickInterval), cancellationToken: token);

                if (!_model.TryClick()) continue;

                _view.PlayClickVFX();
                UpdateUI();
            }
        }

        private async UniTaskVoid StartEnergyRestore(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_model.Config.EnergyRestoreInterval),
                    cancellationToken: token);
                _model.RestoreEnergy();
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            _view.SetCurrency(_model.Currency);
            _view.SetEnergy(_model.Energy);
        }
    }
}