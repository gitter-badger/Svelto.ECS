using System;
using Svelto.ECS.Internal;

namespace Svelto.ECS
{
    public interface IEntityViewBuilder
    {
        void          BuildEntityViewAndAddToList(ref ITypeSafeList list, int entityID, out IEntityView entityView);
        ITypeSafeList Preallocate(ref                 ITypeSafeList list, int size);

        Type[] GetEntityViewType();
        void MoveEntityView(int entityID, ITypeSafeList fromSafeList, ITypeSafeList toSafeList);
        bool mustBeFilled { get; }
    }
}