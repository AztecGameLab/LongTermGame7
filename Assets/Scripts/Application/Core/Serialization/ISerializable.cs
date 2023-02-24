namespace Application.Core.Serialization
{
    public interface ISerializable
    {
        string GetID();
        void ReadData(object data);
        object WriteData();
    }
}