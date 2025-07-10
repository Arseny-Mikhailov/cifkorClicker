using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace MyGame.Scripts.Core
{
    public class RequestQueue
    {
        private readonly Queue<Func<UniTask>> _queue = new();
        private bool _isRunning;

        public static RequestQueue Instance { get; } = new();

        public async UniTask Enqueue(Func<UniTask> request)
        {
            _queue.Enqueue(request);
            
            if (!_isRunning)
            {
                await ProcessQueue();
            }
        }

        private async UniTask ProcessQueue()
        {
            _isRunning = true;

            while (_queue.Count > 0)
            {
                var req = _queue.Dequeue();
                await req();
            }

            _isRunning = false;
        }
    }
}