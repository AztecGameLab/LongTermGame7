using System;

namespace Application.Core
{
    public class DisposableBag : IDisposable
    {
        private readonly IDisposable[] _disposables;
        
        public DisposableBag(IDisposable[] disposables)
        {
            _disposables = disposables;
        }

        public void Dispose()
        {
            foreach (IDisposable disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}