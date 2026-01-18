using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using AlphaBinanceAPI.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace AlphaBinanceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowReactApp")]
public class TokenController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<TokenController> _logger;

    public TokenController(IHttpClientFactory httpClientFactory, ILogger<TokenController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    [HttpGet("alpha-tokens")]
    public async Task<ActionResult<List<TokenDisplay>>> GetAlphaTokens()
    {
        try
        {
            var apiUrl = "https://www.binance.com/bapi/defi/v1/public/wallet-direct/buw/wallet/cex/alpha/all/token/list";
            
            var httpClient = _httpClientFactory.CreateClient("BinanceApi");
            var response = await httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (tokenResponse?.Data == null)
            {
                return Ok(new List<TokenDisplay>());
            }

            // Múi giờ Việt Nam (UTC+7)
            TimeZoneInfo vietnamTimeZone;
            try
            {
                // Thử dùng timezone ID cho Windows
                vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            }
            catch
            {
                // Nếu không tìm thấy, tạo timezone UTC+7
                vietnamTimeZone = TimeZoneInfo.CreateCustomTimeZone(
                    "Vietnam Standard Time",
                    TimeSpan.FromHours(7),
                    "Vietnam Standard Time",
                    "Vietnam Standard Time");
            }
            var nowVietnam = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);

            var allTokens = tokenResponse.Data
                .Where(token => token.ListingTime > 0)
                .Select(token =>
                {
                    var listingDate = DateTimeOffset.FromUnixTimeMilliseconds(token.ListingTime).DateTime;
                    listingDate = TimeZoneInfo.ConvertTimeFromUtc(listingDate.ToUniversalTime(), vietnamTimeZone);
                    
                    var daysSinceListing = (nowVietnam - listingDate).Days;
                    var daysRemaining = 30 - daysSinceListing;

                    return new TokenDisplay
                    {
                        Name = token.Name,
                        Symbol = token.Symbol,
                        DaysRemaining = daysRemaining,
                        ListingDate = listingDate,
                        IsExpired = daysRemaining <= 0
                    };
                })
                .Where(x => (x.DaysRemaining > 0 && x.DaysRemaining <= 30) || (x.DaysRemaining <= 0 && x.DaysRemaining >= -30))
                .OrderByDescending(x => x.DaysRemaining) // Sắp xếp theo số ngày còn lại giảm dần
                .ToList();

            return Ok(allTokens);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching alpha tokens");
            return StatusCode(500, new { error = "Internal server error", message = ex.Message });
        }
    }
}
