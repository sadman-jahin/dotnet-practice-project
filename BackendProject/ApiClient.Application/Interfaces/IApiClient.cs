using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ApiClient.Application.Interfaces
{
    public interface IApiClient
    {
        Task<T> GetDataAsync<T>(string url);
    }
}
