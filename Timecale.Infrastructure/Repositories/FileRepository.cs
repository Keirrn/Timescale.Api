using Microsoft.EntityFrameworkCore;
using Timecale.Application.Interfaces;
using Timecale.Domain.Entities;
using Timecale.Infrastructure.Data;
using Timecale.Application.DTOs;

namespace Timecale.Infrastructure.Repositories;

public class FileRepository : IFileRepository
{
    private readonly ApplicationDbContext _context;

    public FileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UploadedFile?> GetByFileNameAsync(
        string fileName,
        CancellationToken cancellationToken)
    {
        return await _context.UploadedFiles
            .Include(x => x.Values)
            .Include(x => x.Result)
            .FirstOrDefaultAsync(
                x => x.FileName == fileName,
                cancellationToken);
    }

    public async Task AddAsync(
        UploadedFile file,
        CancellationToken cancellationToken)
    {
        await _context.UploadedFiles.AddAsync(file, cancellationToken);
    }

    public Task DeleteAsync(
        UploadedFile file,
        CancellationToken cancellationToken)
    {
        _context.UploadedFiles.Remove(file);

        return Task.CompletedTask;
    }
    public async Task<IReadOnlyList<ResultDTO>> GetResultsAsync(
        ResultFilterDTO filter,
        CancellationToken cancellationToken)
    {
  
        var query = _context.Results
            .AsNoTracking()
            .AsQueryable();
        if (!string.IsNullOrWhiteSpace(filter.FileName))
        {
            query = query.Where(x =>
                x.File.FileName == filter.FileName);
        }

        if (filter.FirstOperationDateFrom.HasValue)
        {
            query = query.Where(x =>
                x.FirstOperationDate >= filter.FirstOperationDateFrom.Value);
        }

        if (filter.FirstOperationDateTo.HasValue)
        {
            query = query.Where(x =>
                x.FirstOperationDate <= filter.FirstOperationDateTo.Value);
        }

        if (filter.AverageValueFrom.HasValue)
        {
            query = query.Where(x =>
                x.AverageValue >= filter.AverageValueFrom.Value);
        }

        if (filter.AverageValueTo.HasValue)
        {
            query = query.Where(x =>
                x.AverageValue <= filter.AverageValueTo.Value);
        }

        if (filter.AverageExecutionTimeFrom.HasValue)
        {
            query = query.Where(x =>
                x.AverageExecutionTime >= filter.AverageExecutionTimeFrom.Value);
        }

        if (filter.AverageExecutionTimeTo.HasValue)
        {
            query = query.Where(x =>
                x.AverageExecutionTime <= filter.AverageExecutionTimeTo.Value);
        }

        return await query
                
            .OrderBy(x => x.FirstOperationDate)
            .Select(x => new ResultDTO
            {
                FileName = x.File.FileName,
                TimeDelta = x.TimeDelta,
                FirstOperationDate = x.FirstOperationDate,
                AverageExecutionTime = x.AverageExecutionTime,
                AverageValue = x.AverageValue,
                MedianValue = x.MedianValue,
                MaxValue = x.MaxValue,
                MinValue = x.MinValue
            })
            .ToListAsync(cancellationToken);
    }
    public async Task<IReadOnlyList<ValueResultDTO>> GetLastValuesAsync(
        string fileName,
        CancellationToken cancellationToken)
    {
        return await _context.Values
            .AsNoTracking()
            .Where(x => x.File.FileName == fileName)
            .OrderByDescending(x => x.Date)
            .Take(10)
            .Select(x => new ValueResultDTO
            {
                Date = x.Date,
                ExecutionTime = x.ExecutionTime,
                Value = x.MeasurementValue
            })
            .ToListAsync(cancellationToken);
    }
    public async Task<bool> ExistsByFileNameAsync(
        string fileName,
        CancellationToken cancellationToken)
    {
        return await _context.UploadedFiles
            .AnyAsync(
                x => x.FileName == fileName,
                cancellationToken);
    }
}