using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kajiApp_blazor.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace kajiapp.Infra.Repository
{
    // Infra/Repository/WorkRepository.cs
    public class WorkRepository : IWorkRepository
    {
        private readonly kajiappDBContext _context;

        public WorkRepository(kajiappDBContext context)
        {
            _context = context;
        }

        public async Task<List<Work>> GetWorksOfPreviousDayAsync()
        {
            var yesterday = DateOnly.FromDateTime(
                TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Tokyo Standard Time").AddDays(-1)
            );

            return await _context.Works
                .Where(w => w.Day == yesterday)
                .OrderBy(w => w.Name)
                .ToListAsync();
        }
    }

}
