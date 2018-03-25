using Svelto.DataStructures;

namespace Svelto.ECS
{
    public interface IEntityViewsDB
    {
        FasterReadOnlyList<T> QueryEntityViews<T>() where T : class, IEntityView;
        FasterReadOnlyList<T> QueryMetaEntityViews<T>() where T : class, IEntityView;
        FasterReadOnlyList<T> QueryGroupedEntityViews<T>(int group) where T : class, IEntityView;

        T[] QueryEntityViewsAsArray<T>(out int    count) where T : IEntityView;
        T[] QueryGroupedEntityViewsAsArray<T>(int group, out int count) where T : IEntityView;

        ReadOnlyDictionary<int, T> QueryIndexableEntityViews<T>() where T : class, IEntityView;
        ReadOnlyDictionary<int, T> QueryIndexableMetaEntityViews<T>() where T : class, IEntityView;

        bool TryQueryEntityView<T>(int ID, out T entityView) where T : class, IEntityView;
        T    QueryEntityView<T>(int    ID) where T : class, IEntityView;

        bool TryQueryMetaEntityView<T>(int metaEntityID, out T entityView) where T : class, IEntityView;
        T    QueryMetaEntityView<T>(int    metaEntityID) where T : class, IEntityView;
    }
}