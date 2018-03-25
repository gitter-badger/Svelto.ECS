namespace Svelto.ECS
{   
    public interface IEntityFactory
    {
        void PreallocateEntitySlots<T>(int size) where T : IEntityDescriptor, new();

        void BuildEntity<T>(int entityID, object[] implementors = null) where T:IEntityDescriptor, new();
        void BuildEntity(int entityID, IEntityDescriptorInfo entityDescriptorInfo, object[] implementors = null);
        void BuildEntity<T>(object[]              implementors                                = null) where T:IEntityDescriptor, new();
        void BuildEntity(IEntityDescriptorInfo entityDescriptorInfo, object[] implementors = null);

        void BuildMetaEntity<T>(int metaEntityID, object[] implementors = null) where T:IEntityDescriptor, new();
        void BuildMetaEntity<T>(object[] implementors = null) where T:IEntityDescriptor, new();

        void BuildEntityInGroup<T>(int entityID, int groupID, object[] implementors = null) where T:IEntityDescriptor, new();
        void BuildEntityInGroup(int entityID, int groupID, IEntityDescriptorInfo entityDescriptor, object[] implementors = null);
        void BuildEntityInGroup<T>(int groupID, object[] implementors = null) where T:IEntityDescriptor, new();
        void BuildEntityInGroup(int groupID, IEntityDescriptorInfo entityDescriptor, object[] implementors = null);
    }
    
    public interface IEntityFunctions
    {
        void RemoveEntity(int entityID);

        void RemoveMetaEntity(int metaEntityID);

        void RemoveGroupAndEntities(int groupID);
        
        void SwapEntityGroup(int entityID, int fromGroupID, int toGroupID);
    }
}
