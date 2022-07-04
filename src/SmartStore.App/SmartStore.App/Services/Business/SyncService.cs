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
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly ISaleDetailRepository _saleDetailRepository;

        public SyncService(
            ILastSyncRepository lastSyncRepository,
            IUnitRepository unitRepository,
            ICategoryRepository categoryRepository,
            ISupplierRepository supplierRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository,
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            ISaleRepository saleRepository,
            ISaleDetailRepository saleDetailRepository
            )
        {
            _lastSyncRepository = lastSyncRepository;
            _unitRepository = unitRepository;
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _categoryRepository = categoryRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _saleRepository = saleRepository;
            _saleDetailRepository = saleDetailRepository;
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
                _customerRepository.CreateTable(),
                _orderRepository.CreateTable(),
                _orderDetailRepository.CreateTable(),
                _saleRepository.CreateTable(),
                _saleDetailRepository.CreateTable()
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
            var upsert = await _lastSyncRepository.Upsert(lastSync);

            return upsert > 0;
        }

        private List<UnitEntity> Units =>
             new List<UnitEntity>
            {
                new UnitEntity
                {
                    Code = "H87",
                    Name =  "Pieza",
                    Description = "Unidad de conteo que define el número de piezas (pieza: un solo artículo, artículo o ejemplar)."
                },
                new UnitEntity
                {
                    Code = "KGM",
                    Name =  "Kilogramo",
                    Description = "Una unidad de masa igual a mil gramos."
                },
                new UnitEntity
                {
                    Code = "LTR",
                    Name =  "Litro",
                    Description = "Es una unidad de volumen equivalente a un decímetro cúbico (1 dm³). Su uso es aceptado en el Sistema Internacional de Unidades (SI), aunque ya no pertenece estrictamente a él."
                },
                new UnitEntity
                {
                    Code = "XBX",
                    Name =  "Caja",
                    Description = "Recipiente de diferentes materiales, tamaños y formas, generalmente con tapa, que sirve para guardar o transportar cosas."
                },
                new UnitEntity
                {
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
                    Code =  "001",
                    Name = "Abarrotes",
                    Description = "Abarrotes"
                },
                new CategoryEntity
                {
                    Code =  "002",
                    Name = "Enlatados",
                    Description = "Enlatados"
                },
                new CategoryEntity
                {
                    Code =  "003",
                    Name = "Lácteos",
                    Description = "Lácteos"
                },
                new CategoryEntity
                {
                    Code =  "004",
                    Name = "Botanas",
                    Description = "Botanas"
                },
                new CategoryEntity
                {
                    Code =  "005",
                    Name = "Confitería",
                    Description = "Confitería"
                },
                new CategoryEntity
                {
                    Code =  "006",
                    Name = "Harinas",
                    Description = "Harinas"
                },
                new CategoryEntity
                {
                    Code =  "007",
                    Name = "Bebidas",
                    Description = "Bebidas"
                },
                new CategoryEntity
                {
                    Code =  "008",
                    Name = "Bebidas Alcohólicas",
                    Description = "Bebidas Alcohólicas"
                },
                new CategoryEntity
                {
                    Code =  "009",
                    Name = "Alimentos Preparados",
                    Description = "Alimentos Preparados"
                },
                new CategoryEntity
                {
                    Code =  "010",
                    Name = "Carnes y Embudos",
                    Description = "Carnes y Embudos"
                },
                new CategoryEntity
                {
                    Code =  "011",
                    Name = "Automedicación",
                    Description = "Automedicación"
                },
                new CategoryEntity
                {
                    Code =  "012",
                    Name = "Higiene Personal",
                    Description = "Higiene Personal"
                },
                new CategoryEntity
                {
                    Code =  "013",
                    Name = "Uso Doméstico",
                    Description = "Uso Doméstico"
                },
                new CategoryEntity
                {
                    Code =  "014",
                    Name = "Helados",
                    Description = "Helados"
                },
                new CategoryEntity
                {
                    Code =  "015",
                    Name = "Jarcería / Productos de Limpieza",
                    Description = "Jarcería / Productos de Limpieza"
                },
                new CategoryEntity
                {
                    Code =  "016",
                    Name = "Otros",
                    Description = "Otros"
                }
            };

        private List<SupplierEntity> Suppliers =>
            new List<SupplierEntity>
            {
                new SupplierEntity
                {
                    Code = "001",
                    Name =  "Supplier 001",
                    Surname = "Surname 001",
                    Email = "supplier001@email.com",
                    PhoneNumber = "0123456789",
                    Address = "Address 001",
                    Reference = "Reference 001"
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
                        Code = $"{i}".PadLeft(5, '0'),
                        Name = $"Product {i}",
                        Description = $"Description {i}",
                        Cost = Math.Round(random.NextDouble() * (100 - i) + i, 2),
                        Price = Math.Round(random.NextDouble() * (100 - i) + i, 2),
                        MinStock = random.Next(1, 100 - i),
                        Stock = random.Next(1, 100 - i),
                        ImageUrl = images[random.Next(0, images.Length)],
                        IsFavorite = random.Next(i, 20) < 10
                        //SupplierId = random.Next(1, 10),
                        //CategoryId = random.Next(1, 10),
                        //UnitId = random.Next(1, 10),
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
                    Code = "001",
                    Name =  "Customer 001",
                    Surname = "Surname 001",
                    Email = "customer001@email.com",
                    PhoneNumber = "0123456789",
                    BirthDate = DateTime.Now,
                    DiscountRate = 100,
                    CreditRate = 100,
                    Address = "Address 001",
                    Reference = "Rerefence 001"
                }
            };
    }
}
