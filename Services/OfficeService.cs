using Microsoft.EntityFrameworkCore;
using AssetTrackingSystem.Data;
using AssetTrackingSystem.Models;

namespace AssetTrackingSystem.Services;

public class OfficeService
{
    public List<Office> GetAllOffices()
    {
        using AppDbContext dbContext = new();
        return dbContext.Offices.OrderBy(office => office.Name).ToList();
    }

    public Office? GetOfficeById(int id)
    {
        using AppDbContext dbContext = new();
        return dbContext.Offices.AsNoTracking().FirstOrDefault(office => office.Id == id);
    }
}
