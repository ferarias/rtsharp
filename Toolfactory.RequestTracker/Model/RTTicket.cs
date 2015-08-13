using System;
using System.Collections.Generic;

namespace Toolfactory.RequestTracker.Model
{
    public sealed class RtTicket : IRtCustomFieldObject
    {
        public long? Id { get; set; }
        public string Queue { get; set; }
        public string Owner { get; set; }
        public string Creator { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }
        public long? Priority { get; set; }
        public long? InitialPriority { get; set; }
        public long? FinalPriority { get; set; }
        public string Requestors { get; set; }
        public string Cc { get; set; }
        public string AdminCc { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Starts { get; set; }
        public DateTime? Started { get; set; }
        public DateTime? Due { get; set; }
        public DateTime? Resolved { get; set; }
        public DateTime? Told { get; set; }
        public DateTime? LastUpdated { get; set; }
        public long? TimeEstimated { get; set; }
        public long? TimeWorked { get; set; }
        public long? TimeLeft { get; set; }
        public IDictionary<string, RtCustomField> CustomFields { get; set; }

        public RtTicket()
        {
            CustomFields = new Dictionary<string, RtCustomField>();
        }

        public override string ToString()
        {
            return "RTTicket [id=" + Id + ", subject=" + Subject + ", timeWorked=" + TimeWorked + ", customFields=" + CustomFields + "]";
        }
    }
}