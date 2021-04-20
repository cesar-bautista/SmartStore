﻿using System.Threading.Tasks;

namespace SmartStore.App.Abstractions.Business
{
    public interface ISyncService
    {
        Task CreateObjects();

        Task DropObjects();

        Task Initialize();

        Task<bool> Sync();
    }
}