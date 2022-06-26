using System.ComponentModel;

namespace GenericDataMapper.Models;

public class Company
{
    [Description("id")]
    public int Id { get; set; }

    [Description("corporate_name")]
    public string CompanyName { get; set; }

    [Description("ticker_symbol")]
    public string TickerSymbol { get; set; }

    [Description("current_stock_price")]
    public double? StockPrice { get; set; }

    [Description("fiscal_year_end")]
    public DateTime? YearEnd { get; set; }

    [Description("board_member_1")]
    public string? BoardMember1 { get; set; }

    [Description("board_member_2")]
    public string? BoardMember2 { get; set; }

    [Description("board_member_3")]
    public string? BoardMember3 { get; set; }
}