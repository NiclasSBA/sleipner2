using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sleipner.TestSite.Service.Netcore.Models;

namespace Sleipner.TestSite.Service.Netcore
{
    public class ArticleService : IArticleService
    {
        public async Task<Article> GetArticleAsync(string Duid)
        {
            Thread.Sleep(5000);
            return await Task.Factory.StartNew( () => new Article() { Duid = "12341234", Name = "Super Awesome Article"});
        }
    }
}
