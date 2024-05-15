using System;
using SpawnConverter.Logs;

namespace SpawnConverter.FStream
{
    public class FileReader : LogEvent, IDisposable
    {
        private bool disposed = false;

        public void Close() => Dispose();
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            disposed = true;
        }
    }
}
