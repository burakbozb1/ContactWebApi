﻿using ContactWeb.Core.Repositories;
using ContactWeb.Core.Services;
using ContactWeb.Core.UnitOfWorks;
using ContactWeb.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ContactWeb.Service.Services
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        protected readonly IUnitOfWork _unitOfWork;

        public Service(IGenericRepository<T> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<T> AddAsync(T Entity)
        {
            await _repository.AddAsync(Entity);
            await _unitOfWork.CommitAsync();
            return Entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            return entities;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _repository.AnyAsync(expression);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAll().ToListAsync();
        }

        public async Task<T> GetByIdAsync(long id)
        {
            var hasProduct = await _repository.GetByIdAsync(id);
            if (hasProduct == null)
            {
                throw new NotFoundException($"{typeof(T).Name} ({id}) not found");
            }
            return hasProduct;
        }

        public async Task RemoveAsync(T Entity)
        {
            _repository.Remove(Entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entites)
        {
            _repository.RemoveRange(entites);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(T Entity)
        {
            _repository.Update(Entity);
            await _unitOfWork.CommitAsync();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _repository.Where(expression);
        }
    }
}
