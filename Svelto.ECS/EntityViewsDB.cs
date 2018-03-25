using System;
using System.Collections.Generic;
using Svelto.DataStructures;

namespace Svelto.ECS.Internal
{
    class EntityViewsDB : IEntityViewsDB
    {
        internal EntityViewsDB(  Dictionary<Type, ITypeSafeList> entityViewsDB,
                                 Dictionary<Type, ITypeSafeList> metaEntityViewsDB,
                                Dictionary<Type, ITypeSafeDictionary> entityViewsDBdic,
                                 Dictionary<Type, ITypeSafeDictionary> metaEntityViewsDBdic,
                                Dictionary<int, Dictionary<Type, ITypeSafeList>>  groupEntityViewsDB)
        {
            _entityViewsDB = entityViewsDB;
            _metaEntityViewsDB = metaEntityViewsDB;
            
            _entityViewsDBdic = entityViewsDBdic;
            _metaEntityViewsDBdic = metaEntityViewsDBdic;
            
            _groupEntityViewsDB = groupEntityViewsDB;
            _mappedID = new Dictionary<int, int>();
            _metaMappedID = new Dictionary<int, int>();
        }

        public FasterReadOnlyList<T> QueryEntityViews<T>() where T:EntityView
        {
            var type = typeof(T);

            ITypeSafeList entityViews;

            if (_entityViewsDB.TryGetValue(type, out entityViews) == false)
                return RetrieveEmptyEntityViewList<T>();

            return new FasterReadOnlyList<T>((FasterList<T>)entityViews);
        }

        public FasterReadOnlyList<T> QueryGroupedEntityViews<T>(int @group) where T:EntityView
        {
            Dictionary<Type, ITypeSafeList> entitiesInGroupPerType;

            if (_groupEntityViewsDB.TryGetValue(group, out entitiesInGroupPerType) == false)
                return RetrieveEmptyEntityViewList<T>();

            ITypeSafeList outList;
            if (entitiesInGroupPerType.TryGetValue(typeof(T), out outList) == false)
                return RetrieveEmptyEntityViewList<T>();
            
            return new FasterReadOnlyList<T>((FasterList<T>) outList);
        }

        public T[] QueryEntityViewsAsArray<T>(out int count) where T : IEntityView
        {
            var type = typeof(T);
            count = 0;
            
            ITypeSafeList entityViews;

            if (_entityViewsDB.TryGetValue(type, out entityViews) == false)
                return RetrieveEmptyEntityViewArray<T>();
            
            return FasterList<T>.NoVirt.ToArrayFast((FasterList<T>)entityViews, out count);
        }
        
        public T[] QueryGroupedEntityViewsAsArray<T>(int @group, out int count) where T : IEntityView
        {
            var type = typeof(T);
            count = 0;
            
            Dictionary<Type, ITypeSafeList> entitiesInGroupPerType;
            
            if (_groupEntityViewsDB.TryGetValue(group, out entitiesInGroupPerType) == false)
                return RetrieveEmptyEntityViewArray<T>();
            
            ITypeSafeList outList;
            if (entitiesInGroupPerType.TryGetValue(typeof(T), out outList) == false)
                return RetrieveEmptyEntityViewArray<T>();
                       
            return FasterList<T>.NoVirt.ToArrayFast((FasterList<T>)entitiesInGroupPerType[type], out count);
        }

        public ReadOnlyDictionary<int, T> QueryIndexableEntityViews<T>() where T:EntityView
        {
            var type = typeof(T);

            ITypeSafeDictionary entityViews;

            if (_entityViewsDBdic.TryGetValue(type, out entityViews) == false)
                return TypeSafeDictionary<T>.Default;

            return new ReadOnlyDictionary<int, T>(entityViews as Dictionary<int, T>);
        }
        
        public ReadOnlyDictionary<int, T> QueryIndexableMetaEntityViews<T>() where T:EntityView
        {
            var type = typeof(T);

            ITypeSafeDictionary entityViews;

            if (_metaEntityViewsDBdic.TryGetValue(type, out entityViews) == false)
                return TypeSafeDictionary<T>.Default;

            return new ReadOnlyDictionary<int, T>(entityViews as Dictionary<int, T>);
        }
        
