namespace Asce.SaveLoads
{
	public interface ISaveable<T>
	{
		bool IsNeedSave => true;

        T Save();
	}
}