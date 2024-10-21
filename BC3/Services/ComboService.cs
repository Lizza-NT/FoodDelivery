using BC3.Classes;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BC3.Services
{
    public class ComboService
    {
        private readonly IGraphClient _client;

        public ComboService(Neo4jService neo4jService)
        {
            _client = neo4jService.GetClient();
        }

        // Lấy tất cả combo
        public async Task<List<Combo>> GetAllComboAsync()
        {
            var query = await _client.Cypher
                .Match("(c:COMBO)")
                .Return(c => c.As<Combo>())
                .ResultsAsync;

            return query.ToList();
        }

        // Lấy combo theo ID
        public async Task<Combo> GetComboByIDAsync(string id)
        {
            var result = await _client.Cypher
                .Match("(c:COMBO)")
                .Where((Combo c) => c.ComboID == id)
                .Return(c => c.As<Combo>())
                .ResultsAsync;

            return result.FirstOrDefault();
        }

        //Lấy combo theo nhóm
        public async Task<List<Combo>> GetCombosByGroupIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new List<Combo>(); // Trả về danh sách trống nếu id rỗng
            }

            var combos = await _client.Cypher
                .Match("(c:COMBO)")
                .Where("c.MaNhom = $id")
                .WithParam("id", id)
                .Return(c => c.As<Combo>())
                .ResultsAsync;

            return combos.ToList();
        }

        //Lấy tất cả mã nhóm món ăn
        public async Task<List<Nhom>> GetNhomComboList()
        {
            var query = await _client.Cypher
                 .Match("(n:NHOM)")
                 .Return(n => n.As<Nhom>())
                 .ResultsAsync;

            return query.ToList();
        }

        // Thêm món ăn
        public async Task AddCombo(Combo cb)
        {
            // Tạo node COMBO và gán ComboID ngay trong một truy vấn
            var result = await _client.Cypher
                .Create("(c:COMBO {TenCombo: $TenCombo, GiaBan: $GiaBan, GiaThuc: $GiaThuc, Hinh: $Hinh, CT_Combo: $CT_Combo, MaNhom: $MaNhom})")
                .WithParams(new
                {
                    cb.TenCombo,
                    cb.GiaBan,
                    cb.GiaThuc,
                    cb.Hinh,
                    cb.CT_Combo,
                    cb.MaNhom
                })
                .With("c")
                .Set("c.ComboID = id(c) + ''") // Gán ComboID cho node
                .Return(c => c.As<Combo>()) // Trả về node Combo
                .ResultsAsync;

            // Lấy node Combo vừa tạo
            var createdCombo = result.FirstOrDefault();

            if (createdCombo != null)
            {
                cb.ComboID = createdCombo.ComboID; // Gán ComboID vào đối tượng Combo
            }
        }

        // Sửa món ăn
        public async Task UpdateCombo(Combo cb)
        {
            await _client.Cypher
                .Match("(c:COMBO {ComboID: $ComboID})")
                .Set("c.TenCombo = $TenCombo, c.GiaBan = $GiaBan, c.GiaThuc = $GiaThuc, c.Hinh = $Hinh, c.CT_Combo = $CT_Combo, c.MaNhom = $MaNhom")
                .WithParams(new
                {
                    cb.ComboID,
                    cb.TenCombo,
                    cb.GiaBan,
                    cb.GiaThuc,
                    cb.Hinh,
                    cb.CT_Combo,
                    cb.MaNhom
                })
                .ExecuteWithoutResultsAsync();
        }

        // Xoá món ăn
        public async Task DeleteCombo(string id)
        {
            await _client.Cypher
                .Match("(c:COMBO {ComboID: $ComboID})")
                .Delete("c")
                .WithParam("ComboID", id)
                .ExecuteWithoutResultsAsync();
        }
    }

}