        public T QueryEntityView<T>(int entityID) where T:EntityView
        {
            return QueryEntityView<T>(_mappedID[entityID], _entityViewsDBdic);
        }

        public bool TryQueryEntityView<T>(int entityID, out T entityView) where T:EntityView
        {
            return TryQueryEntityView(_mappedID[entityID], _entityViewsDBdic, out entityView);
        }

        public T QueryMetaEntityView<T>(int metaEntityID) where T:EntityView
        {
            return QueryEntityView<T>(_metaMappedID[metaEntityID], _metaEntityViewsDBdic);
        }

        public bool TryQueryMetaEntityView<T>(int metaEntityID, out T entityView) where T:EntityView
        {
            return TryQueryEntityView(_metaMappedID[metaEntityID], _metaEntityViewsDBdic, out entityView);
        }

        public FasterReadOnlyList<T> QueryMetaEntityViews<T>() where T:EntityView
        {
            var type = typeof(T);

            ITypeSafeList entityViews;

            if (_metaEntityViewsDB.TryGetValue(type, out entityViews) == false)
                return RetrieveEmptyEntityViewList<T>();

            return new FasterReadOnlyList<T>((FasterList<T>)entityViews);
        }

        static FasterReadOnlyList<T> RetrieveEmptyEntityViewList<T>()
        {
            return FasterReadOnlyList<T>.DefaultList;
        }

        static T[] RetrieveEmptyEntityViewArray<T>()
        {
            return FasterList<T>.DefaultList.ToArrayFast();
        }
        
        static bool TryQueryEntityView<T>(int ID, Dictionary<Type, ITypeSafeDictionary> entityDic, out T entityView) where T : EntityView
        {
            var type = typeof(T);

            T internalEntityView;

            ITypeSafeDictionary   entityViews;
            TypeSafeDictionary<T> casted;

            entityDic.TryGetValue(type, out entityViews);
            casted = entityViews as TypeSafeDictionary<T>;

            if (casted != null &&
                casted.TryGetValue(ID, out internalEntityView))
            {
                entityView = internalEntityView;

                return true;
            }

            entityView = default(T);

            return false;
        }

        static T QueryEntityView<T>(int ID, Dictionary<Type, ITypeSafeDictionary> entityDic) where T : EntityView
        {
            var type = typeof(T);

            T                     internalEntityView; ITypeSafeDictionary entityViews;
            TypeSafeDictionary<T> casted;

            entityDic.TryGetValue(type, out entityViews);
            casted = entityViews as TypeSafeDictionary<T>;

            if (casted != null &&
                casted.TryGetValue(ID, out internalEntityView))
                return (T)internalEntityView;

            throw new Exception("EntityView Not Found");
        }

        readonly Dictionary<Type, ITypeSafeList>         _entityViewsDB;
        readonly Dictionary<Type, ITypeSafeList>         _metaEntityViewsDB;
        
        readonly Dictionary<Type, ITypeSafeDictionary>   _entityViewsDBdic;
        readonly Dictionary<Type, ITypeSafeDictionary>   _metaEntityViewsDBdic;
        
        readonly Dictionary<int, Dictionary<Type, ITypeSafeList>> _groupEntityViewsDB;
        
        int                  _uniqueID;
        int                  _metaUniqueID;
        readonly Dictionary<int, int> _mappedID;
        readonly Dictionary<int, int> _metaMappedID;

        public int GetMappedID(int entityId)
        {
            int realId;
            if (_mappedID.TryGetValue(entityId, out realId) == false)
            {
                realId = GetUniqueID();
                _mappedID[entityId] = realId;
            }

            return realId;
        }

        public int GetUniqueID()
        {
            ++_uniqueID;
            _mappedID[_uniqueID] = _uniqueID;
            return _uniqueID;
        }

        public int GetMetaUniqueID()
        {
            ++_metaUniqueID;
            _metaMappedID[_metaUniqueID] = _metaUniqueID;
            return _metaUniqueID;
        }

        public int GetMetaMappedID(int metaEntityId)
        {
            int realId;
            if (_metaMappedID.TryGetValue(metaEntityId, out realId) == false)
            {
                realId              = GetMetaUniqueID();
                _metaMappedID[metaEntityId] = realId;
            }

            return realId;
        }
    }
}
