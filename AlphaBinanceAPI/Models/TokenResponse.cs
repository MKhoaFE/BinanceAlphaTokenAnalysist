namespace AlphaBinanceAPI.Models;

public class TokenResponse
{
    public string Code { get; set; } = string.Empty;
    public string? Message { get; set; }
    public List<TokenData>? Data { get; set; }
    public bool Success { get; set; }
}

public class TokenData
{
    public string TokenId { get; set; } = string.Empty;
    public string ChainId { get; set; } = string.Empty;
    public string ChainName { get; set; } = string.Empty;
    public string ContractAddress { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
    public string PercentChange24h { get; set; } = string.Empty;
    public long ListingTime { get; set; }
    public int Score { get; set; }
    // Các trường khác nếu cần
}

public class TokenDisplay
{
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public int DaysRemaining { get; set; }
    public DateTime ListingDate { get; set; }
    public bool IsExpired { get; set; }
}
