using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostManagment.Domain.Common;

public class EntityBase
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public EntityBase()
    {
        CreatedAt = DateTime.Now;
    }

    public void Delete()
    {
        this.IsDeleted = true;
    }
}
