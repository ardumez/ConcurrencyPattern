using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ConcurrencyPattern
{
    public class ChildEntityRepository
    {
        public List<ChildEntity> GetChildEntities()
        {
            using (var context = new MyDbContext())
            {
                using (var tx = context.Database.BeginTransaction(IsolationLevel.Snapshot))
                {
                    var results = context.Set<ChildEntity>()
                        .Include(x => x.ParentAggregate)
                        .ToList();

                    return results;
                }
            }
        }

        public void Update(int entityId, int parentId, int rowVersionParent, decimal amount)
        {
            using (var context = new MyDbContext())
            {
                using (var tx = context.Database.BeginTransaction(IsolationLevel.Snapshot))
                {
                    // Check concistency
                    var parent = context.Set<ParentAggregate>().Single(x => x.ParentAggregateId == parentId);
                    if (parent.RowVersion != rowVersionParent)
                    {
                        throw new Exception("parent updated");
                    }

                    var childEntity = context.Set<ChildEntity>().Single(x => x.ChildEntityId == entityId);
                    childEntity.Amount = amount;

                    // Update parent
                    parent.LastModification = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }

        public void Insert(int parentId, ChildEntity childEntity, int rowVersionParent)
        {
            using (var context = new MyDbContext())
            {
                using (var tx = context.Database.BeginTransaction(IsolationLevel.Snapshot))
                {
                    // Check concistency
                    var parent = context.Set<ParentAggregate>().Single(x => x.ParentAggregateId == parentId);
                    if (parent.RowVersion != rowVersionParent)
                    {
                        throw new Exception("parent updated");
                    }

                    childEntity.ParentAggregate = parent;
                    context.Set<ChildEntity>().Add(childEntity);

                    // Update parent
                    parent.LastModification = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }
    }
}
