using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QuotesExchangeApp.Data.Migrations
{
    public partial class added_seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Name", "Ticker" },
                values: new object[] { new Guid("128c3e7a-89a1-4947-841c-3a6de93c919f"), "Apple", "AAPL" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Name", "Ticker" },
                values: new object[] { new Guid("f7216ad4-1954-476a-b79d-9e1cc031f149"), "Tesla", "TSLA" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Name", "Ticker" },
                values: new object[] { new Guid("9f09d3ba-20b8-47ea-a2aa-92aa2d1e572f"), "AMD", "AMD" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Name", "Ticker" },
                values: new object[] { new Guid("460130bd-e137-4cce-957c-d14de307b2d1"), "Intel", "INTC" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Name", "Ticker" },
                values: new object[] { new Guid("842e5181-9984-4832-b0fb-7609381af2e1"), "Amazon", "AMZN" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Name", "Ticker" },
                values: new object[] { new Guid("002ba0ef-a823-4a1c-bde6-79e3f41bc822"), "Microsoft", "MSFT" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Name", "Ticker" },
                values: new object[] { new Guid("fca092a9-6159-4baf-bde9-408db160c615"), "Газпром", "GAZP" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Name", "Ticker" },
                values: new object[] { new Guid("8b36c9b1-9034-4b0a-8e19-7da3d78575f9"), "Яндекс", "YNDX" });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "ApiUrl", "Name" },
                values: new object[] { new Guid("21f4fe1c-d9a5-47de-81d8-814367210f57"), "https://finnhub.io/api/v1/quote?symbol=", "Finnhub" });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "ApiUrl", "Name" },
                values: new object[] { new Guid("d19aef7d-16c2-4649-b490-7e30cc751ef3"), "https://iss.moex.com/iss/engines/stock/markets/shares/securities/", "MOEX" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("002ba0ef-a823-4a1c-bde6-79e3f41bc822"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("128c3e7a-89a1-4947-841c-3a6de93c919f"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("460130bd-e137-4cce-957c-d14de307b2d1"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("842e5181-9984-4832-b0fb-7609381af2e1"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("8b36c9b1-9034-4b0a-8e19-7da3d78575f9"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("9f09d3ba-20b8-47ea-a2aa-92aa2d1e572f"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("f7216ad4-1954-476a-b79d-9e1cc031f149"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("fca092a9-6159-4baf-bde9-408db160c615"));

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: new Guid("21f4fe1c-d9a5-47de-81d8-814367210f57"));

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: new Guid("d19aef7d-16c2-4649-b490-7e30cc751ef3"));
        }
    }
}
