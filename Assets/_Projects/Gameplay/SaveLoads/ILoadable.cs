namespace Asce.SaveLoads
{
	public interface ILoadable<T>
	{
		void Load(T data);
	}
}