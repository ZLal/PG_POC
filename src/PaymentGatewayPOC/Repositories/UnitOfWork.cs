using Microsoft.EntityFrameworkCore.Storage;
using PaymentGatewayPOC.Data;
using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Repositories.Interfaces;

namespace PaymentGatewayPOC.Repositories;

/// <summary>
/// Unit of Work implementation to manage all repositories and database transactions
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly PaymentGatewayContext _context;
    private IDbContextTransaction? _transaction;

    private IRepository<Organization>? _organizationRepository;
    private IApplicationRepository? _applicationRepository;
    private IRepository<Client>? _clientRepository;
    private IRepository<Gateway>? _gatewayRepository;
    private IRepository<ApplicationGateway>? _applicationGatewayRepository;
    private IRepository<Transaction>? _transactionRepository;
    private IRepository<TransactionDetail>? _transactionDetailRepository;
    private IRepository<ErrorLog>? _errorLogRepository;

    public UnitOfWork(PaymentGatewayContext context)
    {
        _context = context;
    }

    public IRepository<Organization> Organizations =>
        _organizationRepository ??= new Repository<Organization>(_context);

    public IApplicationRepository Applications =>
        _applicationRepository ??= new ApplicationRepository(_context);

    public IRepository<Client> Clients =>
        _clientRepository ??= new Repository<Client>(_context);

    public IRepository<Gateway> Gateways =>
        _gatewayRepository ??= new Repository<Gateway>(_context);

    public IRepository<ApplicationGateway> ApplicationGateways =>
        _applicationGatewayRepository ??= new Repository<ApplicationGateway>(_context);

    public IRepository<Transaction> Transactions =>
        _transactionRepository ??= new Repository<Transaction>(_context);

    public IRepository<TransactionDetail> TransactionDetails =>
        _transactionDetailRepository ??= new Repository<TransactionDetail>(_context);

    public IRepository<ErrorLog> ErrorLogs =>
        _errorLogRepository ??= new Repository<ErrorLog>(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }
            _transaction = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
        }

        await _context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
