using Sleipner.TestSite.Service.Netcore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sleipner.TestSite.Service.Netcore
{
    public interface IArticleService
    {

        Task<Article> GetArticleAsync(string Duid);
    }
}
