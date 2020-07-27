using Microsoft.EntityFrameworkCore;

namespace Web.Data
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }
    }
}