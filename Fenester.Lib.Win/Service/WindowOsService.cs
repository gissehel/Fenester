using Fenester.Lib.Core.Domain.Graphical;
using Fenester.Lib.Core.Domain.Os;
using Fenester.Lib.Core.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fenester.Lib.Win.Service
{
    public class WindowOsService : IWindowOsService
    {
        public WindowOsService()
        {
        }

        public Task<IRectangle> GetWindowCurrentPosition(IWindow window)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IWindow>> GetWindows()
        {
            throw new NotImplementedException();
        }

        public Task<IWindow> Hide(IWindow hide)
        {
            throw new NotImplementedException();
        }

        public Task<IRectangle> Move(IWindow window, IRectangle rectangle)
        {
            throw new NotImplementedException();
        }

        public Task<IWindow> Show(IWindow window)
        {
            throw new NotImplementedException();
        }
    }
}