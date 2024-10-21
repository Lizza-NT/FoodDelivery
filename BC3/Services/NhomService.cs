using BC3.Classes;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BC3.Services
{
    public class NhomService
    {
        private readonly IGraphClient _client;

        public NhomService(Neo4jService neo4jService)
        {
            _client = neo4jService.GetClient();
        }

        // Lấy tất cả nhóm món ăn
        public async Task<List<Nhom>> GetAllNhomAsync()
        {
            var query = await _client.Cypher
                .Match("(n:NHOM)")
                .Return(n => n.As<Nhom>())
                .ResultsAsync;

            return query.ToList();
        }
    }
}
