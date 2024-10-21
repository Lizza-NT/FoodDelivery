using BC3.Classes;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BC3.Services
{
    public class NguoiDungService
    {
        private readonly IGraphClient _client;

        public NguoiDungService(Neo4jService neo4jService)
        {
            _client = neo4jService.GetClient();
        }

        //đăng ký tài khoản
        public async Task<bool> RegisterUser(NguoiDung user)
        {
            try
            {
                // Kiểm tra xem username đã tồn tại chưa
                var existingUser = await _client.Cypher
                    .Match("(n:NGUOIDUNG {username: $username})")
                    .WithParam("username", user.username)
                    .Return(n => n.As<NguoiDung>())
                    .ResultsAsync;

                if (existingUser.Any())
                {
                    // Username đã tồn tại, không cho phép đăng ký
                    return false;
                }

                // Đặt role mặc định cho người dùng
                user.role = "Khach";

                // Tạo tài khoản mới nếu username chưa tồn tại
                await _client.Cypher
                    .Create("(user:NGUOIDUNG {username: $username, hoTen: $hoTen, sdt: $sdt, email: $email, password: $password, role: $role})")
                    .WithParams(new
                    {
                        username = user.username,
                        hoTen = user.hoTen,
                        sdt = user.sdt,
                        email = user.email,
                        password = user.password,
                        role = user.role
                    })
                    .ExecuteWithoutResultsAsync();

                return true; // Đăng ký thành công
            }
            catch (Exception)
            {
                return false; // Lỗi khi đăng ký
            }
        }

        //Đăng nhập 
        public async Task<NguoiDung> LoginUser(string username, string password)
        {
            var foundUser = await _client.Cypher
                .Match("(user:NGUOIDUNG)")
                .Where("user.username = $username AND user.password = $password")
                .WithParams(new
                {
                    username = username,
                    password = password
                })
                .Return(user => user.As<NguoiDung>())
                .ResultsAsync;

            return foundUser.FirstOrDefault();
        }

        //Lấy cửa hàng theo nhân viên
        public async Task<string> GetCuaHangByNhanVien(string username)
        {
            var result = await _client.Cypher
                .Match("(nguoiDung:NGUOIDUNG)-[:LAM_VIEC_TAI]->(cuaHang:CUAHANG)")
                .Where((NguoiDung nguoiDung) => nguoiDung.username == username)
                .Return(cuaHang => cuaHang.As<CuaHang>().TenCH)
                .ResultsAsync;

            return result.FirstOrDefault();  // Trả về tên cửa hàng
        }

        // Lấy tất tài khoản
        public async Task<List<NguoiDung>> GetAllNguoiDungAsync()
        {
            var query = await _client.Cypher
                .Match("(n:NGUOIDUNG)")
                .Return(n => n.As<NguoiDung>())
                .ResultsAsync;

            return query.ToList();
        }
        public async Task<NguoiDung> GetNguoiDungByUsernameAsync(string name)
        {
            var result = await _client.Cypher
                .Match("(nd:NGUOIDUNG)")
                .Where("nd.username = $username")
                .WithParam("username", name)
                .Return(nd => nd.As<NguoiDung>())
                .ResultsAsync;

            return result.SingleOrDefault();
        }


        public async Task AddNguoiDung(NguoiDung nd)
        {
            // Kiểm tra xem username đã tồn tại hay chưa
            var existingUser = await _client.Cypher
                .Match("(n:NGUOIDUNG)")
                .Where("n.username = $username")
                .WithParam("username", nd.username)
                .Return(n => n.As<NguoiDung>())
                .ResultsAsync;

            if (existingUser.Any())
            {
                throw new Exception("Tên người dùng đã tồn tại. Vui lòng chọn tên khác.");
            }

            // Nếu username không tồn tại, tiếp tục tạo người dùng mới
            await _client.Cypher
                .Create("(n:NGUOIDUNG {hoTen: $hoTen, sdt: $sdt, email: $email, password: $password, quan: $quan, duong: $duong, thanhpho: $thanhpho, role: $role, cuaHang: $cuaHang, username: $username})")
                .WithParams(new
                {
                    nd.hoTen,
                    nd.sdt,
                    nd.email,
                    nd.password,
                    nd.quan,
                    nd.duong,
                    nd.thanhpho,
                    nd.role,
                    nd.cuaHang,
                    nd.username
                })
                .ExecuteWithoutResultsAsync();
        }

        public async Task UpdateNguoiDung(NguoiDung nd)
        {
            // Kiểm tra xem người dùng có tồn tại hay không
            var existingUser = await _client.Cypher
                .Match("(n:NGUOIDUNG {username: $username})")
                .WithParam("username", nd.username)
                .Return(n => n.As<NguoiDung>())
                .ResultsAsync;

            if (!existingUser.Any())
            {
                throw new Exception("Người dùng không tồn tại.");
            }

            // Cập nhật thông tin người dùng
            await _client.Cypher
                .Match("(n:NGUOIDUNG {username: $username})")
                .Set("n.hoTen = $hoTen, n.sdt = $sdt, n.email = $email, n.password = $password, n.quan = $quan, n.duong = $duong, n.thanhpho = $thanhpho, n.role = $role, n.cuaHang = $cuaHang")
                .WithParams(new
                {
                    username = nd.username,
                    nd.hoTen,
                    nd.sdt,
                    nd.email,
                    nd.password,
                    nd.quan,
                    nd.duong,
                    nd.thanhpho,
                    nd.role,
                    nd.cuaHang
                })
                .ExecuteWithoutResultsAsync();
        }

        public async Task DeleteNguoiDung(string username)
        {
            await _client.Cypher
                .Match("(n:NGUOIDUNG {username: $username})")
                .Delete("n")
                .WithParam("username", username)
                .ExecuteWithoutResultsAsync();
        }
    }
}
