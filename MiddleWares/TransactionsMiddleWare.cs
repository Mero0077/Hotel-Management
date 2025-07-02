
using Hotel_Management.Data;

namespace Hotel_Management.MiddleWares
{
    public class TransactionsMiddleWare : IMiddleware
    {
        private readonly ApplicationDbContext _context;
        public TransactionsMiddleWare(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                await next(context);
                await _context.Database.CommitTransactionAsync();
            }

            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
            }

        }
    }
}
