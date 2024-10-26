using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Domain.Interfaces;

public interface IOtpService
{
    Task<bool> SendOtpAsync(string phoneNumber, string message);
}
