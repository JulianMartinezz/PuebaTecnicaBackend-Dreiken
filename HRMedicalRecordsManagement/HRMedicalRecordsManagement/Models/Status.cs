using System;
using System.Collections.Generic;

namespace HRMedicalRecordsManagement.Models;

public partial class Status
{
    public int StatusId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TMedicalRecord> TMedicalRecords { get; set; } = new List<TMedicalRecord>();
}
