namespace Asce.Game.SaveLoads
{
	public interface ILoadable<T>
	{
		void Load(T data);
	}
}