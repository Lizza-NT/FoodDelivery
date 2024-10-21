using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BC3.Services
{
    public class Neo4jService
    {
        private readonly IGraphClient _client;

        public Neo4jService()
        {
            var uri = ConfigurationManager.AppSettings["Neo4jUri"];
            var username = ConfigurationManager.AppSettings["Neo4jUsername"];
            var password = ConfigurationManager.AppSettings["Neo4jPassword"];
            try
            {
                _client = new BoltGraphClient(new Uri(uri), username, password);
                _client.ConnectAsync().Wait();
            }
            catch (Exception ex)
            {
                // Ghi lại thông tin chi tiết của ngoại lệ
                Console.WriteLine($"Lỗi khi kết nối tới Neo4j: {ex.Message}");
                throw; // Ném lại ngoại lệ để xử lý ngoài
            }
        }
        public IGraphClient GetClient()
        {
            return _client;
        }
    }
}