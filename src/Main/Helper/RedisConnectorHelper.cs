using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace OnlineOrderInfo.Helper
{
    public class RedisConnectorHelper
    {
        
        public  RedisConnectorHelper()
        {
            lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                string cacheConnection = ConfigurationManager.AppSettings["RedisCacheConnectionString"].ToString();

                return ConnectionMultiplexer.Connect(cacheConnection);
            });

        }
        public Lazy<ConnectionMultiplexer> lazyConnection;
        public IDatabase CacheDB
        {
            get
            {
                return Connection.GetDatabase();
            }
        }
        public ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}