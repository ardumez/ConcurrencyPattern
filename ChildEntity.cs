using System;

namespace ConcurrencyPattern
{
    public class ChildEntity
    {
        public int ChildEntityId { get; set; }
        public DateTime ValidityDate { get; set; }
        public ParentAggregate ParentAggregate { get; set; }
        public decimal Amount { get; set; }
    }
}
