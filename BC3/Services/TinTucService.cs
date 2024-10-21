using BC3.Classes;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BC3.Services
{
    public class TinTucService
    {
        private readonly IGraphClient _client;

        public TinTucService(Neo4jService neo4jService)
        {
            _client = neo4jService.GetClient();
        }

        // Lấy hết tin tức
        public async Task<List<TinTuc>> GetAllNewsAsync()
        {
            var query = await _client.Cypher
                .Match("(t:TINTUC)")
                .Return(t => t.As<TinTuc>())
                .ResultsAsync;

            return query.ToList();
        }

        // Lấy tin tức theo id
        public async Task<TinTuc> GetTinTucByIDAsync(string id)
        {
            var result = await _client.Cypher
                .Match("(t:TINTUC)")
                .Where((TinTuc t) => t.MaTT == id)
                .Return(t => t.As<TinTuc>())
                .ResultsAsync;

            return result.FirstOrDefault();
        }
        // Thêm tin tức
        public async Task AddTinTuc(TinTuc t)
        {
            // Tạo node TINTUC và gán maTT ngay trong một truy vấn
            var result = await _client.Cypher
                .Create("(t:TTINTUC {TenTT: $TenTT, Quan: $Quan, ThanhPho: $ThanhPho, TrangThai_TT: $TrangThai_TT, DayShow: $DayShow, Hinh_TT: $Hinh_TT, NoiDung_TT: $NoiDung_TT, TomTat_TT: $TomTat_TT})")
                .WithParams(new
                {
                    t.TenTT,
                    t.Quan,
                    t.ThanhPho,
                    t.TrangThai_TT,
                    t.DayShow,
                    t.Hinh_TT,
                    t.NoiDung_TT,
                    t.TomTat_TT
                })
                .With("t")
                .Set("t.MaTT = id(t) + ''") // Gán MaTT cho node
                .Return(tts => tts.As<TinTuc>()) // Trả về node TinTuc
                .ResultsAsync;

            // Lấy node TinTuc vừa tạo
            var createdTinTuc = result.FirstOrDefault();

            if (createdTinTuc != null)
            {
                t.MaTT = createdTinTuc.MaTT; // Gán MaTT vào đối tượng TinTuc
            }
        }

        public async Task UpdateTinTuc(TinTuc t)
        {
            await _client.Cypher
                .Match("(t:TINTUC {MaTT: $MaTT})")
                .Set("t.TenTT = $DiaChi, t.Quan = $Quan, t.ThanhPho = $ThanhPho, t.TrangThai_TT = $TrangThai_TT, t.DayShow = $DayShow, , t.Hinh_TT = $Hinh_TT, , t.NoiDung_TT = $NoiDung_TT, , t.DayShow = $DayShow, , t.TomTat_TT = $TomTat_TT")
                .WithParams(new
                {
                    t.TenTT,
                    t.Quan,
                    t.ThanhPho,
                    t.TrangThai_TT,
                    t.DayShow,
                    t.Hinh_TT,
                    t.NoiDung_TT,
                    t.TomTat_TT
                })
                .ExecuteWithoutResultsAsync();
        }


        public async Task DeleteTinTuc(string id)
        {
            await _client.Cypher
                .Match("(t:TINTUC {MaTT: $MaTT})")
                .Delete("t")
                .WithParam("MaTT", id)
                .ExecuteWithoutResultsAsync();
        }
    }
}
