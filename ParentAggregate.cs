using System;

namespace ConcurrencyPattern
{
    public class ParentAggregate
    {
        public int ParentAggregateId { get; set; }
        public int RowVersion { get; set; }
        public DateTime LastModification { get; set; }
    }
}
