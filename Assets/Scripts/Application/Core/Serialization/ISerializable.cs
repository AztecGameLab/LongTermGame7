namespace Application.Core
{
    public interface ISerializable
    {
        string GetID();
        void ReadData(object data);
        object WriteData();
    }
}