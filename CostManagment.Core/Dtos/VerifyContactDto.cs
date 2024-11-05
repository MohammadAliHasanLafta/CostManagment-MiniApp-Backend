using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Core.Dtos;

public class VerifyContactDto
{
    public long UserId { get; set; }
    public string? Mobile { get; set; }
    public string? ContactRequest { get; set; }
}
