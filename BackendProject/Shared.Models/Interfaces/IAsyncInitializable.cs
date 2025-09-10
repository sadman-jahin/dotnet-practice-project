using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Resources.Interfaces
{
    public interface IAsyncInitializable
    {
        Task InitializeAsync();
    }

}
