using System;
using Svelto.ECS.Internal;

namespace Svelto.ECS
{
    public class EntityBuilder<Entity> : IEntityViewBuilder where Entity : EntityView, IEntity, new()
    {
        public EntityBuilder()
        {
            _entityViewTypes = new Entity().entityViewTypes;
        }
        
        public void BuildEntityViewAndAddToList(ref ITypeSafeList list, int entityID, out IEntityView entityView)
        {
            if (list == null)
                list = new TypeSafeFasterListForECSForClasses<Entity>();

            var castedList = list as TypeSafeFasterListForECSForClasses<Entity>;

            var lentityView = EntityView<Entity>.BuildEntityView(entityID);

            castedList.Add(lentityView);

            entityView = lentityView;
        }

        public ITypeSafeList Preallocate(ref ITypeSafeList list, int size)
        {
            if (list == null)
                list = new TypeSafeFasterListForECSForClasses<Entity>(size);
            else
                list.ReserveCapacity(size);

            return list;
        }

        public Type[] GetEntityViewType()
        {
            return _entityViewTypes;
        }

        public void MoveEntityView(int entityID, ITypeSafeList fromSafeList, ITypeSafeList toSafeList)
        {
            var fromCastedList = fromSafeList as TypeSafeFasterListForECSForClasses<Entity>;
            var toCastedList   = toSafeList as TypeSafeFasterListForECSForClasses<Entity>;

            toCastedList.Add(fromCastedList[fromCastedList.GetIndexFromID(entityID)]);
        }

        public bool mustBeFilled
        {
            get { return true; }
        }

        Type[] _entityViewTypes;
    }

    public interface IEntity
    {
        Type[] entityViewTypes {get;}
    }
}