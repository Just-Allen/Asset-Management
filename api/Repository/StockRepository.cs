using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock existingStock)
        {
            await _context.Stocks.AddAsync(existingStock);
            await _context.SaveChangesAsync();
            
            return existingStock;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if(existingStock == null)
            {
                return null;
            }

            _context.Remove(existingStock);
            await _context.SaveChangesAsync();

            return existingStock;
        }

        public async Task<Stock?> GeByIdAsnyc(int id)
        {
            return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks.Include(c => c.Comments).ToListAsync();
        }

        public Task<bool> StockExists(int id)
        {
            return _context.Stocks.AnyAsync(s => s.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if(existingStock == null)
            {
                return null;
            }

            existingStock.Symbol = stockDto.Symbol;
            existingStock.CompanyName = stockDto.CompanyName;
            existingStock.Purchase = stockDto.Purchase;
            existingStock.LastDiv = stockDto.LastDiv;
            existingStock.Industry = stockDto.Industry;
            existingStock.MarketCap = stockDto.MarketCap;
            
            await _context.SaveChangesAsync();

            return existingStock;
        }
    }
}