using System;
using System.Threading.Tasks;
using SmartStore.App.Abstractions.Business;
using SmartStore.App.Abstractions.Data;
using SmartStore.App.Services.Data.Entities;

namespace SmartStore.App.Services.Business
{
    public class SyncService : ISyncService
    {
        private readonly ILastSyncRepository _lastSyncRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICategoryRepository _categoryRepository;

        public SyncService(
            ILastSyncRepository lastSyncRepository,
            IUnitRepository unitRepository,
            ISupplierRepository supplierRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository,
            ICategoryRepository categoryRepository
            )
        {
            _lastSyncRepository = lastSyncRepository;
            _unitRepository = unitRepository;
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _categoryRepository = categoryRepository;
        }

        public Task Initialize()
        {
            return Task.WhenAll
            (
                _lastSyncRepository.CreateTable(),
                _unitRepository.CreateTable(),
                _supplierRepository.CreateTable(),
                _productRepository.CreateTable(),
                _customerRepository.CreateTable(),
                _categoryRepository.CreateTable()
            );
        }

        public async Task<bool> Sync()
        {
            // Obtener registro de última actualización
            var lastSync = await _lastSyncRepository.Get(Guid.Empty) ?? new LastSyncEntity();

            // Sincronizar la lista de repositorios
            await Task.WhenAll
            (
                _unitRepository.Sync(lastSync.LastSync),
                _supplierRepository.Sync(lastSync.LastSync),
                _productRepository.Sync(lastSync.LastSync),
                _customerRepository.Sync(lastSync.LastSync),
                _categoryRepository.Sync(lastSync.LastSync)
            );

            // Actualizar registro de última actualización
            //lastSync.LastSync = DateTimeOffset.Now;
            //var upsert = await _lastSyncRepository.Upsert(lastSync);

            // Valor de retorno
            //return upsert > 0;
            return true;
        }
    }
}
