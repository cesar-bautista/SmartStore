using System;
using System.Collections.Generic;
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
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;

        public SyncService(
            ILastSyncRepository lastSyncRepository,
            IUnitRepository unitRepository,
            ICategoryRepository categoryRepository,
            ISupplierRepository supplierRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository
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
                _categoryRepository.CreateTable(),
                _supplierRepository.CreateTable(),
                _productRepository.CreateTable(),
                _customerRepository.CreateTable()
               );
        }

        public async Task<bool> Sync()
        {
            // Obtener registro de última actualización
            var lastSync = await _lastSyncRepository.Get(Guid.Empty) ?? new LastSyncEntity();

            try
            {
                // Sincronizar la lista de repositorios
                await Task.WhenAll
                    (
                        _unitRepository.Sync(lastSync.LastSync),
                        _supplierRepository.Sync(lastSync.LastSync),
                        _productRepository.Sync(lastSync.LastSync),
                        _customerRepository.Sync(lastSync.LastSync),
                        _categoryRepository.Sync(lastSync.LastSync)
                    );
            }
            catch
            {
                // HACK: Modo Mock
                if (lastSync.LastSync == DateTimeOffset.MinValue)
                {
                    await _unitRepository.Insert(Units);
                    await _categoryRepository.Insert(Categories);
                    await _supplierRepository.Insert(Suppliers);
                    await _productRepository.Insert(Products);
                    await _customerRepository.Insert(Customers);
                }
            }
            // Actualizar registro de última actualización
            lastSync.LastSync = DateTimeOffset.Now;
            var upsert = await _lastSyncRepository.Upsert(lastSync);

            return upsert > 0;
        }

        private List<UnitEntity> Units =>
             new List<UnitEntity>
            {
                new UnitEntity
                {
                    Id = Guid.NewGuid(),
                    Code = "H87",
                    Name =  "Pieza",
                    Description = "Unidad de conteo que define el número de piezas (pieza: un solo artículo, artículo o ejemplar)."
                },
                new UnitEntity
                {
                    Id = Guid.NewGuid(),
                    Code = "KGM",
                    Name =  "Kilogramo",
                    Description = "Una unidad de masa igual a mil gramos."
                },
                new UnitEntity
                {
                    Id = Guid.NewGuid(),
                    Code = "LTR",
                    Name =  "Litro",
                    Description = "Es una unidad de volumen equivalente a un decímetro cúbico (1 dm³). Su uso es aceptado en el Sistema Internacional de Unidades (SI), aunque ya no pertenece estrictamente a él."
                },
                new UnitEntity
                {
                    Id = Guid.NewGuid(),
                    Code = "XBX",
                    Name =  "Caja",
                    Description = "Recipiente de diferentes materiales, tamaños y formas, generalmente con tapa, que sirve para guardar o transportar cosas."
                },
                new UnitEntity
                {
                    Id = Guid.NewGuid(),
                    Code = "XPK",
                    Name =  "Paquete",
                    Description = "Unidad de empaque estándar."
                }
            };

        private List<CategoryEntity> Categories =>
            new List<CategoryEntity>
            {
                new CategoryEntity
                {
                    Id = Guid.NewGuid(),
                    Code =  "001",
                    Name = "ABARROTES",
                    Description = "ABARROTES"
                },
                new CategoryEntity
                {
                    Id = Guid.NewGuid(),
                    Code =  "002",
                    Name = "ENLATADOS",
                    Description = "ENLATADOS"
                },
                new CategoryEntity
                {
                    Id = Guid.NewGuid(),
                    Code =  "003",
                    Name = "LÁCTEOS",
                    Description = "LÁCTEOS"
                },
                new CategoryEntity
                {
                    Id = Guid.NewGuid(),
                    Code =  "004",
                    Name = "BOTANAS",
                    Description = "BOTANAS"
                },
                new CategoryEntity
                {
                    Id = Guid.NewGuid(),
                    Code =  "005",
                    Name = "CONFITERÍA",
                    Description = "CONFITERÍA"
                },
                new CategoryEntity
                {
                    Id = Guid.NewGuid(),
                    Code =  "006",
                    Name = "HARINAS",
                    Description = "HARINAS"
                },
                new CategoryEntity
                {
                    Id = Guid.NewGuid(),
                    Code =  "007",
                    Name = "BEBIDAS",
                    Description = "BEBIDAS"
                },
                new CategoryEntity
                {
                    Id = Guid.NewGuid(),
                    Code =  "008",
                    Name = "BEBIDAS ALCOHÓLICAS",
                    Description = "BEBIDAS ALCOHÓLICAS"
                },
                new CategoryEntity
                {
                    Id = Guid.NewGuid(),
                    Code =  "009",
                    Name = "ALIMENTOS PREPARADOS",
                    Description = "ALIMENTOS PREPARADOS"
                },
                new CategoryEntity
                {
                    Id = Guid.NewGuid(),
                    Code =  "010",
                    Name = "Carnes y Embudos",
                    Description = "Carnes y Embudos"
                },
                new CategoryEntity
                {
                    Id = Guid.NewGuid(),
                    Code =  "011",
                    Name = "AUTOMEDICACIÓN",
                    Description = "AUTOMEDICACIÓN"
                },
                new CategoryEntity
                {
                    Id = Guid.NewGuid(),
                    Code =  "012",
                    Name = "HIGIENE PERSONAL",
                    Description = "HIGIENE PERSONAL"
                },
                new CategoryEntity
                {
                    Id = Guid.NewGuid(),
                    Code =  "013",
                    Name = "USO DOMESTICO",
                    Description = "USO DOMESTICO"
                },
                new CategoryEntity
                {
                    Id = Guid.NewGuid(),
                    Code =  "014",
                    Name = "HELADOS",
                    Description = "HELADOS"
                },
                new CategoryEntity
                {
                    Id = Guid.NewGuid(),
                    Code =  "015",
                    Name = "JARCERIA / PRODUCTOS DE LIMPIEZA",
                    Description = "JARCERIA / PRODUCTOS DE LIMPIEZA"
                },
                new CategoryEntity
                {
                    Id = Guid.NewGuid(),
                    Code =  "016",
                    Name = "OTROS",
                    Description = "OTROS"
                }
            };

        private List<SupplierEntity> Suppliers =>
            new List<SupplierEntity>
            {
                new SupplierEntity
                {
                    Id = Guid.NewGuid(),
                    Code = "001",
                    Name =  "Supplier 001",
                    Description = "Supplier 001",
                    Email = "supplier001@email.com",
                    PhoneNumber = "0123456789",
                    Address = "Address 001"
                }
            };

        private List<ProductEntity> Products
        {
            get
            {
                var images = new[]
                {
                    "https://cdn.pixabay.com/photo/2019/06/12/07/12/popcorn-4268489_960_720.jpg",
                    "https://cdn.pixabay.com/photo/2015/01/08/04/16/box-592366_960_720.jpg",
                    "https://cdn.pixabay.com/photo/2020/05/10/05/14/pepsi-5152332_960_720.jpg",
                    "https://media.istockphoto.com/photos/doritos-on-white-picture-id458670023",
                    "https://cdn.pixabay.com/photo/2018/02/26/16/30/eggs-3183410_960_720.jpg",
                    "https://cdn.pixabay.com/photo/2016/06/10/15/15/tomatoes-1448262_960_720.jpg"
                };

                var list = new List<ProductEntity>();
                var random = new Random();
                for (var i = 1; i <= 20; i++)
                {
                    list.Add(new ProductEntity
                    {
                        Id = Guid.NewGuid(),
                        ImageUrl = images[random.Next(0, images.Length)],
                        Price = random.NextDouble() * (100 - i) + i,
                        Name = $"Product {i}",
                        Description = $"Description {i}",
                        Code = $"{i}".PadLeft(5, '0'),
                        Cost = random.NextDouble() * (100 - i) + i,
                        MinStock = random.Next(1, 100 - i),
                        Stock = random.Next(1, 100 - i),
                        //SupplierId = random.Next(1, 10),
                        //CategoryId = random.Next(1, 10),
                        //UnitId = random.Next(1, 10),
                        IsFavorite = random.Next(i, 20) < 10
                    });
                }

                return list;
            }
        }

        private List<CustomerEntity> Customers =>
            new List<CustomerEntity>
            {
                new CustomerEntity
                {
                    Id = Guid.NewGuid(),
                    Code = "001",
                    Name =  "Customer 001",
                    Description = "Customer 001",
                    Email = "customer001@email.com",
                    PhoneNumber = "0123456789",
                    BirthDate = DateTime.Now,
                    DiscountRate = 100,
                    Address = "Address 001",
                }
            };
    }
}
