using BC3.Classes;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BC3.Services
{
    public class CuaHangService
    {
        private readonly IGraphClient _client;
        public CuaHangService(Neo4jService neo4jService)
        {
            _client = neo4jService.GetClient();
        }

        //lấy tất cả có sắp xếp
        public async Task<List<CuaHang>> GetStoresByRegionAsync()
        {

            var query = await _client.Cypher
            .Match("(kv:QUAN)<-[:THUOC_QUAN]-(ch:CUAHANG)")
            .Return((kv, ch) => new
            {
                Store = ch.As<CuaHang>(),
                Region = kv.As<Quan>()
            })
            .OrderBy("kv.MaKV")  // Sắp xếp theo MaKV
            .ResultsAsync;

            return query.Select(result => result.Store).ToList();

        }
        // Lấy cuahang theo tenCH
        public async Task<CuaHang> GetCuaHangByIDAsync(string id)
        {
            var result = await _client.Cypher
                .Match("(ch:CUAHANG)")
                .Where((CuaHang ch) => ch.TenCH == id)
                .Return(ch => ch.As<CuaHang>())
                .ResultsAsync;

            return result.FirstOrDefault();
        }

        //Lấy cuahang theo thành phố
        public async Task<List<CuaHang>> GetCuaHangsByGroupIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new List<CuaHang>(); // Trả về danh sách trống nếu id rỗng
            }

            var cuahangs = await _client.Cypher
                .Match("(ch:CUAHANG)")
                .Where("ch.ThanhPho = $id")
                .WithParam("id", id)
                .Return(ch => ch.As<CuaHang>())
                .ResultsAsync;

            return cuahangs.ToList();
        }

        //Lấy tất cả mã nhóm cuahang
        public async Task<List<CuaHang>> GetAllCuaHang()
        {
            var cuahangs = await _client.Cypher
                 .Match("(ch:CUAHANG)")
                 .Return(ch => ch.As<CuaHang>())
                 .ResultsAsync;

            return cuahangs.ToList();
        }

        // Thêm cuahang
        public async Task AddCuaHang(CuaHang ch)
        {
            // Tạo node CUAHANG và gán THANHPHO ngay trong một truy vấn
            var result = await _client.Cypher
                .Create("(ch:CUAHANG {TenCH: $TenCH, DiaChi: $DiaChi, Quan: $Quan, ThanhPho: $ThanhPho, GioMo: $GioMo, GioDong: $GioDong, HotLine: $HotLine})")
                .WithParams(new
                {
                    ch.TenCH,
                    ch.DiaChi,
                    ch.Quan,
                    ch.ThanhPho,
                    ch.GioMo,
                    ch.GioDong,
                    ch.HotLine
                })
                .With("ch")
                .Set("ch.ThanhPho = id(ch) + ''") // Gán ThanhPho cho node
                .Return(chs => chs.As<CuaHang>()) // Trả về node CuaHang
                .ResultsAsync;

            // Lấy node Combo vừa tạo
            var createdCuaHang = result.FirstOrDefault();

            if (createdCuaHang != null)
            {
                ch.ThanhPho = createdCuaHang.ThanhPho; // Gán ThanhPho vào đối tượng CuaHang
            }
        }

        // Sửa món ăn
        // Sửa thông tin cửa hàng
        public async Task UpdateCuaHang(CuaHang ch)
        {
            await _client.Cypher
                .Match("(c:CUAHANG {TenCH: $TenCH})")
                .Set("c.DiaChi = $DiaChi, c.Quan = $Quan, c.GioMo = $GioMo, c.GioDong = $GioDong, c.HotLine = $HotLine")
                .WithParams(new
                {
                    ch.TenCH,
                    ch.DiaChi,
                    ch.Quan,
                    ch.GioMo,
                    ch.GioDong,
                    ch.HotLine
                })
                .ExecuteWithoutResultsAsync();
        }


        public async Task DeleteCuaHang(string id)
        {
            await _client.Cypher
                .Match("(c:CUAHANG {TenCH: $TenCH})")
                .Delete("c")
                .WithParam("TenCH", id)
                .ExecuteWithoutResultsAsync();
        }
    }
}

