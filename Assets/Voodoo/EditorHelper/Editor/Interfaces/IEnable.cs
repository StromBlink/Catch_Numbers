namespace Voodoo.Utils
{
	public interface IEnable
	{
		void OnEnable();
		void OnDisable();
	}
	
	public interface IEnable<T>
	{
		void OnEnable(T value);
		void OnDisable(T value);
	}
}