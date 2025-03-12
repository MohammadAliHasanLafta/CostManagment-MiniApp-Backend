
using CostManagment.Core.Dtos;
using CostManagment.Domain.Entities;
using CostManagment.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace CostManagment.API.Controllers;

[ApiController]
[Route("/")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITokenService _tokenService;
    string eitaa = "auth_date=1732951497&device_id=76bcb3d278daba5d09bba3df09fa5369&query_id=13295087637858127&user={\"id\":10921723,\"first_name\":\"\\u0645\\u062d\\u0645\\u062f \\u0632\\u0627\\u0631\\u0639\\u06cc \",\"last_name\":\"\",\"language_code\":\"en\"}&hash=2e623e8fc3ed9da0d529748f5a933060c03daf8538af8c70c1d6626e11ac8523";
    //private readonly IOtpService _otpService;
    private readonly IHttpClientFactory _httpClientFactory;

    public AccountController(IAccountRepository accountRepository, ITokenService tokenService, IHttpClientFactory httpClientFactory)
    {
        _accountRepository = accountRepository;
        _tokenService = tokenService;
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost("verify-initdata")]
    public async Task<IActionResult> ValidateEitaaInitData([FromBody] VerifyInitdataDto dto)
    {
        var initData = _accountRepository.ParseUrlEncodedData(dto.Initdata);
        var botToken = _accountRepository.GetBotToken();

        // 1️⃣ بررسی مقدار auth_date و منقضی شدن آن (۳ دقیقه)
        if (!initData.TryGetValue("auth_date", out string authDateStr) ||
            !long.TryParse(authDateStr, out long authDateUnix))
        {
            return BadRequest("Invalid 'auth_date' parameter.");
        }

        var authDateTime = DateTimeOffset.FromUnixTimeSeconds(authDateUnix).UtcDateTime;
        if (DateTime.UtcNow - authDateTime > TimeSpan.FromMinutes(3))
        {
            return Unauthorized("Session expired.");
        }

        // 2️⃣ بررسی مقدار hash و اعتبارسنجی آن
        if (!initData.TryGetValue("hash", out string receivedHash))
        {
            return BadRequest("Missing 'hash' parameter.");
        }

        initData.Remove("hash");
        var sortedData = initData.OrderBy(kvp => kvp.Key);
        var dataCheckString = string.Join("\n", sortedData.Select(kvp => $"{kvp.Key}={kvp.Value}"));

        var secretKey = _accountRepository.GenerateHmacSha256("WebAppData", botToken);
        var generatedHash = _accountRepository.GenerateHmacSha256(secretKey, dataCheckString);

        if (!CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(generatedHash), Encoding.UTF8.GetBytes(receivedHash)))
        {
            return Unauthorized("Invalid hash.");
        }

        // 3️⃣ ارسال درخواست به API ایتا برای بررسی اعتبار hash
        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync("https://eitaayar.ir/api/app/verify", new
        {
            token = botToken,
            hash = receivedHash,
            ip = dto.IpAddress
        });

        var responseData = await response.Content.ReadFromJsonAsync<EitaaVerificationResponse>();

        if (responseData == null || !responseData.Ok)
        {
            return Unauthorized("Hash verification failed.");
        }

        // 4️⃣ ذخیره اطلاعات کاربر در دیتابیس (در صورت تأیید اعتبار)
        var user = await _accountRepository.GetUserById(dto.UserId);
        if (user != null)
        {
            user.Initdata = dto.Initdata;
            user.IsValid = true;
            user.UpdatedAt = DateTime.Now;
            await _accountRepository.SaveChangesAsync();
        }
        else
        {
            user = new MiniAppUser(dto.UserId, dto.FirstName, dto.LastName, dto.Initdata, true);
            await _accountRepository.AddUserAsync(user);
        }

        var token = _tokenService.CreateToken(user, null);
        return Ok(new { Token = token });
    }

    [HttpPost("verify-contact")]
    public async Task<IActionResult> ValidateEitaaContact([FromBody] VerifyContactDto dto)
    {
        var initData = _accountRepository.ParseUrlEncodedData(dto.ContactRequest);
        var botToken = _accountRepository.GetBotToken();

        if (!initData.TryGetValue("hash", out string receivedHash))
        {
            return BadRequest("Missing 'hash' parameter.");
        }

        initData.Remove("hash");

        var sortedData = initData.OrderBy(kvp => kvp.Key);

        var dataCheckString = string.Join("\n", sortedData.Select(kvp => $"{kvp.Key}={kvp.Value}"));

        var secretKey = _accountRepository.GenerateHmacSha256("WebAppData", botToken);

        var generatedHash = _accountRepository.GenerateHmacSha256(secretKey, dataCheckString);

        if (CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(generatedHash), Encoding.UTF8.GetBytes(receivedHash)))
        {
            var user = await _accountRepository.GetContactById(dto.UserId);

            if (user != null)
            {
                user.ContactRequest = dto.ContactRequest;
                user.Mobile = dto.Mobile;
                user.IsValid = true;
                user.UpdatedAt = DateTime.Now;

                await _accountRepository.SaveChangesAsync();
            }
            else
            {
                user = new MiniAppUserContact(dto.UserId, dto.ContactRequest, dto.Mobile, true);

                await _accountRepository.AddContactAsync(user);
            }

            return Ok("success verify!");
        }

        return Unauthorized("Invalid data.");
    }

    [HttpPost("verify-initdata-Bahesabsho")]
    public async Task<IActionResult> ValidateEitaaInitData_Bahesabsho([FromBody] VerifyInitdataDto dto)
    {
        var initData = _accountRepository.ParseUrlEncodedData(dto.Initdata);
        var botToken = _accountRepository.GetBotToken_Bahesabsho();

        if (!initData.TryGetValue("hash", out string receivedHash))
        {
            return BadRequest("Missing 'hash' parameter.");
        }

        initData.Remove("hash");

        var sortedData = initData.OrderBy(kvp => kvp.Key);

        var dataCheckString = string.Join("\n", sortedData.Select(kvp => $"{kvp.Key}={kvp.Value}"));

        var secretKey = _accountRepository.GenerateHmacSha256("WebAppData", botToken);

        var generatedHash = _accountRepository.GenerateHmacSha256(secretKey, dataCheckString);

        if (CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(generatedHash), Encoding.UTF8.GetBytes(receivedHash)))
        {
            var user = await _accountRepository.GetUserById(dto.UserId);

            if (user != null)
            {
                user.Initdata = dto.Initdata;
                user.IsValid = true;
                user.UpdatedAt = DateTime.Now;

                await _accountRepository.SaveChangesAsync();
            }
            else
            {
                user = new MiniAppUser(dto.UserId, dto.FirstName, dto.LastName, dto.Initdata, true);

                await _accountRepository.AddUserAsync(user);
            }

            var token = _tokenService.CreateToken(user, null);
            return Ok(new { Token = token });
        }

        return Unauthorized("Invalid data.");
    }

    [HttpPost("verify-contact-Bahesabsho")]
    public async Task<IActionResult> ValidateEitaaContact_Bahesabsho([FromBody] VerifyContactDto dto)
    {
        var initData = _accountRepository.ParseUrlEncodedData(dto.ContactRequest);
        var botToken = _accountRepository.GetBotToken_Bahesabsho();

        if (!initData.TryGetValue("hash", out string receivedHash))
        {
            return BadRequest("Missing 'hash' parameter.");
        }

        initData.Remove("hash");

        var sortedData = initData.OrderBy(kvp => kvp.Key);

        var dataCheckString = string.Join("\n", sortedData.Select(kvp => $"{kvp.Key}={kvp.Value}"));

        var secretKey = _accountRepository.GenerateHmacSha256("WebAppData", botToken);

        var generatedHash = _accountRepository.GenerateHmacSha256(secretKey, dataCheckString);

        if (CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(generatedHash), Encoding.UTF8.GetBytes(receivedHash)))
        {
            var user = await _accountRepository.GetContactById(dto.UserId);

            if (user != null)
            {
                user.ContactRequest = dto.ContactRequest;
                user.Mobile = dto.Mobile;
                user.IsValid = true;
                user.UpdatedAt = DateTime.Now;

                await _accountRepository.SaveChangesAsync();
            }
            else
            {
                user = new MiniAppUserContact(dto.UserId, dto.ContactRequest, dto.Mobile, true);

                await _accountRepository.AddContactAsync(user);
            }

            return Ok("success verify!");
        }

        return Unauthorized("Invalid data.");
    }

    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOtp([FromBody] SendOtpDto dto)
    {
        //var otp = new Random().Next(100000, 999999).ToString();
        //var message = $"Your OTP code is: {otp}";

        //var result = await _otpService.SendOtpAsync(dto.PhoneNumber, message);

        //if (!result)
        //    return StatusCode(500, "Failed to send OTP.");
        var otp = "123456";

        await _accountRepository.SaveChangesInWebUsers(dto.PhoneNumber, otp);

        return Ok("OTP sent successfully.");
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
    {
        var user = _accountRepository.GetUserByNumber(dto.PhoneNumber);

        if (user == null)
            return Unauthorized("User not found.");

        if (user.Otp != dto.Otp)
            return Unauthorized($"Invalid OTP. Received: {dto.Otp}, Expected: {user.Otp}");

        var token = _tokenService.CreateToken(null, user);
        return Ok(new { Token = token });
    }

    [HttpGet("get-miniappuser-mobile")]
    public async Task<IActionResult> GetMiniAppUserMobile([FromQuery] long UserId)
    {
        var mobile = await _accountRepository.GetUserMobile(UserId);

        if (mobile == null)
            return Ok(false);

        return Ok(mobile);
    }

    public class EitaaVerificationResponse
    {
        public bool Ok { get; set; }
    }
}
