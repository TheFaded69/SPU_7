using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SPU_7.Database.Models;
using SPU_7.Database.Models.EntityTypes;

namespace SPU_7.Database.DbContext;

public sealed class DataContext : Microsoft.EntityFrameworkCore.DbContext, IDataContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
        Database.Migrate();
        //Database.EnsureDeleted();
    }

    private IDbContextTransaction _transaction;

    /// <summary>
    /// Начать транзакцию
    /// </summary>
    public async Task BeginTransactionAsync() => _transaction = await Database.BeginTransactionAsync();

    private void DetachAllEntities()
    {
        var changedEntriesCopy = ChangeTracker.Entries()
            .Where(e => e.State != EntityState.Detached)
            .ToArray();

        foreach (var entry in changedEntriesCopy)
        {
            entry.State = EntityState.Detached;
        }
    }

    /// <summary>
    /// Принять изменения транзакции, или сохранить изменения,
    /// если транзакция не была запущена
    /// </summary>
    public async Task CommitAsync()
    {
        try
        {
            await SaveChangesAsync();
            if (_transaction != null) await _transaction.CommitAsync();
        }
        finally
        {
            _transaction?.Dispose();
            DetachAllEntities();
        }
    }

    /// <summary>
    /// Принять изменения транзакции, или сохранить изменения,
    /// если транзакция не была запущена
    /// </summary>
    public void Commit()
    {
        try
        {
            SaveChanges();
            _transaction?.Commit();
        }
        finally
        {
            _transaction?.Dispose();
            DetachAllEntities();
        }
    }

    /// <summary>
    /// Откат транзакции
    /// </summary>
    public async Task RollbackAsync()
    {
        try
        {
            await _transaction.RollbackAsync();
            _transaction.Dispose();
            DetachAllEntities();
        }
        catch
        {
            // absorb all exceptions
        }
    }

    /// <summary>
    /// Создание моделей БД
    /// </summary>
    /// <param name="modelBuilder">билдер модели</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        CreateDbUsersModel(modelBuilder);
        CreateDbScriptsModel(modelBuilder);
        CreateDbOperationsModel(modelBuilder);
        CreateDbSettingsModel(modelBuilder);
        CreateDbCompletedScriptsModel(modelBuilder);
        CreateDbDevicesModel(modelBuilder);
        CreateDbCompletedOperationsModel(modelBuilder);
    }


    /// <summary>
    /// Создание основных свойств модели
    /// </summary>
    /// <param name="modelBuilder">билдер модели</param>
    /// <typeparam name="TEntityType">тип иодели БД</typeparam>
    /// <typeparam name="TKeyType">тип ключа модели БД</typeparam>
    private void CreateBaseEntity<TEntityType, TKeyType>(ModelBuilder modelBuilder) where TEntityType : DbEntity<TKeyType>
    {
        modelBuilder.Entity<TEntityType>().Property(p => p.Id).HasMaxLength(45);
        modelBuilder.Entity<TEntityType>().Property(e => e.CreateTime).HasDefaultValueSql("GETDATE()");
        modelBuilder.Entity<TEntityType>().Property(e => e.Deleted);
    }

    private void CreateDbUsersModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbUsers>().ToTable("Users");
        CreateBaseEntity<DbUsers, Guid>(modelBuilder);
        modelBuilder.Entity<DbUsers>().Property(e => e.UserName);
        modelBuilder.Entity<DbUsers>().Property(e => e.Password);
        modelBuilder.Entity<DbUsers>().Property(e => e.UserType);
        modelBuilder.Entity<DbUsers>().Property(e => e.Employee);
        modelBuilder.Entity<DbUsers>().Property(e => e.IsRememberUser);
        modelBuilder.Entity<DbUsers>().Property(e => e.ProgramType);

    }

    private void CreateDbScriptsModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbScripts>().ToTable("Scripts");
        CreateBaseEntity<DbScripts, Guid>(modelBuilder);
        modelBuilder.Entity<DbScripts>().Property(e => e.Name);
        modelBuilder.Entity<DbScripts>().Property(e => e.DeviceType);
        modelBuilder.Entity<DbScripts>().Property(e => e.TargetStandType);
        modelBuilder.Entity<DbScripts>().Property(e => e.Description);
        modelBuilder.Entity<DbScripts>().Property(e => e.LineNumber);
    }

    private void CreateDbOperationsModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbOperations>().ToTable("Operations");
        CreateBaseEntity<DbOperations, Guid>(modelBuilder);
        modelBuilder.Entity<DbOperations>().Property(e => e.Name);
        modelBuilder.Entity<DbOperations>().Property(e => e.Description);
        modelBuilder.Entity<DbOperations>().Property(e => e.OperationType);
        modelBuilder.Entity<DbOperations>().Property(e => e.Configuration);
        modelBuilder.Entity<DbOperations>()
            .HasOne(e => e.Script)
            .WithMany(e => e.Operations)
            .HasForeignKey(e => e.ScriptId);
        modelBuilder.Entity<DbOperations>().Property(e => e.Number);
    }
    
    private void CreateDbSettingsModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbStandSettings>().ToTable("StandSettings");
        CreateBaseEntity<DbStandSettings, Guid>(modelBuilder);
        modelBuilder.Entity<DbStandSettings>().Property(e => e.StandSettings);
        modelBuilder.Entity<DbStandSettings>().Property(e => e.StandNumber);
        modelBuilder.Entity<DbStandSettings>().Property(e => e.ProfileName);
    }
    
    private void CreateDbCompletedScriptsModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbScriptResult>().ToTable("CompletedScripts");
        CreateBaseEntity<DbScriptResult, Guid>(modelBuilder);
        modelBuilder.Entity<DbScriptResult>().Property(db => db.Name);
        modelBuilder.Entity<DbScriptResult>().Property(db => db.Description);
    }
    
    private void CreateDbCompletedOperationsModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbOperationResult>().ToTable("CompletedOperations");
        CreateBaseEntity<DbOperationResult, Guid>(modelBuilder);
        modelBuilder.Entity<DbOperationResult>()
            .HasOne(db => db.ScriptResult)
            .WithMany(db => db.OperationResults)
            .HasForeignKey(db => db.ScriptResultId);
        modelBuilder.Entity<DbOperationResult>().Property(db => db.Result);
    }
    
    private void CreateDbDevicesModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbDevice>().ToTable("Devices");
        CreateBaseEntity<DbDevice, Guid>(modelBuilder);
        modelBuilder.Entity<DbDevice>().Property(db => db.VendorNumber);
        modelBuilder.Entity<DbDevice>().Property(db => db.DeviceName)
            .HasMaxLength(256);
        modelBuilder.Entity<DbDevice>().Property(db => db.VendorName)
            .HasMaxLength(2000);
        modelBuilder.Entity<DbDevice>().Property(db => db.VendorAddress)
            .HasMaxLength(2000);
        modelBuilder.Entity<DbDevice>()
            .HasOne(db => db.OperationResult)
            .WithMany(db => db.Device)
            .HasForeignKey(db => db.OperationResultId);
    }
}