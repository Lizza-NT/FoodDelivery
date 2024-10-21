using BC3.Classes;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BC3.Services
{
    public class UuDaiService
    {
        private readonly IGraphClient _client;
        public UuDaiService(Neo4jService neo4jService)
        {
            _client = neo4jService.GetClient();
        }
        //Lấy hết tin tức
        public async Task<List<UuDai>> GetAllVoucherAsync()
        {
            var query = await _client.Cypher
                .Match("(u:UUDAI)")
                .Return(u => u.As<UuDai>())
                .ResultsAsync;

            return query.ToList();
        }
        public async Task<UuDai> GetUuDaiByIDAsync(string id)
        {
            var result = await _client.Cypher
                .Match("(u:UUDAI)")
                .Where((UuDai u) => u.MaUD == id)
                .Return(u => u.As<UuDai>())
                .ResultsAsync;

            return result.FirstOrDefault();
        }

        public async Task AddUuDai(UuDai u)
        {
            // Tạo node UUDAI và gán maUD ngay trong một truy vấn
            var result = await _client.Cypher
                .Create("(u:UUDAI {TenUD: $TenUD, DayStart: $DayStart, DayEnd: $DayEnd, TrangThai_UD: $TrangThai_UD, Hinh_UD: $Hinh_UD, NoiDung_UD: $NoiDung_UD, TomTat_UD: $TomTat_UD})")
                .WithParams(new
                {
                    u.TenUD,
                    u.DayStart,
                    u.DayEnd,
                    u.TrangThai_UD,
                    u.Hinh_UD,
                    u.NoiDung_UD,
                    u.TomTat_UD
                })
                .With("u")
                .Set("u.MaUD = id(u) + ''") // Gán MaTT cho node
                .Return(uds => uds.As<UuDai>()) // Trả về node TinTuc
                .ResultsAsync;

            // Lấy node TinTuc vừa tạo
            var createdUuDai = result.FirstOrDefault();

            if (createdUuDai != null)
            {
                u.MaUD = createdUuDai.MaUD; // Gán MaTT vào đối tượng TinTuc
            }
        }

        public async Task UpdateUuDai(UuDai u)
        {
            await _client.Cypher
                .Match("(u:UUDAI {MaUD: $MaUD})")
                .Set("u.TenUD: $TenUD, u.DayStart: $DayStart, u.DayEnd: $DayEnd, u.TrangThai_UD: $TrangThai_UD, u.Hinh_UD: $Hinh_UD, u.NoiDung_UD: $NoiDung_UD, u.TomTat_UD: $TomTat_UD")
                .WithParams(new
                {
                    u.TenUD,
                    u.DayStart,
                    u.DayEnd,
                    u.TrangThai_UD,
                    u.Hinh_UD,
                    u.NoiDung_UD,
                    u.TomTat_UD
                })
                .ExecuteWithoutResultsAsync();
        }


        public async Task DeleteUuDai(string id)
        {
            await _client.Cypher
                .Match("(u:UUDAI {MaUD: $MaUD})")
                .Delete("u")
                .WithParam("MaUD", id)
                .ExecuteWithoutResultsAsync();
        }
    }
}