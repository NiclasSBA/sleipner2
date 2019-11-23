using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sleipner.TestSite.Service.Netcore;
using Sleipner.TestSite.Service.Netcore.Models;

namespace SleipnerTestSite.Netcore.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }
        [Route("article/{duid}")]
        public async Task<ActionResult> GetArticle(string duid)
        {
            var value = await _articleService.GetArticleAsync("duidValue");

            return new OkObjectResult(value);
        }
    }
}