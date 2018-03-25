using System;
using Svelto.ECS.Internal;

namespace Svelto.ECS
{
    public class EntityViewStructBuilder<EntityViewType> : IEntityViewBuilder where EntityViewType : struct, IEntityStruct
    {
        public void BuildEntityViewAndAddToList(ref ITypeSafeList list, int entityID, out IEntityView entityView)
        {
            var structEntityView = default(EntityViewType);
            structEntityView.ID = entityID;
            
            if (list == null)
                list = new TypeSafeFasterListForECSForStructs<EntityViewType>();

            var castedList = list as TypeSafeFasterListForECSForStructs<EntityViewType>;

            castedList.Add(structEntityView);

            entityView = null;
        }

        public ITypeSafeList Preallocate(ref ITypeSafeList list, int size)
        {
            if (list == null)
                list = new TypeSafeFasterListForECSForStructs<EntityViewType>(size);
            else
                list.ReserveCapacity(size);

            return list;
        }

        public Type[] GetEntityViewType()
        {
            return _entityViewType;
        }

        public void MoveEntityView(int entityID, ITypeSafeList fromSafeList, ITypeSafeList toSafeList)
        {
            var fromCastedList = fromSafeList as TypeSafeFasterListForECSForStructs<EntityViewType>;
            var toCastedList   = toSafeList as TypeSafeFasterListForECSForStructs<EntityViewType>;

            toCastedList.Add(fromCastedList[fromCastedList.GetIndexFromID(entityID)]);
        }

        public bool mustBeFilled
        {
            get { return false; }
        }

        readonly Type[] _entityViewType = {typeof(EntityViewType)};
    }
}