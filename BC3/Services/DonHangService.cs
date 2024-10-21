using BC3.Classes;
using BC3.ViewModel;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BC3.Services
{
    public class DonHangService
    {
        private readonly IGraphClient _client;
        public DonHangService(Neo4jService neo4jService)
        {
            _client = neo4jService.GetClient();
        }

        public async Task<List<DonHangViewModel>> Get_DonHang_ByCuaHang_And_TrangThai(string tenCuaHang, string trangThai)
        {
            var result = await _client.Cypher
                .Match("(nguoiDung:NGUOIDUNG)-[:DAT_HANG]->(donHang:DONHANG)-[:DAT_DON_TAI]->(cuaHang:CUAHANG), (donHang)-[:BAOGOM_COMBO]->(combo:COMBO)")
                .Where("cuaHang.TenCH = $tenCuaHang AND donHang.TrangThai = $trangThai")  // Lọc theo tên cửa hàng và trạng thái đơn hàng
                .WithParam("tenCuaHang", tenCuaHang)  // Thêm tham số cho tên cửa hàng
                .WithParam("trangThai", trangThai)    // Thêm tham số cho trạng thái đơn hàng
                .With("nguoiDung, donHang, cuaHang, COLLECT(combo.TenCombo) AS MonDat, COLLECT(combo.GiaBan) AS GiaBanList")  // Thu thập tên món ăn và giá bán
                .Return((nguoiDung, donHang) => new
                {
                    TenNguoiDat = nguoiDung.As<NguoiDung>().hoTen,
                    SoDienThoai = nguoiDung.As<NguoiDung>().sdt,
                    Duong = nguoiDung.As<NguoiDung>().duong,
                    Quan = nguoiDung.As<NguoiDung>().quan,
                    ThanhPho = nguoiDung.As<NguoiDung>().thanhpho,
                    MaDonHang = donHang.As<DonHang>().MaDH,
                    NgayGiao = donHang.As<DonHang>().NgayGiao,
                    TenComboGop = Return.As<List<string>>("MonDat"),  // Sử dụng MonDat để lấy tên món
                    ThanhTien = Return.As<decimal>("REDUCE(total = 0, price IN GiaBanList | total + price)")  // Tính tổng giá trị từ danh sách giá bán
                })
                .ResultsAsync;

            return result.Select(res => new DonHangViewModel
            {
                TenNguoiDat = res.TenNguoiDat,
                SoDienThoai = res.SoDienThoai,
                DiaChi = $"{res.Duong} {res.Quan} {res.ThanhPho}",
                MaDonHang = res.MaDonHang,
                NgayGiao = res.NgayGiao,
                TenComboGop = res.TenComboGop,
                ThanhTien = res.ThanhTien
            }).ToList();
        }

        public async Task<List<DonHang>> GetAllOrdersAsync()
        {
            var query = await _client.Cypher
                .Match("(d:DONHANG)")
                .Return(d => d.As<DonHang>())
                .ResultsAsync;

            return query.ToList();
        }
        public async Task<DonHang> GetDonHangByIDAsync(string id)
        {
            var result = await _client.Cypher
                .Match("(d:DONHANG)")
                .Where((DonHang d) => d.MaDH == id)
                .Return(d => d.As<DonHang>())
                .ResultsAsync;

            return result.SingleOrDefault();
        }
        public async Task UpdateDonHang_TrangThaiAsync(DonHang donHang)
        {
            await _client.Cypher
                .Match("(d:DONHANG {MaDH: $MaDH})")
                .Set("d.TrangThai = $TrangThai")
                .WithParams(new
                {
                    donHang.MaDH,
                    donHang.TrangThai
                })
                .ExecuteWithoutResultsAsync();
        }

        public async Task AddDonHang(DonHang d)
        {
            var result = await _client.Cypher
                .Create("(d:DONHANG {MaDH: $MaDH, Duong: $Duong, Quan: $Quan, ThanhPho: $ThanhPho, NgayGiao: $NgayGiao, TrangThai: $TrangThai, ThanhTien: $ThanhTien})")
                .WithParams(new
                {
                    d.MaDH,
                    d.Duong,
                    d.Quan,
                    d.ThanhPho,
                    d.NgayGiao,
                    d.TrangThai,
                    d.ThanhTien
                })
                .With("d")
                .Set("d.MaDH = id(d) + ''")
                .Return(dhs => dhs.As<DonHang>())
                .ResultsAsync;


            var createdDonHang = result.FirstOrDefault();

            if (createdDonHang != null)
            {
                d.MaDH = createdDonHang.MaDH;
            }
        }

        public async Task UpdateDonHang(DonHang d)
        {
            await _client.Cypher
                .Match("(d:DONHANG {MaDH: $MaDH})")
                .Set("d.MaDH: $MaDH, d.Duong: $Duong, d.Quan: $Quan, d.ThanhPho: $ThanhPho, d.NgayGiao: $NgayGiao, d.TrangThai: $TrangThai, d.ThanhTien: $ThanhTien")
                .WithParams(new
                {
                    d.MaDH,
                    d.Duong,
                    d.Quan,
                    d.ThanhPho,
                    d.NgayGiao,
                    d.TrangThai,
                    d.ThanhTien
                })
                .ExecuteWithoutResultsAsync();
        }

        public async Task DeleteDonHang(string id)
        {
            await _client.Cypher
                .Match("(d:DONHANG {MaDH: $MaDH})")
                .Delete("d")
                .WithParam("MaDH", id)
                .ExecuteWithoutResultsAsync();
        }
    }
}