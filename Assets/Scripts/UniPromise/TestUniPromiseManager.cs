using UniPromise.Internal;


namespace UniPromise {
	public class TestUniPromiseManager : IUniPromiseManager {
		CallbackUpdater callbackUpdater;

		public TestUniPromiseManager() {
			callbackUpdater = new CallbackUpdater();
		}

		public void OverrideManager() {
			UniPromiseManager.Reset(this);
		}

		public void AddCallback (System.Action callback) {
			callbackUpdater.AddCallback(callback);
		}

		public void Update() {
			callbackUpdater.Update();
		}
	}
}
