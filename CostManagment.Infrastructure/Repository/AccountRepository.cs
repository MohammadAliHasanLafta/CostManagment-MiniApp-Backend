using CostManagment.Domain.Entities;
using CostManagment.Domain.Interfaces;
using CostManagment.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CostManagment.Infrastructure.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;
    private readonly string _eitaaToken = "60315926:,g8kEFWB1-Y)c(FzVhp-QJB9CKwaW-mD(KG[PyM-m@GJId[$x-t*CurXQ?d-jUseEjgf0-@8Yv/H1Hn-Xgef3Nq9r-6T94&lhpa-5Bbxh3q%O-!KIlELG8Y-a{AzSPzNp-1P2U06^xJ-L6lXja24v-CAVQAx]UR-^#EHKW]vq-soEDS.h}R-3L2iRuxnc-WqMs~pGr";
    private readonly string _eitaaToken_Bahesabsho = "61471346:YXMocjc-@13cBKs-TBW*va1-/3jyme6-dxx7i#f-l&$seMR-!uvl*0n-tEr9nwJ-zTsH8F)-~fyAp7A-5aSHX1.-QtD{NO9-ww)MK4W-xPdP2cQ-oqe$Ik0-lW4ReFS-ShZVGP8-3O8CgVh-b0fS[4y-hQHRVxX-TH7vBgR-^iU6JuN-FrKfggb-6t";

    public AccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public string GetBotToken()
    {
        return _eitaaToken;
    }

    public string GetBotToken_Bahesabsho()
    {
        return _eitaaToken_Bahesabsho;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task AddUserAsync(MiniAppUser user)
    {
        await _context.MiniAppUsers.AddAsync(user);
        await SaveChangesAsync();
    }

    public async Task SaveChangesInWebUsers(string phoneNumber, string otp)
    {
        var user = GetUserByNumber(phoneNumber);
        if (user == null)
        {
            user = new WebAppUser(phoneNumber, otp);
            _context.WebAppUsers.AddAsync(user);
            _context.SaveChangesAsync();
        }
        else
        {
            user.Otp = otp;
            user.UpdatedAt = DateTime.Now;
            _context.SaveChangesAsync();
        }
    }

    public async Task<MiniAppUser> GetUserById(long userId)
    {
        return await _context.MiniAppUsers
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public WebAppUser GetUserByNumber(string phoneNumber)
    {
        return _context.WebAppUsers.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
    }

    public byte[] GenerateHmacSha256(string key, string message)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
        {
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
        }
    }

    public string GenerateHmacSha256(byte[] key, string message)
    {
        using (var hmac = new HMACSHA256(key))
        {
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }

    public Dictionary<string, string> ParseUrlEncodedData(string encodedData)
    {
        var query = HttpUtility.ParseQueryString(encodedData);
        return query.AllKeys.ToDictionary(key => key, key => query[key]);
    }

    public async Task AddContactAsync(MiniAppUserContact contact)
    {
        await _context.Contacts.AddAsync(contact);
        await SaveChangesAsync();
    }

    public async Task<MiniAppUserContact> GetContactById(long userId)
    {
        return _context.Contacts.AsNoTracking().FirstOrDefault(u => u.UserId == userId);
    }

    public async Task<string> GetUserMobile(long userId)
    {
        var user = await _context.Contacts.FirstOrDefaultAsync(c => c.UserId == userId);

        if (user == null)
            return null;

        return user.Mobile;
    }
}
