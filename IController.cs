namespace Grigorov.Unity.Controllers {
	public interface IController {
		bool IsActive { get; }
		
		void OnInit();
		void OnReset();
	}
}