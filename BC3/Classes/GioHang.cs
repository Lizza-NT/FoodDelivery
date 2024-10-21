using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace BC3.Classes
{
    public class GioHang
    {
        private readonly IGraphClient _neo4jClient;
        public string ComboID { get; set; }
        public string TenCB { get; set; }
        public double GiaBan { get; set; }
        public int Soluong { get; set; }
        public double ThanhTien { get {  return GiaBan*Soluong; } }

        public GioHang(IGraphClient neo4jClient, string id)
        {
            _neo4jClient = neo4jClient;
            ComboID = id;
            LoadComboFromNeo(ComboID);
            Soluong = 0;
        }

        public async Task LoadComboFromNeo(string comboID)
        {
            var query = await _neo4jClient.Cypher
                .Match("(c:COMBO)")
                .Where((Combo c) => c.ComboID == comboID)
                .Return(c => c.As<Combo>())
                .ResultsAsync;  // Sử dụng ResultsAsync và await để đợi kết quả.

            var combo = query.SingleOrDefault();  // Lấy combo từ kết quả.
            if (combo != null)  // Kiểm tra combo thay vì query.
            {
                TenCB = combo.TenCombo;
                GiaBan = double.Parse(combo.GiaBan.ToString());
            }
        }

    }
}