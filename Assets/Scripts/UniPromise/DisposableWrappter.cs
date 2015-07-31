using System;

namespace UniPromise {

	public class DisposableWrappter : IDisposable {
		Action dispose;
		
		public DisposableWrappter (Action dispose) {
			this.dispose = dispose;
		}
		
		public void Dispose () {
			dispose();
		}
	}

}