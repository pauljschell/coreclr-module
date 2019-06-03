using System.Collections.Generic;
using AltV.Net.NetworkingEntity.Elements.Entities;
using AltV.Net.NetworkingEntity.Elements.Factories;
using AltV.Net.NetworkingEntity.Elements.Providers;

namespace AltV.Net.NetworkingEntity.Elements.Pools
{
    public class NetworkingEntityPool : INetworkingEntityPool
    {
        private readonly IDictionary<ulong, INetworkingEntity> entities = new Dictionary<ulong, INetworkingEntity>();

        public IDictionary<ulong, INetworkingEntity> Entities => entities;

        private readonly IIdProvider<ulong> idProvider;

        private readonly INetworkingEntityFactory factory;

        public NetworkingEntityPool(IIdProvider<ulong> idProvider, INetworkingEntityFactory factory)
        {
            this.idProvider = idProvider;
            this.factory = factory;
        }

        public INetworkingEntity Create(IEntityStreamer entityStreamer, Entity.Entity streamedEntity)
        {
            streamedEntity.Id = idProvider.GetNext();
            var entity = factory.Create(entityStreamer, streamedEntity);
            Add(entity);
            return entity;
        }

        public bool Add(INetworkingEntity entity)
        {
            lock (entities)
            {
                return entities.TryAdd(entity.Id, entity);
            }
        }

        public void Remove(INetworkingEntity entity) => Remove(entity.Id);

        public void Remove(ulong id)
        {
            INetworkingEntity removedEntity;
            idProvider.Free(id);
            lock (entities)
            {
                if (!entities.Remove(id, out removedEntity))
                {
                    return;
                }
            }

            if (removedEntity is IInternalNetworkingEntity internalNetworkingEntity)
            {
                lock (internalNetworkingEntity)
                {
                    internalNetworkingEntity.Exists = false;
                }
            }
        }

        public bool TryGet(ulong id, out INetworkingEntity entity)
        {
            lock (entities)
            {
                return entities.TryGetValue(id, out entity);
            }
        }
    }
}