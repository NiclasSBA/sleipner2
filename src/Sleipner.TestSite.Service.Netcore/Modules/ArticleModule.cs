using Autofac;
using Sleipner.Cache.Netcore;
using Sleipner.Cache.Netcore.Configuration;
using Sleipner.Cache.Netcore.Configuration.Expressions;
using Sleipner.Cache.Netcore.RedisSharp;
using Sleipner.Cache.Netcore.RedisSharp.Wrapper;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sleipner.TestSite.Service.Netcore.Modules
{
   public class ArticleModule : Module
    {


        protected override void Load(ContainerBuilder builder)
        {
            var options = new ConfigurationOptions() { EndPoints = { "46.101.249.95" } };
            var redis = new RedisClient(options);
                var proxy = new SleipnerCache<IArticleService>(new ArticleService(), new RedisProvider<IArticleService>(redis));

                proxy.Config(a =>
                {
                    a.DefaultIs().CacheFor(10);
                    a.For(x => x.GetArticleAsync(Param.IsAny<string>())).CacheFor(10);
                });

            builder.RegisterInstance(proxy.CreateCachedInstance()).As<IArticleService>().SingleInstance();
      
            
        }
    }
}
