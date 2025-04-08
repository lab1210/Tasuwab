//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TMFDomain.Entities.Staff;
//using TMFDomain.Interfaces.Staff;
//using TMFInfrastructure.Data;

//namespace TMFInfrastructure.Repositories
//{
//    public class ActivityRepository : Repository<Activity>, IActivityRepository
//    {
//        public ActivityRepository(ApplicationDbContext context) : base(context) { }

//        public async Task<IEnumerable<Activity>> GetByTypeTagAsync(string typeTag)
//        {
//            return await _dbSet.Where(a => a.type_tag == typeTag).ToListAsync();
//        }

//        public async Task<IEnumerable<Activity>> GetByPerformedByAsync(string performedBy)
//        {
//            return await _dbSet.Where(a => a.performedby == performedBy).ToListAsync();
//        }

//        public async Task<IEnumerable<Activity>> GetByDateRangeAsync(DateTime start, DateTime end)
//        {
//            return await _dbSet.Where(a => a.created_at >= start && a.created_at <= end).ToListAsync();
//        }
//    }
//}
